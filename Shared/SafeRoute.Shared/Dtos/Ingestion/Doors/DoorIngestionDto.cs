using SafeRoute.Shared.Enums.Elements;
using SafeRoute.Shared.Enums.Normas;

namespace SafeRoute.Shared.Dtos.Ingestion.Doors
{
    public class DoorIngestionDto
    {
        /// <summary>
        /// External identifier from the source system (e.g. Revit ElementId)
        /// </summary>
        public string ExternalId { get; set; } = default!;

        /// <summary>
        /// Door width in meters
        /// </summary>
        public double Width { get; set; }

        /// <summary>
        /// Door height in meters
        /// </summary>
        public double Height { get; set; }

        /// <summary>
        /// Indicates whether the door is marked as accessible
        /// </summary>
        public bool IsAccessible { get; set; }

        /// <summary>
        /// Normative context used for validation
        /// </summary>
        public NormaTypeEnum Norma { get; set; }

        /// <summary>
        /// Element type (fixed for this DTO)
        /// </summary>
        public ElementTypeEnum ElementType { get; set; } = ElementTypeEnum.Door;
    }
}
