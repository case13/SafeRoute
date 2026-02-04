using SafeRoute.Application.Dtos.Rules;
using SafeRoute.Shared.Dtos.Ingestion.Doors;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SafeRoute.Application.Services.Interfaces
{
    public interface IDoorRulesService
    {
        Task<List<GeneratedRuleViolationDto>> EvaluateAsync(List<DoorIngestionDto> doors);
    }
}
