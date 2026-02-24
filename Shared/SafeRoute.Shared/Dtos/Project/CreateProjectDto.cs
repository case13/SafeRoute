using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SafeRoute.Shared.Dtos.Project
{
    public class CreateProjectDto
    {
        [Required(ErrorMessage = "O nome do projeto é obrigatório.")]
        public string Name { get; set; } = default!;
        public string? ExternalId { get; set; } = default!;
    }
}
