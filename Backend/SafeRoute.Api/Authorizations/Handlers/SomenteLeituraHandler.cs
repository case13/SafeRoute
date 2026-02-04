using Microsoft.AspNetCore.Authorization;
using SafeRoute.Api.Authorizations.Requeriments;
using SafeRoute.Shared.Enums.Tipos;

namespace SafeRoute.Api.Authorizations.Handlers
{
    public class SomenteLeituraHandler
        : AuthorizationHandler<SomenteLeituraRequirement>
    {
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            SomenteLeituraRequirement requirement)
        {
            if (!context.User.Identity?.IsAuthenticated ?? true)
                return Task.CompletedTask;

            var tipoUsuarioClaim =
                context.User.FindFirst("tipo_usuario")?.Value;

            if (!Enum.TryParse<UserTypeEnum>(tipoUsuarioClaim, out var tipoUsuario))
                return Task.CompletedTask;

            // Todos podem ler
            context.Succeed(requirement);

            return Task.CompletedTask;
        }
    }
}
