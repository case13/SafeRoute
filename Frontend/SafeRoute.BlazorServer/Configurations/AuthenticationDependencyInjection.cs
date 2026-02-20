using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.Extensions.DependencyInjection;
using SafeRoute.BlazorServer.Authentications.Implementations;
using SafeRoute.BlazorServer.Authentications.Interfaces;

namespace SafeRoute.BlazorServer.Configurations
{
    public static class AuthenticationDependencyInjection
    {
        public static IServiceCollection AddAuthenticationConfiguration(this IServiceCollection services)
        {
            services.AddAuthorizationCore();

            services.AddScoped<ProtectedSessionStorage>();

            services.AddScoped<ITokenStore, TokenStore>();

            services.AddScoped<AuthStateNotifier>();
            services.AddScoped<IAuthStateNotifier>(sp => sp.GetRequiredService<AuthStateNotifier>());
            services.AddScoped<AuthenticationStateProvider>(sp => sp.GetRequiredService<AuthStateNotifier>());

            return services;
        }
    }
}
