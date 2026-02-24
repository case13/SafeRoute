using SafeRoute.Revit.NetFramework.Models;
using SafeRoute.Shared.Dtos.Ingestion.Doors;
using SafeRoute.Shared.Enums.Elements;
using SafeRoute.Shared.Enums.Normas;

namespace SafeRoute.Revit.NetFramework.Adapters
{
    public static class DoorAdapter
    {
        public static DoorIngestionDto ToIngestionDto(
            DoorData data,
            NormaTypeEnum norma)
        {
            return new DoorIngestionDto
            {
                ExternalId = data.ElementId,
                Width = data.Width,
                Height = data.Height,
                IsAccessible = data.IsAccessible,
                Norma = norma,
                ElementType = ElementTypeEnum.Door
            };
        }
    }
}
