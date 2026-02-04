using SafeRoute.Shared.Enums.Rules;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SafeRoute.Shared.Dtos.Rules
{
    public class ReadRuleViolationDto
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }

        public string ElementExternalId { get; set; } = default!;
        public string ElementType { get; set; } = default!;
        public string RuleCode { get; set; } = default!;
        public string Message { get; set; } = default!;
        public RuleSeverityEnum Severity { get; set; }

        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
