using System;
using System.Collections.Generic;
using System.Text;

namespace SafeRoute.Shared.Dtos.Project
{
    public class ReadProjectDto
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }

        public string Name { get; set; } = default!;
        public string? ExternalId { get; set; }

        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
