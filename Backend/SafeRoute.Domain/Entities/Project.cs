using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SafeRoute.Domain.Entities
{
    public class Project : BaseEntity
    {
        public string Name { get; set; } = default!;
        public string? ExternalId { get; set; }

        public int CompanyId { get; set; }
        public Company Company { get; set; } = default!;

        public ICollection<RuleViolation> RuleViolations { get; set; } = new List<RuleViolation>();
    }
}
