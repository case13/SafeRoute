using Autodesk.Revit.UI;
using SafeRoute.Revit.NetFramework.Services;
using SafeRoute.Revit.NetFramework.Settings;
using System;
using System.Windows;
using System.Windows.Interop;

namespace SafeRoute.Revit.NetFramework.Views
{
    public partial class LoginWindow : Window
    {
        public string BaseUrl { get; private set; } = "";
        public string Email { get; private set; } = "";
        public string AccessToken { get; private set; } = "";

        private readonly PluginSettings _settings;
        private readonly UIApplication _uiApp;

        public LoginWindow(UIApplication uiApp)
        {
            InitializeComponent();

            _uiApp = uiApp;

            var helper = new WindowInteropHelper(this);
            helper.Owner = _uiApp.MainWindowHandle;

            _settings = PluginSettingsProvider.Load();

            BaseUrlTextBox.Text = _settings.BaseUrl ?? "https://localhost:7030/";
            EmailTextBox.Text = _settings.LastEmail ?? "";
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private async void Login_Click(object sender, RoutedEventArgs e)
        {
            StatusText.Text = "";

            BaseUrl = (BaseUrlTextBox.Text ?? "").Trim();
            Email = (EmailTextBox.Text ?? "").Trim();
            var password = PasswordBox.Password ?? "";

            if (string.IsNullOrWhiteSpace(BaseUrl) ||
                string.IsNullOrWhiteSpace(Email) ||
                string.IsNullOrWhiteSpace(password))
            {
                StatusText.Text = "Preencha BaseUrl, Email e Senha.";
                return;
            }

            try
            {
                var authClient = new AuthApiClient(BaseUrl);

                var ok = await authClient.LoginAsync(Email, password);
                if (!ok || string.IsNullOrWhiteSpace(authClient.AccessToken))
                {
                    StatusText.Text = "Falha na autenticação.";
                    return;
                }

                AccessToken = authClient.AccessToken;

                _settings.BaseUrl = BaseUrl;
                _settings.LastEmail = Email;
                PluginSettingsProvider.Save(_settings);

                DialogResult = true;
                Close();
            }
            catch
            {
                StatusText.Text = "Ocorreu um erro ao enviar a solicitação.";
            }
        }
    }
}
