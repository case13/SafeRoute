namespace SafeRoute.BlazorServer.Authentications.Interfaces
{
    public interface ITokenStore
    {
        string AccessToken { get; }
        string RefreshToken { get; }

        void Set(string accessToken, string refreshToken);
        void Clear();
    }
}
