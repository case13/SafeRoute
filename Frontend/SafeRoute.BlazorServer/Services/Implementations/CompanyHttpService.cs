using SafeRoute.BlazorServer.Services.Interfaces;
using SafeRoute.Shared.Dtos.Common;
using SafeRoute.Shared.Dtos.Company;

namespace SafeRoute.BlazorServer.Services.Implementations
{
    public class CompanyHttpService : BaseHttpService, ICompanyHttpService
    {
        public CompanyHttpService(IHttpClientFactory httpClientFactory, IAuthHttpService authHttpService) : base(httpClientFactory, authHttpService)
        {
        }

        public Task<IEnumerable<ReadCompanyDto>> GetAllAsync()
        {
            var url = "api/company";
            return GetAllAsync<ReadCompanyDto>(url);
        }

        public Task<PagedResultDto<ReadCompanyDto>> GetPagedAsync(int pageNumber, int pageSize, string filterColumn, string filterText)
        {
            var col = string.IsNullOrWhiteSpace(filterColumn)
                ? ""
                : Uri.EscapeDataString(filterColumn);
            var txt = string.IsNullOrWhiteSpace(filterText)
                ? ""
                : Uri.EscapeDataString(filterText);
            var url = "api/company/paged"
                + "?pageNumber=" + pageNumber
                + "&pageSize=" + pageSize
                + "&filterColumn=" + col
                + "&filterText=" + txt;
            return GetAsync<PagedResultDto<ReadCompanyDto>>(url);
        }
        public Task<ReadCompanyDto> GetByIdAsync(int id)
        {
            var url = "api/company/" + id;
            return GetAsync<ReadCompanyDto>(url);
        }
        public Task<ReadCompanyDto> CreateAsync(CreateCompanyDto dto)
        {
            var url = "api/company";
            return PostAsync<CreateCompanyDto, ReadCompanyDto>(url, dto);
        }
        public Task<ReadCompanyDto> UpdateAsync(int id, UpdateCompanyDto dto)
        {
            var url = "api/company/" + id;
            return PutAsync<UpdateCompanyDto, ReadCompanyDto>(url, dto);
        }
        public Task<bool> DeleteAsync(int id)
        {
            var url = "api/company/" + id;
            return DeleteAsync(url);
        }
    }
}
