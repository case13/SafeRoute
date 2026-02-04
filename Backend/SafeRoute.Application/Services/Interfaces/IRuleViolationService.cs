using SafeRoute.Shared.Dtos.Common;
using SafeRoute.Shared.Dtos.Rules;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SafeRoute.Application.Services.Interfaces
{
    public interface IRuleViolationService : IBaseService<ReadRuleViolationDto, CreateRuleViolationDto, UpdateRuleViolationDto>
    {
        Task<PagedResultDto<ReadRuleViolationDto>> GetPagedAsync(
            int projectId,
            int pageNumber,
            int pageSize,
            string? filterColumn,
            string? filterText);

        Task<int> CreateBatchAsync(List<CreateRuleViolationDto> dtos);
    }
}
