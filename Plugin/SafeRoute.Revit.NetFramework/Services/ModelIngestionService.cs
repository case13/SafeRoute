using System.Collections.Generic;
using Autodesk.Revit.DB;
using SafeRoute.Revit.NetFramework.Adapters;
using SafeRoute.Revit.NetFramework.Extractors;
using SafeRoute.Shared.Dtos.Ingestion.Doors;
using SafeRoute.Shared.Dtos.Ingestion.Ramps;
using SafeRoute.Shared.Enums.Normas;

namespace SafeRoute.Revit.NetFramework.Services
{
    public class ModelIngestionService
    {
        private readonly Document _document;

        public ModelIngestionService(Document document)
        {
            _document = document;
        }

        public IList<DoorIngestionDto> GetDoors(NormaTypeEnum norma)
        {
            var extractor = new DoorExtractor(_document);
            var doorModels = extractor.Extract();

            var result = new List<DoorIngestionDto>();
            foreach (var door in doorModels)
            {
                result.Add(DoorAdapter.ToIngestionDto(door, norma));
            }

            return result;
        }

        public IList<RampIngestionDto> GetRamps(NormaTypeEnum norma)
        {
            var extractor = new RampExtractor(_document);
            var rampModels = extractor.Extract();

            var result = new List<RampIngestionDto>();
            foreach (var ramp in rampModels)
            {
                result.Add(RampAdapter.ToIngestionDto(ramp, norma));
            }

            return result;
        }
    }
}
