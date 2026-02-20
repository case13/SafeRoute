using SafeRoute.Shared.Dtos.Common;
using SafeRoute.Shared.Dtos.Rules;
using System;

namespace SafeRoute.BlazorServer.Services.Interfaces
{
    public interface IRuleViolationHttpService
    {
        Task<PagedResultDto<ReadRuleViolationDto>> GetPagedAsync(
            int projectId,
            int pageNumber,
            int pageSize,
            string filterColumn,
            string filterText);

        Task<ReadRuleViolationDto> GetByIdAsync(int id);

        Task<ReadRuleViolationDto> CreateAsync(CreateRuleViolationDto dto);

        Task<ReadRuleViolationDto> UpdateAsync(int id, UpdateRuleViolationDto dto);

        Task<bool> DeleteAsync(int id);
    }
}
