using SafeRoute.Shared.Dtos.Ingestion.Doors;
using SafeRoute.Shared.Dtos.Ingestion.Ramps;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SafeRoute.Shared.Dtos.Ingestion.Project
{
    public class IngestProjectElementsRequestDto
    {
        public int? ProjectId { get; set; }
        public string? ProjectExternalId { get; set; }
        public string? ProjectName { get; set; }

        [Required(ErrorMessage = "Doors é obrigatório.")]
        public List<DoorIngestionDto> Doors { get; set; } = new List<DoorIngestionDto>();

        [Required(ErrorMessage = "Ramps é obrigatório.")]
        public List<RampIngestionDto> Ramps { get; set; } = new List<RampIngestionDto>();
    }
}
