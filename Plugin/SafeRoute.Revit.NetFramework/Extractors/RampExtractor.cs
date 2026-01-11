using Autodesk.Revit.DB;
using SafeRoute.Revit.NetFramework.Models;
using SafeRoute.Revit.NetFramework.Utils;
using System.Collections.Generic;
using System.Xml.Linq;

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
                if (element is not Ramp ramp)
                    continue;

                var widthParam = ramp.get_Parameter(BuiltInParameter.RAMP_WIDTH);
                var lengthParam = ramp.get_Parameter(BuiltInParameter.RAMP_LENGTH);

                if (widthParam == null || lengthParam == null)
                    continue;

                var widthFeet = widthParam.AsDouble();
                var lengthFeet = lengthParam.AsDouble();

                // Slope = rise / run (decimal)
                var slope = ramp.Slope;

                var rampData = new RampData
                {
                    ElementId = ramp.Id.IntegerValue.ToString(),
                    Width = UnitConverter.FeetToMeters(widthFeet),
                    Length = UnitConverter.FeetToMeters(lengthFeet),
                    Slope = slope
                };

                result.Add(rampData);
            }

            return result;
        }
    }
}
