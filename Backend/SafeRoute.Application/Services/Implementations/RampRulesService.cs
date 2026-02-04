using SafeRoute.Application.Dtos.Rules;
using SafeRoute.Application.Services.Interfaces;
using SafeRoute.Shared.Dtos.Ingestion.Ramps;
using SafeRoute.Shared.Enums.Rules;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SafeRoute.Application.Services.Implementations
{
    public class RampRulesService : IRampRulesService
    {
        public Task<List<GeneratedRuleViolationDto>> EvaluateAsync(List<RampIngestionDto> ramps)
        {
            var violations = new List<GeneratedRuleViolationDto>();

            if (ramps == null || ramps.Count == 0)
                return Task.FromResult(violations);

            foreach (var ramp in ramps)
            {
                if (ramp == null)
                    continue;

                if (ramp.Slope > 0.0833)
                {
                    violations.Add(new GeneratedRuleViolationDto
                    {
                        ElementExternalId = (ramp.ExternalId ?? string.Empty).Trim(),
                        ElementType = ramp.ElementType.ToString(),
                        RuleCode = "RAMP_MAX_SLOPE",
                        Message = "Inclinação máxima da rampa excedida.",
                        Severity = RuleSeverityEnum.Serious
                    });
                }
            }

            return Task.FromResult(violations);
        }
    }
}
