using SafeRoute.Shared.Dtos.Rules;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SafeRoute.BlazorServer.ViewModels.Interfaces
{
    public interface IRuleViolationViewModel
    {
        bool IsBusy { get; }

        int PageNumber { get; set; }
        int PageSize { get; set; }
        int TotalCount { get; }

        string SelectedColumn { get; set; }
        string SearchTerm { get; set; }

        int ProjectId { get; set; }

        IEnumerable<ReadRuleViolationDto> Items { get; }
        ReadRuleViolationDto Selected { get; set; }

        CreateRuleViolationDto New { get; set; }
        UpdateRuleViolationDto Edit { get; set; }

        Task LoadPagedAsync();
        Task LoadByIdAsync(int id);

        Task<bool> CreateAsync();
        Task<bool> SaveEditAsync();
        Task<bool> DeleteAsync(int id);

        void SetPage(int pageNumber);
    }
}
