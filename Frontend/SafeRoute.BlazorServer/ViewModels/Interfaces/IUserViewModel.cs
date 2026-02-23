using SafeRoute.BlazorServer.Components.Selects;
using SafeRoute.Shared.Dtos.User;

namespace SafeRoute.BlazorServer.ViewModels.Interfaces
{
    public interface IUserViewModel
    {
        bool IsBusy { get; }
        int PageNumber { get; set; }
        int PageSize { get; set; }
        int TotalCount { get; }
        string SelectedColumn { get; set; }
        string SearchTerm { get; set; }
        IEnumerable<ReadUserDto> Users { get; }
        ReadUserDto Selected { get; set; }
        CreateUserDto New { get; set; }
        UpdateUserDto Edit { get; set; }

        IEnumerable<SelectBoxOption<int?>> CompanyOptions { get; }
        Task LoadCompanyOptionsAsync();

        void PrepareNew();
        void PrepareEdit();

        Task LoadPagedAsync();
        Task LoadByIdAsync(int id);
        Task<bool> CreateAsync();
        Task<bool> SaveEditAsync();
        Task<bool> DeleteAsync(int id);
        void SetPage(int pageNumber);
    }
}
