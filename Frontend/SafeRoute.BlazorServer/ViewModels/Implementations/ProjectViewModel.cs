using SafeRoute.BlazorServer.Services.Interfaces;
using SafeRoute.BlazorServer.ViewModels.Interfaces;
using SafeRoute.Shared.Dtos.Project;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SafeRoute.BlazorServer.ViewModels.Implementations
{
    public class ProjectViewModel : IProjectViewModel
    {
        private readonly IProjectHttpService _service;

        public bool IsBusy { get; private set; }

        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public int TotalCount { get; private set; }

        public string SelectedColumn { get; set; } = "Name";
        public string SearchTerm { get; set; } = string.Empty;

        public IEnumerable<ReadProjectDto> Projects { get; private set; }
            = Enumerable.Empty<ReadProjectDto>();

        public ReadProjectDto Selected { get; set; } = new ReadProjectDto();

        public CreateProjectDto New { get; set; } = new CreateProjectDto();
        public UpdateProjectDto Edit { get; set; } = new UpdateProjectDto();

        public ProjectViewModel(IProjectHttpService service)
        {
            _service = service;
        }

        public void PrepareNew()
        {
            New = new CreateProjectDto();
        }

        public void PrepareEdit()
        {
            Selected = new ReadProjectDto();
            Edit = new UpdateProjectDto();
        }

        public async Task LoadPagedAsync()
        {
            IsBusy = true;
            try
            {
                var term = string.IsNullOrWhiteSpace(SearchTerm)
                    ? string.Empty
                    : SearchTerm;

                var page = await _service.GetPagedAsync(
                    PageNumber,
                    PageSize,
                    SelectedColumn,
                    term);

                Projects = page?.Items?.ToList()
                    ?? Enumerable.Empty<ReadProjectDto>();

                TotalCount = page?.TotalCount ?? 0;
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
                PrepareEdit();

                var dto = await _service.GetByIdAsync(id);
                Selected = dto;

                if (dto != null && dto.Id > 0)
                {
                    Edit = new UpdateProjectDto
                    {
                        Name = dto.Name,
                        ExternalId = dto.ExternalId,
                        IsActive = dto.IsActive
                    };
                }
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

                    if (!Projects.Any() && PageNumber > 1)
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
