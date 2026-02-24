using SafeRoute.Shared.Dtos.Auth;
using System.Threading.Tasks;

namespace SafeRoute.BlazorServer.Services.Interfaces
{
    public interface IAuthHttpService
    {
        Task<LoginResultDto?> LoginAsync(string email, string password);
        Task LogoutAsync();
        Task<bool> SetPasswordAsync(SetPasswordDto dto);
        Task<bool> ChangePasswordAsync(ChangePasswordRequestDto dto);

        Task<string> GetAccessTokenAsync();
        Task<string> GetRefreshTokenAsync();
    }
}
