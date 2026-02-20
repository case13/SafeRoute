using SafeRoute.Shared.Enums.Elements;
using SafeRoute.Shared.Enums.Rules;

namespace SafeRoute.Shared.Dtos.Rules
{
    public class GeneratedRuleViolationDto
    {
        public string ElementExternalId { get; set; } = string.Empty;
        public ElementTypeEnum ElementType { get; set; } 
        public string RuleCode { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public RuleSeverityEnum Severity { get; set; }
    }
}
