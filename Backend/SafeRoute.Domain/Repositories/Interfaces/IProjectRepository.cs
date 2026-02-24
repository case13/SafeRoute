using SafeRoute.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SafeRoute.Domain.Repositories.Interfaces
{
    public interface IProjectRepository : IBaseRepository<Project>
    {
        Task<Project?> GetByExternalIdAsync(string externalId);
        Task<bool> ExistsByExternalIdAsync(string externalId);

    }
}
