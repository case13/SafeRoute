using SafeRoute.BlazorServer.Services.Interfaces;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace SafeRoute.BlazorServer.Authentications.Handlers
{
    public class BearerTokenHandler : DelegatingHandler
    {
        private readonly IAuthHttpService _auth;

        public BearerTokenHandler(IAuthHttpService auth)
        {
            _auth = auth;
        }

        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            if (request.Headers.Authorization == null)
            {
                var token = await _auth.GetAccessTokenAsync();

                Console.WriteLine("BearerTokenHandler token length: " + (token?.Length ?? 0));

                if (!string.IsNullOrWhiteSpace(token))
                {
                    request.Headers.Authorization =
                        new AuthenticationHeaderValue("Bearer", token);
                }
            }

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
