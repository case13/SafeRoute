using SafeRoute.Application.Dtos.Rules;
using SafeRoute.Shared.Dtos.Ingestion.Ramps;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SafeRoute.Application.Services.Interfaces
{
    public interface IRampRulesService
    {
        Task<List<GeneratedRuleViolationDto>> EvaluateAsync(List<RampIngestionDto> ramps);
    }
}
