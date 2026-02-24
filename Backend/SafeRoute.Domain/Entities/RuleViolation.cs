using SafeRoute.Shared.Enums.Elements;
using SafeRoute.Shared.Enums.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SafeRoute.Domain.Entities
{
    public class RuleViolation : BaseEntity
    {
        public string ElementExternalId { get; set; } = default!;
        public ElementTypeEnum ElementType { get; set; }
        public string RuleCode { get; set; } = default!;
        public string Message { get; set; } = default!;
        public RuleSeverityEnum Severity { get; set; }

        public int ProjectId { get; set; }
        public Project Project { get; set; } = default!;
    }
}
