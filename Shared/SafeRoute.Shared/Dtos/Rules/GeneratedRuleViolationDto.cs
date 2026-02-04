using SafeRoute.Shared.Enums.Rules;

namespace SafeRoute.Application.Dtos.Rules
{
    public class GeneratedRuleViolationDto
    {
        public string ElementExternalId { get; set; } = string.Empty;
        public string ElementType { get; set; } = string.Empty;
        public string RuleCode { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public RuleSeverityEnum Severity { get; set; }
    }
}
