using System.Collections.Generic;
using Autodesk.Revit.DB;
using SafeRoute.Revit.NetFramework.Models;
using SafeRoute.Revit.NetFramework.Utils;

namespace SafeRoute.Revit.NetFramework.Extractors
{
    public class RampExtractor
    {
        private readonly Document _document;

        public RampExtractor(Document document)
        {
            _document = document;
        }

        public IList<RampData> Extract()
        {
            var result = new List<RampData>();

            var collector = new FilteredElementCollector(_document)
                .OfCategory(BuiltInCategory.OST_Ramps)
                .WhereElementIsNotElementType();

            foreach (var element in collector)
            {
                var ramp = element as FamilyInstance;
                if (ramp == null)
                    continue;

                var widthParam = ramp.LookupParameter("Width");
                var lengthParam = ramp.LookupParameter("Length");

                if (widthParam == null || lengthParam == null)
                    continue;

                var rampData = new RampData
                {
                    ElementId = ramp.Id.IntegerValue.ToString(),
                    Width = UnitConverter.FeetToMeters(widthParam.AsDouble()),
                    Length = UnitConverter.FeetToMeters(lengthParam.AsDouble()),
                    Slope = 0 // Fase 1: slope será tratado depois
                };

                result.Add(rampData);
            }

            return result;
        }
    }
}
