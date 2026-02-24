using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SafeRoute.Application.Services.Interfaces;
using SafeRoute.Shared.Authorizations;
using SafeRoute.Shared.Dtos.Common;
using SafeRoute.Shared.Dtos.Project;

namespace SafeRoute.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Policy = PolicyNames.AdminOuUsuario)]
    public class ProjectsController : ControllerBase
    {
        private readonly IProjectService _service;

        public ProjectsController(IProjectService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReadProjectDto>>> GetAll()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ReadProjectDto>> GetById(int id)
        {
            var result = await _service.GetByIdAsync(id);
            if (result == null || result.Id == 0) return NotFound();
            return Ok(result);
        }

        [HttpGet("paged")]
        public async Task<ActionResult<PagedResultDto<ReadProjectDto>>> GetPaged(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? filterColumn = null,
            [FromQuery] string? filterText = null)
        {
            var result = await _service.GetPagedAsync(pageNumber, pageSize, filterColumn, filterText);
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<ReadProjectDto>> Create([FromBody] CreateProjectDto dto)
        {
            var result = await _service.CreateAsync(dto);
            if (result == null || result.Id == 0) return BadRequest();
            return Ok(result);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<ReadProjectDto>> Update(int id, [FromBody] UpdateProjectDto dto)
        {
            var result = await _service.UpdateAsync(id, dto);
            if (result == null || result.Id == 0) return NotFound();
            return Ok(result);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var ok = await _service.DeleteAsync(id);
            if (!ok) return NotFound();
            return NoContent();
        }
    }
}
