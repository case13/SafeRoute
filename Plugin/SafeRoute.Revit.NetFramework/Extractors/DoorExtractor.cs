using System.Collections.Generic;
using Autodesk.Revit.DB;
using SafeRoute.Revit.NetFramework.Models;
using SafeRoute.Revit.NetFramework.Utils;

namespace SafeRoute.Revit.NetFramework.Extractors
{
    public class DoorExtractor
    {
        private readonly Document _document;

        public DoorExtractor(Document document)
        {
            _document = document;
        }

        public IList<DoorData> Extract()
        {
            var result = new List<DoorData>();

            var collector = new FilteredElementCollector(_document)
                .OfCategory(BuiltInCategory.OST_Doors)
                .WhereElementIsNotElementType();

            foreach (var element in collector)
            {
                var door = element as FamilyInstance;
                if (door == null)
                    continue;

                var widthParam = door.get_Parameter(BuiltInParameter.DOOR_WIDTH);
                var heightParam = door.get_Parameter(BuiltInParameter.DOOR_HEIGHT);

                if (widthParam == null || heightParam == null)
                    continue;

                var doorData = new DoorData
                {
                    ElementId = door.Id.IntegerValue.ToString(),
                    Width = UnitConverter.FeetToMeters(widthParam.AsDouble()),
                    Height = UnitConverter.FeetToMeters(heightParam.AsDouble()),
                    IsAccessible = false // Fase 1
                };

                result.Add(doorData);
            }

            return result;
        }
    }
}
