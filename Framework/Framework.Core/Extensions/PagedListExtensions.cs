using Framework.Core.Abstractions;
using Framework.Core.Collections;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Framework.Core.Extensions
{
    public static class PagedListExtensions
    {
        /// <summary>
        /// Converts the specified source to <see cref="IPagedList{T}"/> by the specified <paramref name="page"/> and <paramref name="size"/>.
        /// </summary>
        /// <typeparam name="T">The type of the source.</typeparam>
        /// <param name="source">The source to paging.</param>
        /// <param name="page">The index of the page.</param>
        /// <param name="size">The size of the page.</param>
        /// <returns>An instance of the inherited from <see cref="IPagedList{T}"/> interface.</returns>
        public static PagedList<T> ToPagedList<T>(this IEnumerable<T> source, int page, int size) => new PagedList<T>(source, page, size);

        /// <summary>
        /// Converts the specified source to <see cref="PagedList{T}"/> by the specified <paramref name="converter"/>, <paramref name="page"/> and <paramref name="size"/>
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <typeparam name="TResult">The type of the result</typeparam>
        /// <param name="source">The source to convert.</param>
        /// <param name="converter">The converter to change the <typeparamref name="TSource"/> to <typeparamref name="TResult"/>.</param>
        /// <param name="page">The page index.</param>
        /// <param name="size">The page size.</param>
        /// <returns>An instance of the inherited from <see cref="PagedList{T}"/> interface.</returns>
        public static PagedList<TResult> ToPagedList<TSource, TResult>(this IEnumerable<TSource> source, Func<IEnumerable<TSource>, IEnumerable<TResult>> converter, int page, int size)
        {
            return new PagedList<TSource, TResult>(source, converter, page, size);
        }

        /// <summary>
        /// Converts the specified source to <see cref="IPagedList{T}"/> by the specified <paramref name="page"/> and <paramref name="size"/>.
        /// </summary>
        /// <typeparam name="T">The type of the source.</typeparam>
        /// <param name="source">The source to paging.</param>
        /// <param name="page">The index of the page.</param>
        /// <param name="size">The size of the page.</param>
        /// <param name="cancellationToken">
        ///     A <see cref="CancellationToken" /> to observe while waiting for the task to complete.
        /// </param>
        /// <returns>An instance of the inherited from <see cref="IPagedList{T}"/> interface.</returns>
        public static async Task<PagedList<T>> ToPagedListAsync<T>(this IQueryable<T> source, int page, int size, CancellationToken cancellationToken = default(CancellationToken))
        {
            var count = await source.CountAsync(cancellationToken).ConfigureAwait(false);
            var items = await source.Skip((page - 1) * size)
                                    .Take(size).ToListAsync(cancellationToken).ConfigureAwait(false);

            var pagedList = new PagedList<T>()
            {
                page = page,
                size = size,
                total = count,
                data = items,
            };

            return pagedList;
        }

        public static PagedList<T> ToPagedList<T>(this IQueryable<T> source, int page, int size)
        {
            var count = source.Count();
            var items = source.Skip((page - 1) * size).Take(size).ToList();

            var pagedList = new PagedList<T>()
            {
                page = page,
                size = size,
                total = count,
                data = items,
            };

            return pagedList;
        }

        public static PagedList<T> ToAllPageList<T>(this IQueryable<T> source)
        {
            var pagedList = new PagedList<T>()
            {
                total= source.Count(),
                data = source.ToList(),
            };

            return pagedList;
        }
    }
}
