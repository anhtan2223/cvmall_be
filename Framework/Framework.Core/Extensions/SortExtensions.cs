using System.Linq.Expressions;

namespace Framework.Core.Extensions
{
    public static class SortExtensions
    {
        public static IQueryable<T> SortBy<T>(this IQueryable<T> source, string? orderBy)
        {
            if (orderBy == null) return source;

            foreach (var item in orderBy.Split(","))
            {
                var keys = item.Split(".");
                source = source.GetOrderByQuery(keys[0], keys[1] == "asc" ? "OrderBy" : "OrderByDescending");
            }
            return source;
        }

        private static IQueryable<T> GetOrderByQuery<T>(this IQueryable<T> source, string orderBy, string methodName)
        {
            var sourceType = typeof(T);
            var property = sourceType.GetProperty(orderBy);
            var parameterExpression = Expression.Parameter(sourceType, "x");
            var getPropertyExpression = Expression.MakeMemberAccess(parameterExpression, property);
            var orderByExpression = Expression.Lambda(getPropertyExpression, parameterExpression);
            var resultExpression = Expression.Call(typeof(Queryable), methodName,
                                                   new[] { sourceType, property.PropertyType }, source.Expression,
                                                   orderByExpression);

            return source.Provider.CreateQuery<T>(resultExpression);
        }
    }
}
