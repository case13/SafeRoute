using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SafeRoute.Shared.Enums.Tipos
{
    public enum DocumentTypeEnum
    {
        [Display(Name = "CPF")]
        CPF = 1,
        [Display(Name = "CNPJ")]
        CNPJ = 2
    }
}
