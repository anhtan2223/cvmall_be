using Domain.Abstractions;
using System.Linq.Expressions;

namespace Framework.Core.Extensions
{
    public static class QueryExtensions
    {
        public static IQueryable<T> ExcludeSoftDeleted<T>(this IQueryable<T> source) where T : ISoftDelete
        {
            return source.Where(x => !x.del_flg);
        }

        public static bool Duplicated<T>(this IQueryable<T> source, Expression<Func<T, bool>> func)
        {
            var duplicated = source.Where(func).Any();
            return duplicated;
        }

        public static IQueryable<T> DefaultOrder<T>(this IQueryable<T> source) where T : IAudit
        {
            return source.OrderByDescending(x => x.updated_at);
        }

        public static IQueryable<T> FilterById<T>(this IQueryable<T> source, Guid id) where T : IEntity<Guid>
        {
            return source.Where(x => x.id == id);
        }

        public static IQueryable<T> FindActiveById<T>(this IQueryable<T> source, Guid id) where T : IEntity<Guid>, ISoftDelete
        {
            return source.FilterById(id).ExcludeSoftDeleted();
        }

        public static IQueryable<T> FilterByIds<T>(this IQueryable<T> source, Guid[] ids) where T : IEntity<Guid>
        {
            return ids != null ? source.Where(x => ids.Contains(x.id)) : source;
        }

    }
}