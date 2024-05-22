using Domain.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Application.Common.Abstractions
{
    public interface IRepository<TEntity> where TEntity : class, IAudit, ISoftDelete, IEntity<Guid>
    {
        Task<TEntity> FindAsync(Guid id);

        IQueryable<TEntity> GetQuery(Func<DbSet<TEntity>, IQueryable<TEntity>>? func = null);

        Task<bool> ExistByIdAsync(Guid id);

        Task AddEntityAsync(TEntity entity);

        Task UpdateEntityAsync(TEntity entity);

        Task DeleteEntityAsync(TEntity entity);

        IDbContextTransaction BeginTransaction();
    }
}
