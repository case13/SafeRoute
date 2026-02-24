using SafeRoute.Shared.Dtos.Common;
using SafeRoute.Shared.Dtos.Ingestion.Project;
using System.Threading.Tasks;

namespace SafeRoute.Application.Services.Interfaces
{
    public interface IProjectIngestionService
    {
        Task<ReplaceResultDto> ReplaceByProjectAsync(IngestProjectElementsRequestDto dto);
    }
}