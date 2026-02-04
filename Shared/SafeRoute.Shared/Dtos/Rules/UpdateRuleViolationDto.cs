using SafeRoute.Shared.Enums.Rules;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SafeRoute.Shared.Dtos.Rules
{
    public class UpdateRuleViolationDto
    {
        [Required(ErrorMessage = "A mensagem da violação é obrigatória.")]
        public string Message { get; set; } = default!;

        [Required(ErrorMessage = "A severidade da regra é obrigatória.")]
        public RuleSeverityEnum Severity { get; set; }

        [Required(ErrorMessage = "O status da violação é obrigatório.")]
        public bool IsActive { get; set; }
    }
}
