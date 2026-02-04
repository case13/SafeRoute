using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SafeRoute.Shared.Enums.Status
{
    public enum StatusBasicEnum
    {
        [Display(Name = "Ativo")]
        Ativo = 1,
        [Display(Name = "Inativo")]
        Inativo = 2
    }
}
