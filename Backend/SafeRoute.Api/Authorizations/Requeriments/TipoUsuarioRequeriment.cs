using Microsoft.AspNetCore.Authorization;
using SafeRoute.Shared.Enums.Tipos;

namespace SafeRoute.Api.Authorizations.Requeriments
{
    public class TipoUsuarioRequirement : IAuthorizationRequirement
    {
        public IReadOnlyCollection<UserTypeEnum> TiposPermitidos { get; }

        public TipoUsuarioRequirement(params UserTypeEnum[] tiposPermitidos)
        {
            TiposPermitidos = tiposPermitidos;
        }
    }
}
