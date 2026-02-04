using SafeRoute.Shared.Enums.Rules;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SafeRoute.Shared.Dtos.Rules
{
    public class CreateRuleViolationDto
    {
        [Required(ErrorMessage = "O projeto é obrigatório.")]
        [Range(1, int.MaxValue, ErrorMessage = "O projeto é obrigatório.")]
        public int ProjectId { get; set; }

        [Required(ErrorMessage = "O identificador externo do elemento é obrigatório.")]
        public string ElementExternalId { get; set; } = default!;

        [Required(ErrorMessage = "O tipo do elemento é obrigatório.")]
        public string ElementType { get; set; } = default!;

        [Required(ErrorMessage = "O código da regra é obrigatório.")]
        public string RuleCode { get; set; } = default!;

        [Required(ErrorMessage = "A mensagem da violação é obrigatória.")]
        public string Message { get; set; } = default!;

        [Required(ErrorMessage = "A severidade da regra é obrigatória.")]
        public RuleSeverityEnum Severity { get; set; }
    }
}
