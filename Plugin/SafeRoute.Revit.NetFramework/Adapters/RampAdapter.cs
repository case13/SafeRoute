using SafeRoute.Revit.NetFramework.Models;
using SafeRoute.Shared.Dtos.Ingestion.Ramps;
using SafeRoute.Shared.Enums.Elements;
using SafeRoute.Shared.Enums.Normas;

namespace SafeRoute.Revit.NetFramework.Adapters
{
    public static class RampAdapter
    {
        public static RampIngestionDto ToIngestionDto(
            RampData data,
            NormaTypeEnum norma)
        {
            return new RampIngestionDto
            {
                ExternalId = data.ElementId,
                Width = data.Width,
                Length = data.Length,
                Slope = data.Slope,
                Norma = norma,
                ElementType = ElementTypeEnum.Ramp
            };
        }
    }
}
