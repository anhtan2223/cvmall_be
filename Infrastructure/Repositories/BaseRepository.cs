using Application.Common.Abstractions;
using Domain.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Infrastructure.Repositories
{
    public class BaseRepository<TEntity> : IRepository<TEntity> where TEntity : class, IAudit, ISoftDelete, IEntity<Guid>
    {
        protected DbContext DbContext { get; private set; }
        protected readonly DbSet<TEntity> _dbSet;

        public BaseRepository(DbContext dbContext)
        {
            DbContext = dbContext;
            _dbSet = DbContext.Set<TEntity>();
        }

        public async Task AddEntityAsync(TEntity entity)
        {
            entity.id = Guid.NewGuid();
            entity.created_at = DateTime.Now;
            entity.updated_at = DateTime.Now;
            await _dbSet.AddAsync(entity);
        }

        public async Task DeleteEntityAsync(TEntity entity)
        {
            entity.del_flg = true;
            await Task.CompletedTask;
        }

        public async Task<bool> ExistByIdAsync(Guid id)
        {
            var result = _dbSet.Any(x => x.id == id);
            return await Task.FromResult(result);
        }

        public async Task<TEntity> FindAsync(Guid id)
        {
            return await _dbSet.FindAsync(id);
        }

        public IQueryable<TEntity> GetQuery(Func<DbSet<TEntity>, IQueryable<TEntity>>? func = null)
        {
            return func != null ? func(_dbSet) : _dbSet;
        }

        public async Task UpdateEntityAsync(TEntity entity)
        {
            DbContext.Attach(entity);
            entity.updated_at = DateTime.Now;
            DbContext.Entry(entity).State = EntityState.Modified;
            await Task.CompletedTask;
        }

        public IDbContextTransaction BeginTransaction()
        {
            return DbContext.Database.BeginTransaction();
        }
    }
}
