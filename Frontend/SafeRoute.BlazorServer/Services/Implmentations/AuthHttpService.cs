using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using SafeRoute.BlazorServer.Authentications.Interfaces;
using SafeRoute.BlazorServer.Services.Interfaces;
using SafeRoute.Shared.Dtos.Auth;
using System;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace SafeRoute.BlazorServer.Services.Implementations
{
    public class AuthHttpService : IAuthHttpService
    {
        private const string AccessTokenKey = "access_token";
        private const string RefreshTokenKey = "refresh_token";

        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ProtectedSessionStorage _session;
        private readonly ITokenStore _tokenStore;
        private readonly IAuthStateNotifier _authStateNotifier;

        public AuthHttpService(
            IHttpClientFactory httpClientFactory,
            ProtectedSessionStorage session,
            ITokenStore tokenStore,
            IAuthStateNotifier authStateNotifier)
        {
            _httpClientFactory = httpClientFactory;
            _session = session;
            _tokenStore = tokenStore;
            _authStateNotifier = authStateNotifier;
        }

        public async Task<bool> LoginAsync(string email, string password)
        {
            email = (email ?? "").Trim();
            password = (password ?? "").Trim();

            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
                return false;

            var http = _httpClientFactory.CreateClient("ApiAuth");

            var request = new LoginRequestDto
            {
                Email = email,
                Password = password
            };

            var response = await http.PostAsJsonAsync("api/auth/login", request);
            if (!response.IsSuccessStatusCode)
                return false;

            var data = await response.Content.ReadFromJsonAsync<LoginResultDto>();
            if (data == null || string.IsNullOrWhiteSpace(data.AccessToken))
                return false;

            var accessToken = data.AccessToken ?? "";
            var refreshToken = data.RefreshToken ?? "";

            try
            {
                await _session.SetAsync(AccessTokenKey, accessToken);
                await _session.SetAsync(RefreshTokenKey, refreshToken);
            }
            catch (InvalidOperationException)
            {
                // Pode acontecer em prerender (JS interop indisponível).
                // Mantém ao menos em memória via TokenStore.
            }

            _tokenStore.Set(accessToken, refreshToken);
            _authStateNotifier.NotifyUserAuthentication(accessToken);

            return true;
        }

        public async Task LogoutAsync()
        {
            try
            {
                await _session.DeleteAsync(AccessTokenKey);
                await _session.DeleteAsync(RefreshTokenKey);
            }
            catch (InvalidOperationException)
            {
            }

            _tokenStore.Clear();
            _authStateNotifier.NotifyUserLogout();
        }

        public async Task<string> GetAccessTokenAsync()
        {
            try
            {
                var result = await _session.GetAsync<string>(AccessTokenKey);
                return result.Success ? (result.Value ?? "") : "";
            }
            catch (InvalidOperationException)
            {
                return _tokenStore.AccessToken ?? "";
            }
        }

        public async Task<string> GetRefreshTokenAsync()
        {
            try
            {
                var result = await _session.GetAsync<string>(RefreshTokenKey);
                return result.Success ? (result.Value ?? "") : "";
            }
            catch (InvalidOperationException)
            {
                return _tokenStore.RefreshToken ?? "";
            }
        }
    }
}
