using Microsoft.EntityFrameworkCore;
using SafeRoute.Domain.Currents.Interfaces;
using SafeRoute.Domain.Entities;
using SafeRoute.Domain.Repositories.Interfaces;
using SafeRoute.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SafeRoute.Infrastructure.Repositories.Implementations
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        private readonly ICurrentCompany _currentCompany;
        private readonly SafeRouteDbContext _context;

        public UserRepository(
            SafeRouteDbContext context,
            ICurrentCompany currentCompany)
            : base(context)
        {
            _context = context;
            _currentCompany = currentCompany;
        }

        public async Task<User?> GetByEmailForLoginAsync(string email)
        {
            return await _dbSet
                .FirstOrDefaultAsync(x => x.Email == email);
        }

        public override async Task<User?> GetByIdAsync(int id)
        {
            return await _dbSet
                .AsNoTracking()
                .Where(x => x.CompanyId == _currentCompany.CompanyId)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public override async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _dbSet
                .AsNoTracking()
                .Where(x => x.CompanyId == _currentCompany.CompanyId)
                .OrderBy(x => x.Name)
                .ToListAsync();
        }

        public override async Task<(IEnumerable<User> Items, int TotalCount)> GetPagedAsync(
            int pageNumber,
            int pageSize)
        {
            var query = _dbSet
                .AsNoTracking()
                .Where(x => x.CompanyId == _currentCompany.CompanyId);

            var totalCount = await query.CountAsync();

            var items = await query
                .OrderBy(x => x.Name)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, totalCount);
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _dbSet
                .AsNoTracking()
                .Where(x => x.CompanyId == _currentCompany.CompanyId)
                .FirstOrDefaultAsync(x => x.Email == email);
        }
    }
}
