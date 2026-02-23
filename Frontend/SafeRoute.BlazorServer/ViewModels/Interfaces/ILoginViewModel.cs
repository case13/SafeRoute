using System.Threading.Tasks;

namespace SafeRoute.BlazorServer.ViewModels.Interfaces
{
    public interface ILoginViewModel
    {
        string Email { get; set; }
        string Password { get; set; }
        bool RequiresPasswordSetup { get; }
        string PasswordSetupEmail { get; }
        bool IsBusy { get; }
        string Error { get; }

        Task<bool> LoginAsync();
        Task LogoutAsync();
        Task<bool> IsAuthenticatedAsync();
    }
}
