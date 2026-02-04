using SafeRoute.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SafeRoute.Domain.Repositories.Interfaces
{
    public interface IRuleViolationRepository : IBaseRepository<RuleViolation>
    {
        Task AddRangeAsync(IEnumerable<RuleViolation> entities);
        IQueryable<RuleViolation> QueryByProject(int projectId, bool asNoTracking = true);
        Task<int> DeleteByProjectIdAsync(int projectId);
    }
}
