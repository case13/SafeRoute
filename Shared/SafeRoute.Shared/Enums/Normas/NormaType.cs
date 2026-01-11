using System.ComponentModel.DataAnnotations;

namespace SafeRoute.Shared.Enums.Normas
{
    public enum NormaType
    {
        [Display(Name = "Undefined")]
        Undefined = 0,

        [Display(Name = "NBR 9050")]
        NBR_9050 = 1,

        [Display(Name = "NBR 9077")]
        NBR_9077 = 2
    }
}
