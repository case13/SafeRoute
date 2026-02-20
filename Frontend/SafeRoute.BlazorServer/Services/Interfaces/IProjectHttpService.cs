using SafeRoute.Shared.Dtos.Common;
using SafeRoute.Shared.Dtos.Project;
using System.Threading.Tasks;

namespace SafeRoute.BlazorServer.Services.Interfaces
{
    public interface IProjectHttpService
    {
        Task<PagedResultDto<ReadProjectDto>> GetPagedAsync(
            int pageNumber,
            int pageSize,
            string filterColumn,
            string filterText);

        Task<ReadProjectDto> GetByIdAsync(int id);

        Task<ReadProjectDto> CreateAsync(CreateProjectDto dto);

        Task<ReadProjectDto> UpdateAsync(int id, UpdateProjectDto dto);

        Task<bool> DeleteAsync(int id);
    }
}
