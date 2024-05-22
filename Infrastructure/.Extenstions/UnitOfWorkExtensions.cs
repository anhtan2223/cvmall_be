using Application.Common.Abstractions;
using Domain.Abstractions;
using Framework.Core.Abstractions;
using Framework.Core.Extenstions;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Extensions
{
    /// <summary>
    /// Extension methods for setting up unit of work related services in an <see cref="IServiceCollection"/>.
    /// </summary>
    public static class UnitOfWorkExtensions
    {
        /// <summary>
        /// Registers the unit of work given context as a service in the <see cref="IServiceCollection"/>.
        /// </summary>
        /// <typeparam name="TContext">The type of the db context.</typeparam>
        /// <param name="services">The <see cref="IServiceCollection"/> to add services to.</param>
        /// <returns>The same service collection so that multiple calls can be chained.</returns>
        /// <remarks>
        /// This method only support one db context, if been called more than once, will throw exception.
        /// </remarks>
        public static IServiceCollection AddUnitOfWork<TContext>(this IServiceCollection services) where TContext : DbContext
        {
            services.AddScoped<IUserOrgInfoServices, UserOrgInfoServices>();

            services.AddScoped<IRepositoryFactory, BaseUnitOfWork<TContext>>();
            // Following has a issue: IUnitOfWork cannot support multiple dbcontext/database, 
            // that means cannot call AddUnitOfWork<TContext> multiple times.
            // Solution: check IUnitOfWork whether or null
            services.AddScoped<IUnitOfWork, BaseUnitOfWork<TContext>>();
            services.AddScoped<IUnitOfWork<TContext>, BaseUnitOfWork<TContext>>();

            return services;
        }

        /// <summary>
        /// Registers the custom repository as a service in the <see cref="IServiceCollection"/>.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <typeparam name="TRepository">The type of the custom repositry.</typeparam>
        /// <param name="services">The <see cref="IServiceCollection"/> to add services to.</param>
        /// <returns>The same service collection so that multiple calls can be chained.</returns>
        public static IServiceCollection AddCustomRepository<TEntity, TRepository>(this IServiceCollection services)
            where TEntity : class, IAudit, ISoftDelete, IEntity<Guid>
            where TRepository : class, IRepository<TEntity>
        {
            services.AddScoped<IRepository<TEntity>, TRepository>();

            return services;
        }
    }
}
