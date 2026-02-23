using SafeRoute.Shared.Dtos.Common;
using SafeRoute.Shared.Dtos.User;

namespace SafeRoute.BlazorServer.Services.Interfaces
{
    public interface IUserHttpService
    {
        Task<PagedResultDto<ReadUserDto>> GetPagedAsync(
            int pageNumber,
            int pageSize,
            string filterColumn,
            string filterText);

        Task<ReadUserDto> GetByIdAsync(int id);

        Task<ReadUserDto> CreateAsync(CreateUserDto dto);

        Task<ReadUserDto> UpdateAsync(int id, UpdateUserDto dto);

        Task<bool> DeleteAsync(int id);
    }
}
