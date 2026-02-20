using SafeRoute.Shared.Dtos.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SafeRoute.Application.Services.Interfaces
{
    public interface IAuthService
    {
        Task<LoginResultDto> LoginAsync(LoginRequestDto dto);
        Task<LoginResultDto> RefreshAsync(string refreshToken);
    }
}
