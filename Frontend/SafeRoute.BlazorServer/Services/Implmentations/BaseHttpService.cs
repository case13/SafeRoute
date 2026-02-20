using SafeRoute.BlazorServer.Services.Interfaces;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;

namespace SafeRoute.BlazorServer.Services.Implementations
{
    public abstract class BaseHttpService : IBaseHttpService
    {
        protected readonly HttpClient Http;
        protected readonly IAuthHttpService AuthHttpService;

        protected BaseHttpService(
            IHttpClientFactory httpClientFactory,
            IAuthHttpService authHttpService)
        {
            Http = httpClientFactory.CreateClient("Api");
            AuthHttpService = authHttpService;
        }

        protected virtual async Task EnsureAuthHeaderAsync()
        {
            var token = await AuthHttpService.GetAccessTokenAsync();

            if (!string.IsNullOrWhiteSpace(token))
            {
                Http.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);
            }
        }

        protected virtual async Task<T> GetAsync<T>(string url, CancellationToken ct = default)
            where T : new()
        {
            try
            {
                await EnsureAuthHeaderAsync();

                var response = await Http.GetAsync(url, ct);
                if (!response.IsSuccessStatusCode)
                    return new T();

                return await response.Content.ReadFromJsonAsync<T>(cancellationToken: ct)
                       ?? new T();
            }
            catch
            {
                return new T();
            }
        }

        protected virtual async Task<TResponse> PostAsync<TRequest, TResponse>(
            string url, TRequest body, CancellationToken ct = default)
            where TResponse : new()
        {
            try
            {
                await EnsureAuthHeaderAsync();

                var response = await Http.PostAsJsonAsync(url, body, ct);
                if (!response.IsSuccessStatusCode)
                    return new TResponse();

                return await response.Content.ReadFromJsonAsync<TResponse>(cancellationToken: ct)
                       ?? new TResponse();
            }
            catch
            {
                return new TResponse();
            }
        }

        protected virtual async Task<TResponse> PutAsync<TRequest, TResponse>(
            string url, TRequest body, CancellationToken ct = default)
            where TResponse : new()
        {
            try
            {
                await EnsureAuthHeaderAsync();

                var response = await Http.PutAsJsonAsync(url, body, ct);
                if (!response.IsSuccessStatusCode)
                    return new TResponse();

                return await response.Content.ReadFromJsonAsync<TResponse>(cancellationToken: ct)
                       ?? new TResponse();
            }
            catch
            {
                return new TResponse();
            }
        }

        protected virtual async Task<bool> DeleteAsync(string url, CancellationToken ct = default)
        {
            try
            {
                await EnsureAuthHeaderAsync();

                var response = await Http.DeleteAsync(url, ct);
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }
    }
}
