using SafeRoute.Shared.Enums.Rules;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SafeRoute.Shared.Dtos.Rules
{
    public class RuleViolationQueryDto
    {
        [Range(1, int.MaxValue, ErrorMessage = "A página deve ser maior ou igual a 1.")]
        public int Page { get; set; }
        [Range(1, 100, ErrorMessage = "O tamanho da página deve estar entre 1 e 100.")]
        public int PageSize { get; set; }

        public string? Search { get; set; }
        public string? ElementExternalId { get; set; }
        public string? RuleCode { get; set; }
        public RuleSeverityEnum? Severity { get; set; }
    }
}
