using SafeRoute.Shared.Dtos.Company;
using SafeRoute.Shared.Dtos.User;

namespace SafeRoute.BlazorServer.ViewModels.Interfaces
{
    public interface ICompanyViewModel
    {
        bool IsBusy { get; }

        int PageNumber { get; set; }
        int PageSize { get; set; }
        int TotalCount { get; }

        string SelectedColumn { get; set; }
        string SearchTerm { get; set; }

        IEnumerable<ReadCompanyDto> Companies { get; }
        ReadCompanyDto Selected { get; set; }

        CreateCompanyDto New { get; set; }
        UpdateCompanyDto Edit { get; set; }

        Task LoadPagedAsync();
        Task LoadByIdAsync(int id);

        Task<bool> CreateAsync();
        Task<bool> SaveEditAsync();
        Task<bool> DeleteAsync(int id);

        void SetPage(int pageNumber);
    }
}
