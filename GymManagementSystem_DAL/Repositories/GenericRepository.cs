using GymManagementSystem_DAL.Data;
using GymManagementSystem_DAL.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace GymManagementSystem_DAL.Repositories
{
    public class GenericRepository<TEntity>(GymDbContext dbContext) : IGenericRepository<TEntity> where TEntity : BaseEntity
    {
        protected readonly GymDbContext _dbContext = dbContext;
        protected readonly DbSet<TEntity> _dbSet = dbContext.Set<TEntity>();

        public async Task<IReadOnlyList<TEntity>> GetAllAsync(CancellationToken ct = default)
            => await _dbSet.AsNoTracking().ToListAsync(ct);

        public async Task<IReadOnlyList<TEntity>> GetAllWhereAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken ct = default)
            => await _dbSet.Where(predicate).ToListAsync(ct);

        public async Task<TEntity?> GetByIdAsync(int id, CancellationToken ct = default, params Expression<Func<TEntity, object?>>[] includes)
            => await ApplyIncludes(_dbSet.AsQueryable(), includes)
                .FirstOrDefaultAsync(e => e.Id == id, ct);

        public async Task<TEntity?> GetWhereAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken ct = default)
            => await _dbSet.FirstOrDefaultAsync(predicate, ct);

        public async Task<TEntity?> GetByIdIncludingDeletedAsync(int id, CancellationToken ct = default)
            => await _dbSet.IgnoreQueryFilters().FirstOrDefaultAsync(e => e.Id == id, ct);

        public async Task<bool> ExistsAsync(int id, CancellationToken ct = default)
            => await _dbSet.AnyAsync(e => e.Id == id, ct);

        public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken ct = default)
            => await _dbSet.AnyAsync(predicate, ct);

        public async Task<int> CountAsync(Expression<Func<TEntity, bool>>? predicate = null, CancellationToken ct = default)
            => predicate is null
                ? await _dbSet.CountAsync(ct)
                : await _dbSet.CountAsync(predicate, ct);

        public void Add(TEntity entity)
            => _dbSet.Add(entity);

        public void Update(TEntity entity)
            => _dbSet.Update(entity);

        public void Delete(TEntity entity)
            => _dbSet.Remove(entity);

        protected static IQueryable<TEntity> ApplyIncludes(IQueryable<TEntity> query, Expression<Func<TEntity, object?>>[] includes)
        {
            foreach (var include in includes)
                query = query.Include(include);

            return query;
        }
    }
}
