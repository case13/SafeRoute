using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SafeRoute.Shared.Enums.Tipos
{
    public enum UserTypeEnum
    {
        [Display(Name = "Administrador")]
        Administrador = 1,
        [Display(Name = "Usuário Comum")]
        UsuarioComum = 2,
        [Display(Name = "Convidado")]
        Convidado = 3
    }
}
