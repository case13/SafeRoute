using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SafeRoute.Application.Services.Interfaces;
using SafeRoute.Shared.Authorizations;
using SafeRoute.Shared.Dtos.Common;
using SafeRoute.Shared.Dtos.Company;

namespace SafeRoute.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Policy = PolicyNames.Administrador)]
    public class CompanyController : ControllerBase
    {
        private readonly ILogger<CompanyController> _logger;
        private readonly ICompanyService _service;

        public CompanyController(ILogger<CompanyController> logger, ICompanyService service)
        {
            _logger = logger;
            _service = service;
        }

        [HttpGet]
        [Route("{registry}")]
        public async Task<ActionResult<ReadCompanyDto>> GetByRegistryAsync(string registry)
        {
            var company = await _service.GetByRegistryAsync(registry);
            if (company == null)
                return NotFound();
            return Ok(company);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReadCompanyDto>>> GetAllAsync()
        {
            var companies = await _service.GetAllAsync();
            return Ok(companies);
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<ActionResult<ReadCompanyDto>> GetByIdAsync(int id)
        {
            var company = await _service.GetByIdAsync(id);
            if (company == null)
                return NotFound();
            return Ok(company);
        }

        [HttpGet]
        [Route("paged")]
        public async Task<ActionResult<PagedResultDto<ReadCompanyDto>>> GetPagedAsync(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? filterColumn = null,
            [FromQuery] string? filterText = null)
        {
            var result = await _service.GetPagedAsync(pageNumber, pageSize, filterColumn, filterText);
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<ReadCompanyDto>> CreateAsync([FromBody] CreateCompanyDto dto)
        {
            var createdCompany = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetByIdAsync), new { id = createdCompany.Id }, createdCompany);
        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<ActionResult<ReadCompanyDto>> UpdateAsync(int id, [FromBody] UpdateCompanyDto dto)
        {
            var updatedCompany = await _service.UpdateAsync(id, dto);
            if (updatedCompany == null)
                return NotFound();
            return Ok(updatedCompany);
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var deleted = await _service.DeleteAsync(id);
            if (!deleted)
                return NotFound();
            return NoContent();
        }

    }
}