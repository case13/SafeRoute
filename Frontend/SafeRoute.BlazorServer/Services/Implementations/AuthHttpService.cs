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

        public async Task<bool> SetPasswordAsync(SetPasswordDto dto)
        {
            var http = _httpClientFactory.CreateClient("ApiAuth");

            var response = await http.PostAsJsonAsync("api/auth/set-password", dto);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> ChangePasswordAsync(ChangePasswordRequestDto dto)
        {
            var http = _httpClientFactory.CreateClient("Api");

            var token = await GetAccessTokenAsync();
            if (!string.IsNullOrWhiteSpace(token))
                http.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var response = await http.PutAsJsonAsync("api/auth/change-password", dto);
            return response.IsSuccessStatusCode;
        }

        public async Task<LoginResultDto?> LoginAsync(string email, string password)
        {
            email = (email ?? "").Trim();
            password = (password ?? "").Trim();

            if (string.IsNullOrWhiteSpace(email))
                return null;

            var http = _httpClientFactory.CreateClient("ApiAuth");

            var request = new LoginRequestDto
            {
                Email = email,
                Password = password
            };

            var response = await http.PostAsJsonAsync("api/auth/login", request);
            if (!response.IsSuccessStatusCode)
                return null;

            var data = await response.Content.ReadFromJsonAsync<LoginResultDto>();
            if (data == null)
                return null;
            
            if (data.RequiresPasswordSetup)
                return data;
            
            if (string.IsNullOrWhiteSpace(data.AccessToken))
                return null;

            var accessToken = data.AccessToken ?? "";
            var refreshToken = data.RefreshToken ?? "";

            try
            {
                await _session.SetAsync(AccessTokenKey, accessToken);
                await _session.SetAsync(RefreshTokenKey, refreshToken);
            }
            catch (InvalidOperationException)
            {
            }

            _tokenStore.Set(accessToken, refreshToken);
            _authStateNotifier.NotifyUserAuthentication(accessToken);

            return data;
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
