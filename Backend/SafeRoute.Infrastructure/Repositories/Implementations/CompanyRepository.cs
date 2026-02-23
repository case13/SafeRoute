using Microsoft.EntityFrameworkCore;
using SafeRoute.Domain.Entities;
using SafeRoute.Domain.Repositories.Interfaces;
using SafeRoute.Infrastructure.Data;

namespace SafeRoute.Infrastructure.Repositories.Implementations
{
    public class CompanyRepository : BaseRepository<Company>, ICompanyRepository
    {
        private readonly SafeRouteDbContext _context;
        public CompanyRepository(SafeRouteDbContext context) : base(context)
        {
            _context = context;
        }

        public Task<Company?> GetByRegistryAsync(string registry)
        {
            return _dbSet
                .Where(c => c.Registry == registry)
                .FirstOrDefaultAsync();
        }

    }
}
