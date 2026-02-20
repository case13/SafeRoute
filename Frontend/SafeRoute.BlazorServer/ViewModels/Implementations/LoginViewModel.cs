using SafeRoute.BlazorServer.Services.Interfaces;
using SafeRoute.BlazorServer.ViewModels.Interfaces;
using System.Threading.Tasks;

namespace SafeRoute.BlazorServer.ViewModels.Implementations
{
    public class LoginViewModel : ILoginViewModel
    {
        private readonly IAuthHttpService _authHttpService;

        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;

        public bool IsBusy { get; private set; }
        public string Error { get; private set; } = string.Empty;

        public LoginViewModel(IAuthHttpService authHttpService)
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

                var ok = await _authHttpService.LoginAsync(Email, Password);

                if (!ok)
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
    }
}
