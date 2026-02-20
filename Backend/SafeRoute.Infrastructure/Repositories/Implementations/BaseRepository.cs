using Microsoft.EntityFrameworkCore;
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
    public class BaseRepository<TEntity> : IBaseRepository<TEntity>
        where TEntity : BaseEntity
    {
        protected readonly SafeRouteDbContext _context;
        protected readonly DbSet<TEntity> _dbSet;

        public BaseRepository(SafeRouteDbContext context)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
        }

        public virtual IQueryable<TEntity> Query(bool asNoTracking = true)
        {
            var query = _dbSet.AsQueryable();

            if (asNoTracking)
                query = query.AsNoTracking();

            return query;
        }

        public virtual async Task<TEntity?> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public virtual async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public virtual async Task<(IEnumerable<TEntity> Items, int TotalCount)> GetPagedAsync(
            int pageNumber,
            int pageSize)
        {
            var totalCount = await _dbSet.CountAsync();

            var items = await _dbSet
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, totalCount);
        }

        public virtual async Task AddAsync(TEntity entity)
        {
            entity.CreatedAt = DateTime.UtcNow;
            entity.UpdatedAt = null;
            entity.IsActive = true;
            await _dbSet.AddAsync(entity);
        }

        public virtual async Task UpdateAsync(TEntity entity)
        {
            entity.UpdatedAt = DateTime.UtcNow;
            _dbSet.Update(entity);
        }

        public virtual async Task DeleteAsync(TEntity entity)
        {
            _dbSet.Remove(entity);
        }

        public virtual async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
