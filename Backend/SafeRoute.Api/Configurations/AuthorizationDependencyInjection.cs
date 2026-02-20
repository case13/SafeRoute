using Microsoft.AspNetCore.Authorization;
using SafeRoute.Api.Authorizations.Handlers;
using SafeRoute.Api.Authorizations.Requeriments;
using SafeRoute.Shared.Authorizations;
using SafeRoute.Shared.Enums.Tipos;


namespace SafeRoute.Api.Configurations
{
    public static class AuthorizationDependencyInjection
    {
        public static IServiceCollection AddAuthorizationConfiguration(
            this IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                // Somente Administrador
                options.AddPolicy(PolicyNames.Administrador, policy =>
                    policy.Requirements.Add(
                        new TipoUsuarioRequirement(
                            UserTypeEnum.Administrador)));

                // Administrador ou Usuário Comum
                options.AddPolicy(PolicyNames.AdminOuUsuario, policy =>
                    policy.Requirements.Add(
                        new TipoUsuarioRequirement(
                            UserTypeEnum.Administrador,
                            UserTypeEnum.UsuarioComum)));

                // Usuário Comum (Administrador também passa)
                options.AddPolicy(PolicyNames.UsuarioComum, policy =>
                    policy.Requirements.Add(
                        new TipoUsuarioRequirement(
                            UserTypeEnum.UsuarioComum,
                            UserTypeEnum.Administrador)));

                // Somente Leitura 
                options.AddPolicy(PolicyNames.SomenteLeitura, policy =>
                    policy.Requirements.Add(new SomenteLeituraRequirement()));
            });

            // Handler das policies
            services.AddScoped<IAuthorizationHandler, TipoUsuarioHandler>();
            services.AddScoped<IAuthorizationHandler, SomenteLeituraHandler>();

            return services;
        }
    }
}
