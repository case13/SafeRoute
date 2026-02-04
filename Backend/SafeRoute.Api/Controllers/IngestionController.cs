using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SafeRoute.Application.Services.Interfaces;
using SafeRoute.Shared.Authorizations;
using SafeRoute.Shared.Dtos.Ingestion.Project;
using System.Threading.Tasks;

namespace SafeRoute.Api.Controllers
{
    [ApiController]
    [Route("api/ingestion")]
    [Authorize(Policy = PolicyNames.AdminOuUsuario)]
    public class IngestionController : ControllerBase
    {
        private readonly IProjectIngestionService _service;

        public IngestionController(IProjectIngestionService service)
        {
            _service = service;
        }

        [HttpPost("project/replace")]
        public async Task<IActionResult> ReplaceByProject([FromBody] IngestProjectElementsRequestDto dto)
        {
            var result = await _service.ReplaceByProjectAsync(dto);
            if (result == null || result.ProjectId <= 0) return BadRequest();
            return Ok(result);
        }
    }
}
