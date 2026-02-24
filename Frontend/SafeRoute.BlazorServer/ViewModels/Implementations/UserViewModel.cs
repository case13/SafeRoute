using SafeRoute.BlazorServer.Components.Selects;
using SafeRoute.BlazorServer.Services.Interfaces;
using SafeRoute.BlazorServer.ViewModels.Interfaces;
using SafeRoute.Shared.Dtos.Company;
using SafeRoute.Shared.Dtos.User;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SafeRoute.BlazorServer.ViewModels.Implementations
{
    public class UserViewModel : IUserViewModel
    {
        private readonly IUserHttpService _service;
        private readonly ICompanyHttpService _companyService;

        public bool IsBusy { get; private set; }

        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public int TotalCount { get; private set; }

        public string SelectedColumn { get; set; } = "Name";
        public string SearchTerm { get; set; } = string.Empty;

        public IEnumerable<ReadUserDto> Users { get; private set; }
            = Enumerable.Empty<ReadUserDto>();

        public ReadUserDto Selected { get; set; } = new ReadUserDto();

        public CreateUserDto New { get; set; } = new CreateUserDto();
        public UpdateUserDto Edit { get; set; } = new UpdateUserDto();

        public IEnumerable<SelectBoxOption<int?>> CompanyOptions { get; private set; }
            = Enumerable.Empty<SelectBoxOption<int?>>();

        public UserViewModel(
            IUserHttpService service,
            ICompanyHttpService companyService)
        {
            _service = service;
            _companyService = companyService;
        }

        public void PrepareNew()
        {
            New = new CreateUserDto
            {
                CompanyId = null
            };
        }

        public void PrepareEdit()
        {
            Selected = new ReadUserDto();
            Edit = new UpdateUserDto();
        }

        public async Task LoadCompanyOptionsAsync()
        {
            var companies = await _companyService.GetAllAsync();

            CompanyOptions = (companies ?? Enumerable.Empty<ReadCompanyDto>())
                .Select(x => new SelectBoxOption<int?>(x.Id, x.Name))
                .ToList();
        }

        public async Task LoadPagedAsync()
        {
            IsBusy = true;
            try
            {
                var term = string.IsNullOrWhiteSpace(SearchTerm) ? string.Empty : SearchTerm;

                var page = await _service.GetPagedAsync(PageNumber, PageSize, SelectedColumn, term);

                Users = page.Items;
                TotalCount = page.TotalCount;
            }
            finally
            {
                IsBusy = false;
            }
        }

        public async Task LoadByIdAsync(int id)
        {
            IsBusy = true;
            try
            {
                Selected = await _service.GetByIdAsync(id);
            }
            finally
            {
                IsBusy = false;
            }
        }

        public async Task<bool> CreateAsync()
        {
            IsBusy = true;
            try
            {
                var created = await _service.CreateAsync(New);
                var ok = created != null && created.Id > 0;

                if (ok)
                {
                    PrepareNew();
                    await LoadPagedAsync();
                }

                return ok;
            }
            finally
            {
                IsBusy = false;
            }
        }

        public async Task<bool> SaveEditAsync()
        {
            if (Selected == null || Selected.Id <= 0)
                return false;

            IsBusy = true;
            try
            {
                var updated = await _service.UpdateAsync(Selected.Id, Edit);
                var ok = updated != null && updated.Id > 0;

                if (ok)
                {
                    PrepareEdit();
                    await LoadPagedAsync();
                }

                return ok;
            }
            finally
            {
                IsBusy = false;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            IsBusy = true;
            try
            {
                var ok = await _service.DeleteAsync(id);

                if (ok)
                {
                    await LoadPagedAsync();

                    if (!Users.Any() && PageNumber > 1)
                    {
                        PageNumber--;
                        await LoadPagedAsync();
                    }
                }

                return ok;
            }
            finally
            {
                IsBusy = false;
            }
        }

        public void SetPage(int pageNumber)
        {
            PageNumber = pageNumber <= 0 ? 1 : pageNumber;
        }
    }
}