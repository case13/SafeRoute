using SafeRoute.BlazorServer.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
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

        protected virtual async Task<TDto?> GetAsync<TDto>(
            string url,
            CancellationToken ct = default)
        {
            try
            {
                await EnsureAuthHeaderAsync();

                var response = await Http.GetAsync(url, ct);
                if (!response.IsSuccessStatusCode)
                    return default;

                return await response.Content.ReadFromJsonAsync<TDto>(cancellationToken: ct);
            }
            catch
            {
                return default;
            }
        }

        protected virtual async Task<IEnumerable<TDto>> GetAllAsync<TDto>(
            string url,
            CancellationToken ct = default)
        {
            try
            {
                await EnsureAuthHeaderAsync();

                var response = await Http.GetAsync(url, ct);
                if (!response.IsSuccessStatusCode)
                    return Enumerable.Empty<TDto>();

                return await response.Content.ReadFromJsonAsync<IEnumerable<TDto>>(cancellationToken: ct)
                       ?? Enumerable.Empty<TDto>();
            }
            catch
            {
                return Enumerable.Empty<TDto>();
            }
        }

        protected virtual async Task<TResponseDto?> PostAsync<TRequestDto, TResponseDto>(
            string url,
            TRequestDto body,
            CancellationToken ct = default)
        {
            try
            {
                await EnsureAuthHeaderAsync();

                var response = await Http.PostAsJsonAsync(url, body, ct);
                if (!response.IsSuccessStatusCode)
                    return default;

                return await response.Content.ReadFromJsonAsync<TResponseDto>(cancellationToken: ct);
            }
            catch
            {
                return default;
            }
        }

        protected virtual async Task<TResponseDto?> PutAsync<TRequestDto, TResponseDto>(
            string url,
            TRequestDto body,
            CancellationToken ct = default)
        {
            try
            {
                await EnsureAuthHeaderAsync();

                var response = await Http.PutAsJsonAsync(url, body, ct);
                if (!response.IsSuccessStatusCode)
                    return default;

                return await response.Content.ReadFromJsonAsync<TResponseDto>(cancellationToken: ct);
            }
            catch
            {
                return default;
            }
        }

        protected virtual async Task<bool> DeleteAsync(
            string url,
            CancellationToken ct = default)
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