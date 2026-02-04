using SafeRoute.Application.Dtos.Rules;
using SafeRoute.Application.Services.Interfaces;
using SafeRoute.Shared.Dtos.Ingestion.Doors;
using SafeRoute.Shared.Enums.Rules;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SafeRoute.Application.Services.Implementations
{
    public class DoorRulesService : IDoorRulesService
    {
        public Task<List<GeneratedRuleViolationDto>> EvaluateAsync(List<DoorIngestionDto> doors)
        {
            var violations = new List<GeneratedRuleViolationDto>();

            if (doors == null || doors.Count == 0)
                return Task.FromResult(violations);

            foreach (var door in doors)
            {
                if (door == null)
                    continue;

                if (door.Width < 0.80)
                {
                    violations.Add(new GeneratedRuleViolationDto
                    {
                        ElementExternalId = (door.ExternalId ?? string.Empty).Trim(),
                        ElementType = door.ElementType.ToString(),
                        RuleCode = "DOOR_MIN_WIDTH",
                        Message = "Largura mínima da porta não atendida.",
                        Severity = RuleSeverityEnum.Warning
                    });
                }
            }

            return Task.FromResult(violations);
        }
    }
}
