using SafeRoute.Shared.Enums.Status;
using SafeRoute.Shared.Enums.Tipos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SafeRoute.Domain.Entities
{
    public class User : BaseEntity
    {
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PasswordHash { get; set; } = null;
        public string PasswordSalt { get; set; } = null!;
        public string Document { get; set; } = null!;

        public int CompanyId { get; set; } = 0;
        public Company Company { get; set; } = null!;
        public UserTypeEnum UserType { get; set; } = UserTypeEnum.UsuarioComum;
        public StatusBasicEnum UserStatus { get; set; } = StatusBasicEnum.Ativo;
    }
}
