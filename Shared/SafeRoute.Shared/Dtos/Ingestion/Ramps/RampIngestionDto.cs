using SafeRoute.Shared.Enums.Elements;
using SafeRoute.Shared.Enums.Normas;

namespace SafeRoute.Shared.Dtos.Ingestion.Ramps
{
    public class RampIngestionDto
    {
        /// <summary>
        /// External identifier from the source system (e.g. Revit ElementId)
        /// </summary>
        public string ExternalId { get; set; } = default!;

        /// <summary>
        /// Ramp width in meters
        /// </summary>
        public double Width { get; set; }

        /// <summary>
        /// Ramp length in meters
        /// </summary>
        public double Length { get; set; }

        /// <summary>
        /// Ramp slope as decimal (e.g. 0.0833 = 8.33%)
        /// </summary>
        public double Slope { get; set; }

        /// <summary>
        /// Normative context used for validation
        /// </summary>
        public NormaTypeEnum Norma { get; set; }

        /// <summary>
        /// Element type (fixed for this DTO)
        /// </summary>
        public ElementTypeEnum ElementType { get; set; } = ElementTypeEnum.Ramp;
    }
}
