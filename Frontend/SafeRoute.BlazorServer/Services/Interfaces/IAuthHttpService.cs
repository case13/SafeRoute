using System.Threading.Tasks;

namespace SafeRoute.BlazorServer.Services.Interfaces
{
    public interface IAuthHttpService
    {
        Task<bool> LoginAsync(string email, string password);
        Task LogoutAsync();

        Task<string> GetAccessTokenAsync();
        Task<string> GetRefreshTokenAsync();
    }
}
