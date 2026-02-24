using SafeRoute.Shared.Dtos.Common;
using SafeRoute.Shared.Dtos.Company;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SafeRoute.Application.Services.Interfaces
{
    public interface ICompanyService : IBaseService<ReadCompanyDto, CreateCompanyDto, UpdateCompanyDto>
    {
        Task<ReadCompanyDto> GetByRegistryAsync(string registry);
        Task<PagedResultDto<ReadCompanyDto>> GetPagedAsync(
            int pageNumber,
            int pageSize,
            string? filterColumn,
            string? filterText);
    }
}
