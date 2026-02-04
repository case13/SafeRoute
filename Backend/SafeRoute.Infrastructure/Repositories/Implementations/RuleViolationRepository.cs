using Microsoft.EntityFrameworkCore;
using SafeRoute.Domain.Currents.Interfaces;
using SafeRoute.Domain.Entities;
using SafeRoute.Domain.Repositories.Interfaces;
using SafeRoute.Infrastructure.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SafeRoute.Infrastructure.Repositories.Implementations
{
    public class RuleViolationRepository : BaseRepository<RuleViolation>, IRuleViolationRepository
    {
        private readonly ICurrentCompany _currentCompany;

        public RuleViolationRepository(
            SafeRouteDbContext context,
            ICurrentCompany currentCompany)
            : base(context)
        {
            _currentCompany = currentCompany;
        }

        public async Task<int> DeleteByProjectIdAsync(int projectId)
        {
            return await _dbSet
                .Where(x => x.ProjectId == projectId)
                .ExecuteDeleteAsync();
        }


        public IQueryable<RuleViolation> QueryByProject(int projectId, bool asNoTracking = true)
        {
            return base.Query(asNoTracking)
                .Where(x => x.ProjectId == projectId);
        }

        public override async Task<RuleViolation?> GetByIdAsync(int id)
        {
            var companyId = _currentCompany.CompanyId;

            return await _dbSet
                .AsNoTracking()
                .Include(x => x.Project)
                .FirstOrDefaultAsync(x =>
                    x.Id == id &&
                    x.Project.CompanyId == companyId);
        }

        public async Task AddRangeAsync(IEnumerable<RuleViolation> entities)
        {
            await _dbSet.AddRangeAsync(entities);
        }
    }
}
