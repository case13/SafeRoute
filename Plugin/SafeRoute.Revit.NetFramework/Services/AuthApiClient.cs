using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SafeRoute.Revit.NetFramework.Services
{
    public class AuthApiClient
    {
        private readonly string _baseUrl;

        public string AccessToken { get; private set; } = "";

        public AuthApiClient(string baseUrl)
        {
            _baseUrl = (baseUrl ?? "").Trim().TrimEnd('/') + "/";
        }

        public async Task<bool> LoginAsync(string email, string password)
        {
            using (var http = new HttpClient())
            {
                http.BaseAddress = new Uri(_baseUrl);

                var payload = new
                {
                    email = email,
                    password = password
                };

                var json = JsonConvert.SerializeObject(payload);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await http.PostAsync("api/auth/login", content);
                if (!response.IsSuccessStatusCode)
                    return false;

                var body = await response.Content.ReadAsStringAsync();

                var obj = JsonConvert.DeserializeObject<LoginResponse>(body);
                if (obj == null || string.IsNullOrWhiteSpace(obj.AccessToken))
                    return false;

                AccessToken = obj.AccessToken;
                return true;
            }
        }

        private class LoginResponse
        {
            [JsonProperty("accessToken")]
            public string AccessToken { get; set; } = "";
        }
    }
}
