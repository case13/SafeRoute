using System.Threading.Tasks;

namespace SafeRoute.BlazorServer.ViewModels.Interfaces
{
    public interface IAuthViewModel
    {
        string Email { get; set; }
        string Password { get; set; }

        bool IsBusy { get; }
        string Error { get; }

        Task<bool> LoginAsync();
        Task LogoutAsync();
        Task<bool> IsAuthenticatedAsync();

        // Change Password
        string CurrentPassword { get; set; }
        string NewPassword { get; set; }
        string ConfirmNewPassword { get; set; }

        string ChangePasswordError { get; }
        string ChangePasswordSuccess { get; }

        Task<bool> ChangePasswordAsync();
        void ClearChangePassword();
    }
}