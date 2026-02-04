using SafeRoute.Domain.Entities;
using SafeRoute.Shared.Dtos.Common;
using SafeRoute.Shared.Dtos.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SafeRoute.Application.Services.Interfaces
{
    public interface IUserService
         : IBaseService<
             ReadUserDto,
             CreateUserDto,
             UpdateUserDto>
    {
        Task<ReadUserDto> GetByEmailAsync(string email);
        Task<User?> LoginAsync(string email, string senha);
        Task<PagedResultDto<ReadUserDto>> GetPagedAsync(
                int pageNumber, int pageSize, string? filterColumn, string? filterText);
    }
}
