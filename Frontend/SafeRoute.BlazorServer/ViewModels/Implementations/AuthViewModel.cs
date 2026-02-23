using SafeRoute.BlazorServer.Services.Interfaces;
using SafeRoute.BlazorServer.ViewModels.Interfaces;
using SafeRoute.Shared.Dtos.Auth;
using System.Threading.Tasks;

namespace SafeRoute.BlazorServer.ViewModels.Implementations
{
    public class AuthViewModel : IAuthViewModel
    {
        private readonly IAuthHttpService _authHttpService;

        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;

        public bool IsBusy { get; private set; }
        public string Error { get; private set; } = string.Empty;

        public string CurrentPassword { get; set; } = string.Empty;
        public string NewPassword { get; set; } = string.Empty;
        public string ConfirmNewPassword { get; set; } = string.Empty;

        public string ChangePasswordError { get; private set; } = string.Empty;
        public string ChangePasswordSuccess { get; private set; } = string.Empty;

        public AuthViewModel(IAuthHttpService authHttpService)
        {
            _authHttpService = authHttpService;
        }

        public async Task<bool> LoginAsync()
        {
            Error = string.Empty;
            IsBusy = true;

            try
            {
                if (string.IsNullOrWhiteSpace(Email) ||
                    string.IsNullOrWhiteSpace(Password))
                {
                    Error = "Informe email e senha.";
                    return false;
                }

                var result = await _authHttpService.LoginAsync(Email, Password);

                if (result == null || string.IsNullOrWhiteSpace(result.AccessToken))
                {
                    Error = "Email ou senha inválidos.";
                    return false;
                }

                return true;
            }
            finally
            {
                IsBusy = false;
            }
        }

        public async Task LogoutAsync()
        {
            await _authHttpService.LogoutAsync();
        }

        public async Task<bool> IsAuthenticatedAsync()
        {
            var token = await _authHttpService.GetAccessTokenAsync();
            return !string.IsNullOrWhiteSpace(token);
        }

        public void ClearChangePassword()
        {
            CurrentPassword = string.Empty;
            NewPassword = string.Empty;
            ConfirmNewPassword = string.Empty;
            ChangePasswordError = string.Empty;
            ChangePasswordSuccess = string.Empty;
        }

        public async Task<bool> ChangePasswordAsync()
        {
            ChangePasswordError = string.Empty;
            ChangePasswordSuccess = string.Empty;

            if (string.IsNullOrWhiteSpace(CurrentPassword) ||
                string.IsNullOrWhiteSpace(NewPassword) ||
                string.IsNullOrWhiteSpace(ConfirmNewPassword))
            {
                ChangePasswordError = "Preencha todos os campos.";
                return false;
            }

            if (NewPassword != ConfirmNewPassword)
            {
                ChangePasswordError = "A confirmação não confere.";
                return false;
            }

            IsBusy = true;

            try
            {
                var dto = new ChangePasswordRequestDto
                {
                    CurrentPassword = CurrentPassword,
                    NewPassword = NewPassword,
                    ConfirmNewPassword = ConfirmNewPassword
                };

                var ok = await _authHttpService.ChangePasswordAsync(dto);

                if (!ok)
                {
                    ChangePasswordError = "Não foi possível alterar a senha.";
                    return false;
                }

                ClearChangePassword();
                ChangePasswordSuccess = "Senha alterada com sucesso.";
                return true;
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}