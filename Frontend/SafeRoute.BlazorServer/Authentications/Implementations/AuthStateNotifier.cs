using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using SafeRoute.BlazorServer.Authentications.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SafeRoute.BlazorServer.Authentications.Implementations
{
    public class AuthStateNotifier :
        AuthenticationStateProvider,
        IAuthStateNotifier
    {
        private readonly ProtectedSessionStorage _session;

        public AuthStateNotifier(ProtectedSessionStorage session)
        {
            _session = session;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var result = await _session.GetAsync<string>("access_token");

            if (!result.Success || string.IsNullOrWhiteSpace(result.Value))
                return Anonymous();

            return BuildAuthenticationState(result.Value);
        }

        public void NotifyUserAuthentication(string accessToken)
        {
            NotifyAuthenticationStateChanged(
                Task.FromResult(BuildAuthenticationState(accessToken)));
        }

        public void NotifyUserLogout()
        {
            NotifyAuthenticationStateChanged(
                Task.FromResult(Anonymous()));
        }

        private AuthenticationState BuildAuthenticationState(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(token);

            foreach (var c in jwt.Claims)
            {
                Console.WriteLine($"CLAIM => {c.Type} = {c.Value}");
            }

            var identity = new ClaimsIdentity(jwt.Claims, "jwt");
            var user = new ClaimsPrincipal(identity);

            return new AuthenticationState(user);
        }

        private AuthenticationState Anonymous()
        {
            return new AuthenticationState(
                new ClaimsPrincipal(new ClaimsIdentity()));
        }
    }
}
