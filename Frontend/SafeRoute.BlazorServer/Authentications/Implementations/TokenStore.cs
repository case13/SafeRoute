using SafeRoute.BlazorServer.Authentications.Interfaces;

namespace SafeRoute.BlazorServer.Authentications.Implementations
{
    public class TokenStore : ITokenStore
    {
        public string AccessToken { get; private set; } = string.Empty;
        public string RefreshToken { get; private set; } = string.Empty;

        public void Set(string accessToken, string refreshToken)
        {
            AccessToken = accessToken ?? string.Empty;
            RefreshToken = refreshToken ?? string.Empty;
        }

        public void Clear()
        {
            AccessToken = string.Empty;
            RefreshToken = string.Empty;
        }
    }
}
