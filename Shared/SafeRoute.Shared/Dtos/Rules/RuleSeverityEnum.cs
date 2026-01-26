using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SafeRoute.Shared.Dtos.Rules
{
    public enum RuleSeverityEnum
    {
        [Display(Name = "Informational")]
        Info = 1,
        [Display(Name = "Warning")]
        Warning = 2,
        [Display(Name = "Error")]
        Error = 3
    }
}
