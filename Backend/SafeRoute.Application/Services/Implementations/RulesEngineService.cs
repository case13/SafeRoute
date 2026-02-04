using SafeRoute.Application.Dtos.Rules;
using SafeRoute.Application.Services.Interfaces;
using SafeRoute.Shared.Dtos.Ingestion.Project;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SafeRoute.Application.Services.Implementations
{
    public class RulesEngineService : IRulesEngineService
    {
        private readonly IDoorRulesService _doorRulesService;
        private readonly IRampRulesService _rampRulesService;

        public RulesEngineService(
            IDoorRulesService doorRulesService,
            IRampRulesService rampRulesService)
        {
            _doorRulesService = doorRulesService;
            _rampRulesService = rampRulesService;
        }

        public async Task<List<GeneratedRuleViolationDto>> EvaluateAsync(IngestProjectElementsRequestDto dto)
        {
            var result = new List<GeneratedRuleViolationDto>();

            if (dto == null)
                return result;

            var doorViolations = await _doorRulesService.EvaluateAsync(dto.Doors);
            if (doorViolations != null && doorViolations.Count > 0)
                result.AddRange(doorViolations);

            var rampViolations = await _rampRulesService.EvaluateAsync(dto.Ramps);
            if (rampViolations != null && rampViolations.Count > 0)
                result.AddRange(rampViolations);

            return result;
        }
    }
}
