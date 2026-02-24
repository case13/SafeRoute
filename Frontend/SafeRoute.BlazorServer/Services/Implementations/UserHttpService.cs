using SafeRoute.BlazorServer.Services.Implementations;
using SafeRoute.BlazorServer.Services.Interfaces;
using SafeRoute.Shared.Dtos.Common;
using SafeRoute.Shared.Dtos.User;

namespace SafeRoute.BlazorServer.Services.Implmentations
{
    public class UserHttpService : BaseHttpService, IUserHttpService
    {
        public UserHttpService(IHttpClientFactory httpClientFactory,
            IAuthHttpService authHttpService) : base(httpClientFactory, authHttpService)
        {
        }

        public Task<PagedResultDto<ReadUserDto>> GetPagedAsync(int pageNumber, int pageSize, string filterColumn, string filterText)
        {
            var col = string.IsNullOrWhiteSpace(filterColumn)
                ? ""
                : Uri.EscapeDataString(filterColumn);
            var txt = string.IsNullOrWhiteSpace(filterText)
                ? ""
                : Uri.EscapeDataString(filterText);
            var url = "api/user/paged"
                + "?pageNumber=" + pageNumber
                + "&pageSize=" + pageSize
                + "&filterColumn=" + col
                + "&filterText=" + txt;
            return GetAsync<PagedResultDto<ReadUserDto>>(url);
        }


        public Task<ReadUserDto> GetByIdAsync(int id)
        {
            var url = "api/user/" + id;
            return GetAsync<ReadUserDto>(url);
        }

        public Task<ReadUserDto> CreateAsync(CreateUserDto dto)
        {
            var url = "api/user";
            return PostAsync<CreateUserDto, ReadUserDto>(url, dto);
        }

        public Task<ReadUserDto> UpdateAsync(int id, UpdateUserDto dto)
        {
            var url = "api/user/" + id;
            return PutAsync<UpdateUserDto, ReadUserDto>(url, dto);
        }

        public Task<bool> DeleteAsync(int id)
        {
            var url = "api/users/" + id;
            return DeleteAsync(url);
        }

    }
}
