using SafeRoute.Shared.Dtos.Project;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SafeRoute.BlazorServer.ViewModels.Interfaces
{
    public interface IProjectViewModel
    {
        bool IsBusy { get; }

        int PageNumber { get; set; }
        int PageSize { get; set; }
        int TotalCount { get; }

        string SelectedColumn { get; set; }
        string SearchTerm { get; set; }

        IEnumerable<ReadProjectDto> Projects { get; }
        ReadProjectDto Selected { get; set; }

        CreateProjectDto New { get; set; }
        UpdateProjectDto Edit { get; set; }

        Task LoadPagedAsync();
        Task LoadByIdAsync(int id);

        Task<bool> CreateAsync();
        Task<bool> SaveEditAsync();
        Task<bool> DeleteAsync(int id);

        void SetPage(int pageNumber);
    }
}
