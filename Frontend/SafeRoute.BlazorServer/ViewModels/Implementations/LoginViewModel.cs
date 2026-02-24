using SafeRoute.BlazorServer.Services.Interfaces;
using SafeRoute.BlazorServer.ViewModels.Interfaces;
using System.Threading.Tasks;

namespace SafeRoute.BlazorServer.ViewModels.Implementations
{
    public class LoginViewModel : ILoginViewModel
    {
        private readonly IAuthHttpService _authHttpService;

        public bool RequiresPasswordSetup { get; private set; }
        public string PasswordSetupEmail { get; private set; } = string.Empty;
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
            RequiresPasswordSetup = false;
            PasswordSetupEmail = string.Empty;
            IsBusy = true;

            try
            {
                if (string.IsNullOrWhiteSpace(Email))
                {
                    Error = "Informe email.";
                    return false;
                }

                // senha pode vir vazia no primeiro acesso, mas no teu formulário ela existe
                if (string.IsNullOrWhiteSpace(Password))
                {
                    Error = "Informe a senha.";
                    return false;
                }

                var result = await _authHttpService.LoginAsync(Email, Password);

                if (result == null)
                {
                    Error = "Email ou senha inválidos.";
                    return false;
                }

                if (result.RequiresPasswordSetup)
                {
                    RequiresPasswordSetup = true;
                    PasswordSetupEmail = result.Email;
                    return true; // retorna true para o Page decidir a navegação
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
