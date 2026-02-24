using SafeRoute.BlazorServer.Services.Interfaces;
using SafeRoute.BlazorServer.ViewModels.Interfaces;
using SafeRoute.Shared.Dtos.Rules;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SafeRoute.BlazorServer.ViewModels.Implementations
{
    public class RuleViolationViewModel : IRuleViolationViewModel
    {
        private readonly IRuleViolationHttpService _service;

        public bool IsBusy { get; private set; }

        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public int TotalCount { get; private set; }

        public string SelectedColumn { get; set; } = "Id";
        public string SearchTerm { get; set; } = string.Empty;
        public int ProjectId { get; set; }

        public IEnumerable<ReadRuleViolationDto> Items { get; private set; }
            = Enumerable.Empty<ReadRuleViolationDto>();

        public ReadRuleViolationDto Selected { get; set; } = new ReadRuleViolationDto();

        public CreateRuleViolationDto New { get; set; } = new CreateRuleViolationDto();
        public UpdateRuleViolationDto Edit { get; set; } = new UpdateRuleViolationDto();

        public RuleViolationViewModel(IRuleViolationHttpService service)
        {
            _service = service;
        }

        public async Task LoadPagedAsync()
        {
            IsBusy = true;
            try
            {
                var term = string.IsNullOrWhiteSpace(SearchTerm) ? "" : SearchTerm;

                var page = await _service.GetPagedAsync(
                     ProjectId,
                     PageNumber,
                     PageSize,
                     SelectedColumn,
                     term
                     );

                Items = page?.Items?.ToList()
                    ?? Enumerable.Empty<ReadRuleViolationDto>();

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
                var dto = await _service.GetByIdAsync(id);
                Selected = dto;

                if (dto != null && dto.Id > 0)
                {
                    Edit = new UpdateRuleViolationDto
                    {
                        // Ajuste os campos conforme seu UpdateRuleViolationDto
                        // Exemplo:
                        // IsActive = dto.IsActive
                    };
                }
                else
                {
                    Edit = new UpdateRuleViolationDto();
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
                    New = new CreateRuleViolationDto();
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
                    Selected = new ReadRuleViolationDto();
                    Edit = new UpdateRuleViolationDto();
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

                    if (!Items.Any() && PageNumber > 1)
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
