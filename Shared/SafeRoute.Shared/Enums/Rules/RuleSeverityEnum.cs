using System.ComponentModel.DataAnnotations;

namespace SafeRoute.Shared.Enums.Rules
{
    public enum RuleSeverityEnum
    {
        [Display(Name = "Information")]
        Info = 0,

        [Display(Name = "Warning")]
        Warning = 1,

        [Display(Name = "Serious")]
        Serious = 2,

        [Display(Name = "Critical")]
        Critical = 3
    }
}
