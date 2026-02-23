using SafeRoute.BlazorServer.Services.Interfaces;
using SafeRoute.Shared.Dtos.Common;
using SafeRoute.Shared.Dtos.Project;
using System;
using System.Threading.Tasks;

namespace SafeRoute.BlazorServer.Services.Implementations
{
    public class ProjectHttpService : BaseHttpService, IProjectHttpService
    {
        public ProjectHttpService(
            IHttpClientFactory httpClientFactory,
            IAuthHttpService authHttpService)
            : base(httpClientFactory, authHttpService)
        {
        }

        public Task<PagedResultDto<ReadProjectDto>> GetPagedAsync(
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

            var url = "api/projects/paged"
                + "?pageNumber=" + pageNumber
                + "&pageSize=" + pageSize
                + "&filterColumn=" + col
                + "&filterText=" + txt;

            return GetAsync<PagedResultDto<ReadProjectDto>>(url);
        }

        public Task<ReadProjectDto> GetByIdAsync(int id)
        {
            var url = "api/projects/" + id;
            return GetAsync<ReadProjectDto>(url);
        }

        public Task<ReadProjectDto> CreateAsync(CreateProjectDto dto)
        {
            var url = "api/projects";
            return PostAsync<CreateProjectDto, ReadProjectDto>(url, dto);
        }

        public Task<ReadProjectDto> UpdateAsync(int id, UpdateProjectDto dto)
        {
            var url = "api/projects/" + id;
            return PutAsync<UpdateProjectDto, ReadProjectDto>(url, dto);
        }

        public Task<bool> DeleteAsync(int id)
        {
            var url = "api/projects/" + id;
            return DeleteAsync(url);
        }
    }
}
