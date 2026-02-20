using Microsoft.Extensions.DependencyInjection;
using SafeRoute.Shared.Authorizations;
using SafeRoute.Shared.Enums.Tipos;

namespace SafeRoute.BlazorServer.Configurations
{
    public static class AuthorizationDependencyInjection
    {
        private const string ClaimUserType = "user_type";

        public static IServiceCollection AddAuthorizationConfiguration(this IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                options.AddPolicy(PolicyNames.Administrador, policy =>
                    policy.RequireClaim(ClaimUserType, UserTypeEnum.Administrador.ToString()));

                options.AddPolicy(PolicyNames.UsuarioComum, policy =>
                    policy.RequireClaim(ClaimUserType, UserTypeEnum.UsuarioComum.ToString()));

                options.AddPolicy(PolicyNames.AdminOuUsuario, policy =>
                    policy.RequireClaim(ClaimUserType,
                        UserTypeEnum.Administrador.ToString(),
                        UserTypeEnum.UsuarioComum.ToString()));

                options.AddPolicy(PolicyNames.SomenteLeitura, policy =>
                    policy.RequireClaim(ClaimUserType, UserTypeEnum.Convidado.ToString()));
            });

            return services;
        }
    }
}
