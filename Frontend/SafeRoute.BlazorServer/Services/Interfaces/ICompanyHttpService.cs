using SafeRoute.Shared.Dtos.Common;
using SafeRoute.Shared.Dtos.Company;

namespace SafeRoute.BlazorServer.Services.Interfaces
{
    public interface ICompanyHttpService
    {
        Task<PagedResultDto<ReadCompanyDto>> GetPagedAsync(
            int pageNumber,
            int pageSize,
            string filterColumn,
            string filterText);

        Task<IEnumerable<ReadCompanyDto>> GetAllAsync();

        Task<ReadCompanyDto> GetByIdAsync(int id);

        Task<ReadCompanyDto> CreateAsync(CreateCompanyDto dto);

        Task<ReadCompanyDto> UpdateAsync(int id, UpdateCompanyDto dto);

        Task<bool> DeleteAsync(int id);
    }
}
