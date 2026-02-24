using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SafeRoute.BlazorServer.Authentications;
using SafeRoute.BlazorServer.Authentications.Handlers;
using System;

namespace SafeRoute.BlazorServer.Configurations
{
    public static class HttpClientDependencyInjection
    {
        public static IServiceCollection AddApiHttpClient(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddHttpClient("Api", client =>
            {
                client.BaseAddress = new Uri(configuration["ApiSettings:BaseUrl"]);
            });

            services.AddHttpClient("ApiAuth", client =>
            {
                client.BaseAddress = new Uri(configuration["ApiSettings:BaseUrl"]);
            });

            return services;
        }
    }
}
