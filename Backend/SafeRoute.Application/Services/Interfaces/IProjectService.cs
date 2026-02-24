using SafeRoute.Shared.Dtos.Common;
using SafeRoute.Shared.Dtos.Project;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SafeRoute.Application.Services.Interfaces
{
    public interface IProjectService : IBaseService<ReadProjectDto, CreateProjectDto, UpdateProjectDto>
    {
        Task<PagedResultDto<ReadProjectDto>> GetPagedAsync(
            int pageNumber,
            int pageSize,
            string? filterColumn,
            string? filterText);
    }
}
