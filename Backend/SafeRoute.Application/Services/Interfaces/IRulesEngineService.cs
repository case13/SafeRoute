using SafeRoute.Application.Dtos.Rules;
using SafeRoute.Shared.Dtos.Ingestion.Project;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SafeRoute.Application.Services.Interfaces
{
    public interface IRulesEngineService
    {
        Task<List<GeneratedRuleViolationDto>> EvaluateAsync(IngestProjectElementsRequestDto dto);
    }
}
