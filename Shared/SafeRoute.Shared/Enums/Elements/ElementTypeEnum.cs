using System.ComponentModel.DataAnnotations;

namespace SafeRoute.Shared.Enums.Elements
{
    public enum ElementTypeEnum
    {
        [Display(Name = "Undefined")]
        Undefined = 0,

        [Display(Name = "Door")]
        Door = 1,

        [Display(Name = "Ramp")]
        Ramp = 2,

        // Fase 2
        [Display(Name = "Stair")]
        Stair = 10,

        [Display(Name = "Evacuation Route")]
        EvacuationRoute = 20
    }
}
