using Microsoft.AspNetCore.Authorization;
using SafeRoute.Api.Authorizations.Requeriments;
using SafeRoute.Shared.Enums.Tipos;

namespace SafeRoute.Api.Authorizations.Handlers
{
    public class TipoUsuarioHandler : AuthorizationHandler<TipoUsuarioRequirement>
    {
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            TipoUsuarioRequirement requirement)
        {
            Console.WriteLine("=== TipoUsuarioHandler EXECUTOU ===");
            Console.WriteLine("IsAuthenticated: " + context.User.Identity?.IsAuthenticated);

            foreach (var claim in context.User.Claims)
            {
                Console.WriteLine($"CLAIM => {claim.Type} = {claim.Value}");
            }

            var claimTipo = context.User.FindFirst("user_type");

            if (claimTipo == null)
            {
                Console.WriteLine("❌ CLAIM user_type NÃO EXISTE");
                return Task.CompletedTask;
            }

            Console.WriteLine("user_type recebido: " + claimTipo.Value);

            if (!Enum.TryParse<UserTypeEnum>(claimTipo.Value, out var tipoUsuario))
            {
                Console.WriteLine("❌ NÃO conseguiu converter para enum");
                return Task.CompletedTask;
            }

            Console.WriteLine("Enum convertido: " + tipoUsuario);

            if (requirement.TiposPermitidos.Contains(tipoUsuario))
            {
                Console.WriteLine("✅ POLICY SATISFEITA");
                context.Succeed(requirement);
            }
            else
            {
                Console.WriteLine("❌ POLICY NÃO SATISFEITA");
            }

            return Task.CompletedTask;
        }
    }
}
