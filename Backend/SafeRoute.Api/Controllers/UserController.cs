using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SafeRoute.Application.Services.Interfaces;
using SafeRoute.Shared.Authorizations;
using SafeRoute.Shared.Dtos.Common;
using SafeRoute.Shared.Dtos.User;

namespace SafeRoute.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Policy = PolicyNames.AdminOuUsuario)]
    public class UserController : ControllerBase
    {
        private readonly IUserService _service;
        private readonly ILogger<UserController> _logger;

        public UserController(IUserService service, ILogger<UserController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet("paged")]
        public async Task<ActionResult<PagedResultDto<ReadUserDto>>> GetPaged(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? filterColumn = null,
            [FromQuery] string? filterText = null)
        {
            var result = await _service.GetPagedAsync(pageNumber, pageSize, filterColumn, filterText);
            return Ok(result);
        }

        [HttpGet]
        public async Task<ActionResult<ReadUserDto>> GetAll()
        {
            var users = await _service.GetAllAsync();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ReadUserDto>> GetById(int id)
        {
            var user = await _service.GetByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [HttpPost]
        [Authorize(Policy = PolicyNames.Administrador)]
        public async Task<ActionResult<ReadUserDto>> Create([FromBody] CreateUserDto dto)
        {
            var createdUser = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = createdUser.Id }, createdUser);
        }

        [HttpPut("{id}")]
        [Authorize(Policy = PolicyNames.Administrador)]
        public async Task<ActionResult<ReadUserDto>> Update(int id, [FromBody] UpdateUserDto dto)
        {
            var updatedUser = await _service.UpdateAsync(id, dto);
            if (updatedUser == null)
            {
                return NotFound();
            }
            return Ok(updatedUser);
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = PolicyNames.Administrador)]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _service.DeleteAsync(id);
            if (!success)
            {
                return NotFound();
            }
            return NoContent();
        }

    }
}
