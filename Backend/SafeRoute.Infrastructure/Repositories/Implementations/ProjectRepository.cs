using Microsoft.EntityFrameworkCore;
using SafeRoute.Domain.Currents.Interfaces;
using SafeRoute.Domain.Entities;
using SafeRoute.Domain.Repositories.Interfaces;
using SafeRoute.Infrastructure.Data;
using System.Linq;
using System.Threading.Tasks;

namespace SafeRoute.Infrastructure.Repositories.Implementations
{
    public class ProjectRepository : BaseRepository<Project>, IProjectRepository
    {
        private readonly ICurrentCompany _currentCompany;

        public ProjectRepository(
            SafeRouteDbContext context,
            ICurrentCompany currentCompany)
            : base(context)
        {
            _currentCompany = currentCompany;
        }

        // Query SEMPRE filtrada por Company
        public override IQueryable<Project> Query(bool asNoTracking = true)
        {
            var companyId = _currentCompany.CompanyId;

            var query = base.Query(asNoTracking)
                .Where(x => x.CompanyId == companyId);

            return query;
        }

        // Get por ID SEMPRE filtrado por Company
        public override async Task<Project?> GetByIdAsync(int id)
        {
            var companyId = _currentCompany.CompanyId;

            return await _dbSet
                .AsNoTracking()
                .FirstOrDefaultAsync(x =>
                    x.Id == id &&
                    x.CompanyId == companyId);
        }

        public async Task<Project?> GetByExternalIdAsync(string externalId)
        {
            if (string.IsNullOrWhiteSpace(externalId))
                return null;

            var companyId = _currentCompany.CompanyId;

            return await _dbSet
                .AsNoTracking()
                .FirstOrDefaultAsync(x =>
                    x.CompanyId == companyId &&
                    x.ExternalId == externalId);
        }

        public async Task<bool> ExistsByExternalIdAsync(string externalId)
        {
            if (string.IsNullOrWhiteSpace(externalId))
                return false;

            var companyId = _currentCompany.CompanyId;

            return await _dbSet
                .AsNoTracking()
                .AnyAsync(x =>
                    x.CompanyId == companyId &&
                    x.ExternalId == externalId);
        }
    }
}
