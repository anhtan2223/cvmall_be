using Application.Common.Abstractions;
using DocumentFormat.OpenXml.InkML;
using Domain.Abstractions;
using Framework.Core.Abstractions;
using Framework.Core.Helpers.Cache;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data.Common;
using System.Transactions;

namespace Infrastructure.Repositories
{
    /// <summary>
    /// Represents the default implementation of the <see cref="IUnitOfWork"/> and <see cref="IUnitOfWork{TContext}"/> interface.
    /// </summary>
    /// <typeparam name="TContext">The type of the db context.</typeparam>
    public class BaseUnitOfWork<TContext> : IRepositoryFactory, IUnitOfWork<TContext>, IUnitOfWork where TContext : DbContext
    {
        private readonly TContext context;
        private bool disposed = false;
        private Dictionary<Type, object> repositories;
        private readonly ICachingService cachingService;
        protected IUserOrgInfoServices userOrgInfoServices;

        public TContext DbContext => context;

        public BaseUnitOfWork(TContext context) {
            context = context;
        }

        public BaseUnitOfWork(TContext _context, ICachingService _cachingService, IUserOrgInfoServices _userOrgInfoServices)
        {
            context = _context;
            cachingService = _cachingService;
            userOrgInfoServices = _userOrgInfoServices;
        }

        public IRepository<TEntity> GetRepository<TEntity>(bool hasCustomRepository = false) where TEntity : class, IAudit, ISoftDelete, IEntity<Guid>
        {
            if (repositories == null)
                repositories = new Dictionary<Type, object>();

            // what's the best way to support custom reposity?
            if (hasCustomRepository)
            {
                var customRepo = context.GetService<IRepository<TEntity>>();
                if (customRepo != null)
                {
                    return customRepo;
                }
            }

            var type = typeof(TEntity);
            if (!repositories.ContainsKey(type))
            {
                repositories[type] = new BaseRepository<TEntity>(context);
            }

            return (IRepository<TEntity>)repositories[type];
        }

        /// <summary>
        /// Saves all changes made in this context to the database.
        /// </summary>
        /// <returns>The number of state entries written to the database.</returns>
        public int SaveChanges() => context.SaveChanges();

        /// <summary>
        /// Asynchronously saves all changes made in this unit of work to the database.
        /// </summary>
        /// <returns>A <see cref="Task{TResult}"/> that represents the asynchronous save operation. The task result contains the number of state entities written to database.</returns>
        public async Task<int> SaveChangesAsync() => await context.SaveChangesAsync();

        public async Task<IDbContextTransaction> BeginTransactionAsync() => await context.Database.BeginTransactionAsync();

        public DbConnection GetConnection() => context.Database.GetDbConnection();

        /// <summary>
        /// Saves all changes made in this context to the database with distributed transaction.
        /// </summary>
        /// <param name="ensureAutoHistory"><c>True</c> if save changes ensure auto record the change history.</param>
        /// <param name="unitOfWorks">An optional <see cref="IUnitOfWork"/> array.</param>
        /// <returns>A <see cref="Task{TResult}"/> that represents the asynchronous save operation. The task result contains the number of state entities written to database.</returns>
        public async Task<int> SaveChangesAsync(bool ensureAutoHistory = false, params IUnitOfWork[] unitOfWorks)
        {
            using (var ts = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                var count = 0;
                foreach (var unitOfWork in unitOfWorks)
                {
                    count += await unitOfWork.SaveChangesAsync().ConfigureAwait(false);
                }

                count += await SaveChangesAsync();

                ts.Complete();

                return count;
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <param name="disposing">The disposing.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    // clear repositories
                    if (repositories != null)
                    {
                        repositories.Clear();
                    }

                    // dispose the db context.
                    context.Dispose();
                }
            }

            disposed = true;
        }

        public void TrackGraph(object rootEntity, Action<EntityEntryGraphNode> callback)
        {
            context.ChangeTracker.TrackGraph(rootEntity, callback);
        }


        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }
    }
}
