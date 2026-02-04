using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SafeRoute.Application.Services.Interfaces;
using SafeRoute.Shared.Authorizations;
using SafeRoute.Shared.Dtos.Common;
using SafeRoute.Shared.Dtos.Rules;

namespace SafeRoute.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Policy = PolicyNames.AdminOuUsuario)]
    public class RuleViolationsController : ControllerBase
    {
        private readonly IRuleViolationService _service;

        public RuleViolationsController(IRuleViolationService service)
        {
            _service = service;
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ReadRuleViolationDto>> GetById(int id)
        {
            var result = await _service.GetByIdAsync(id);
            if (result == null || result.Id == 0) 
                return NotFound();
            return Ok(result);
        }

        [HttpGet("paged")]
        public async Task<ActionResult<PagedResultDto<ReadRuleViolationDto>>> GetPaged(
            [FromQuery] int projectId,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? filterColumn = null,
            [FromQuery] string? filterText = null)
        {
            if (projectId <= 0) 
                return BadRequest("projectId é obrigatório.");

            var result = await _service.GetPagedAsync(pageNumber, pageSize, projectId, filterColumn, filterText);
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<ReadRuleViolationDto>> Create([FromBody] CreateRuleViolationDto dto)
        {
            var result = await _service.CreateAsync(dto);
            if (result == null || result.Id == 0) return BadRequest();
            return Ok(result);
        }

        [HttpPost("batch")]
        public async Task<IActionResult> CreateBatch([FromBody] List<CreateRuleViolationDto> dtos)
        {
            var inserted = await _service.CreateBatchAsync(dtos);
            if (inserted <= 0) 
                return BadRequest();
            return Ok(new { inserted });
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<ReadRuleViolationDto>> Update(int id, [FromBody] UpdateRuleViolationDto dto)
        {
            var result = await _service.UpdateAsync(id, dto);
            if (result == null || result.Id == 0) 
                return NotFound();
            return Ok(result);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var ok = await _service.DeleteAsync(id);
            if (!ok) 
                return NotFound();
            return NoContent();
        }
    }
}
