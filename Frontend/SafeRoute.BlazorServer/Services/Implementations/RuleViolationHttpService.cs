using SafeRoute.BlazorServer.Services.Interfaces;
using SafeRoute.Shared.Dtos.Common;
using SafeRoute.Shared.Dtos.Rules;
using System;

namespace SafeRoute.BlazorServer.Services.Implementations
{
    public class RuleViolationHttpService : BaseHttpService, IRuleViolationHttpService
    {
        public RuleViolationHttpService(
            IHttpClientFactory httpClientFactory,
            IAuthHttpService authHttpService)
            : base(httpClientFactory, authHttpService)
        {
        }

        public Task<PagedResultDto<ReadRuleViolationDto>> GetPagedAsync(
            int projectId,
            int pageNumber,
            int pageSize,
            string filterColumn,
            string filterText)
        {
            var col = string.IsNullOrWhiteSpace(filterColumn)
                ? ""
                : Uri.EscapeDataString(filterColumn);

            var txt = string.IsNullOrWhiteSpace(filterText)
                ? ""
                : Uri.EscapeDataString(filterText);

            var url = "api/ruleviolations/paged"
                + "?projectId=" + projectId
                + "&pageNumber=" + pageNumber
                + "&pageSize=" + pageSize
                + "&filterColumn=" + col
                + "&filterText=" + txt;

            return GetAsync<PagedResultDto<ReadRuleViolationDto>>(url);
        }

        public Task<ReadRuleViolationDto> GetByIdAsync(int id)
        {
            var url = "api/ruleviolations/" + id;
            return GetAsync<ReadRuleViolationDto>(url);
        }

        public Task<ReadRuleViolationDto> CreateAsync(CreateRuleViolationDto dto)
        {
            var url = "api/ruleviolations";
            return PostAsync<CreateRuleViolationDto, ReadRuleViolationDto>(url, dto);
        }

        public Task<ReadRuleViolationDto> UpdateAsync(int id, UpdateRuleViolationDto dto)
        {
            var url = "api/ruleviolations/" + id;
            return PutAsync<UpdateRuleViolationDto, ReadRuleViolationDto>(url, dto);
        }

        public Task<bool> DeleteAsync(int id)
        {
            var url = "api/ruleviolations/" + id;
            return DeleteAsync(url);
        }
    }
}
