using Newtonsoft.Json;
using SafeRoute.Shared.Dtos.Common;
using SafeRoute.Shared.Dtos.Ingestion.Project;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace SafeRoute.Revit.NetFramework.Services
{
    public class SafeRouteApiClient
    {
        private readonly string _baseUrl;
        private readonly string _token;

        public SafeRouteApiClient(string baseUrl, string token)
        {
            _baseUrl = (baseUrl ?? "").Trim().TrimEnd('/') + "/";
            _token = token ?? "";
        }

        public async Task<ReplaceResultDto> ReplaceProjectAsync(IngestProjectElementsRequestDto dto)
        {
            using (var http = new HttpClient())
            {
                http.BaseAddress = new Uri(_baseUrl);

                if (!string.IsNullOrWhiteSpace(_token))
                {
                    http.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue("Bearer", _token);
                }

                var json = JsonConvert.SerializeObject(dto);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await http.PostAsync("api/ingestion/project/replace", content);
                if (!response.IsSuccessStatusCode)
                    return null;

                var body = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<ReplaceResultDto>(body);
            }
        }
    }
}
