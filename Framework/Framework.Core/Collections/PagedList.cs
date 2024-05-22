using Framework.Core.Abstractions;

namespace Framework.Core.Collections
{
    /// <summary>
    /// Represents the default implementation of the <see cref="IPagedList{T}"/> interface.
    /// </summary>
    /// <typeparam name="T">The type of the data to page</typeparam>
    public class PagedList<T>
    {
        /// <summary>
        /// Gets or sets the index of the page.
        /// </summary>
        /// <value>The index of the page.</value>
        public int page { get; set; }
        /// <summary>
        /// Gets or sets the size of the page.
        /// </summary>
        /// <value>The size of the page.</value>
        public int size { get; set; }
        /// <summary>
        /// Gets or sets the total count.
        /// </summary>
        /// <value>The total count.</value>
        public int total { get; set; }

        /// <summary>
        /// Gets or sets the items.
        /// </summary>
        /// <value>The items.</value>
        public IList<T> data { get; set; }


        /// <summary>
        /// Initializes a new instance of the <see cref="PagedList{T}" /> class.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="page">The index of the page.</param>
        /// <param name="size">The size of the page.</param>
        internal PagedList(IEnumerable<T> source, int _page, int _size)
        {
            if (source is IQueryable<T> querable)
            {
                page = _page;
                size = _size;
                total = querable.Count();

                data = querable.Skip((page) * size).Take(size).ToList();
            }
            else
            {
                page = _page;
                size = _size;
                total = source.Count();

                data = source.Skip((page - 1) * size).Take(size).ToList();
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PagedList{T}" /> class.
        /// </summary>
        internal PagedList() => data = new T[0];
    }

    /// <summary>
    /// Provides the implementation of the <see cref="IPagedList{T}"/> and converter.
    /// </summary>
    /// <typeparam name="TSource">The type of the source.</typeparam>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    internal class PagedList<TSource, TResult> : PagedList<TResult>
    {
        /// <summary>
        /// Gets the index of the page.
        /// </summary>
        /// <value>The index of the page.</value>
        public int page { get; }
        /// <summary>
        /// Gets the size of the page.
        /// </summary>
        /// <value>The size of the page.</value>
        public int size { get; }
        /// <summary>
        /// Gets the total count.
        /// </summary>
        /// <value>The total count.</value>
        public int total { get; }

        /// <summary>
        /// Gets the items.
        /// </summary>
        /// <value>The items.</value>
        public IList<TResult> data { get; }


        /// <summary>
        /// Initializes a new instance of the <see cref="PagedList{TSource, TResult}" /> class.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="converter">The converter.</param>
        /// <param name="pageIndex">The index of the page.</param>
        /// <param name="pageSize">The size of the page.</param>
        public PagedList(IEnumerable<TSource> source, Func<IEnumerable<TSource>, IEnumerable<TResult>> converter, int _page, int _size)
        {
            if (source is IQueryable<TSource> querable)
            {
                page = _page;
                size = _size;
                total = querable.Count();

                var items = querable.Skip((page - 1) * size).Take(size).ToArray();

                data = new List<TResult>(converter(items));
            }
            else
            {
                page = _page;
                size = _size;
                total = source.Count();

                var items = source.Skip((page - 1) * size).Take(size).ToArray();

                data = new List<TResult>(converter(items));
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PagedList{TSource, TResult}" /> class.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="converter">The converter.</param>
        public PagedList(IPagedList<TSource> source, Func<IEnumerable<TSource>, IEnumerable<TResult>> converter)
        {
            page = source.page;
            size = source.size;
            total = source.total;

            data = new List<TResult>(converter(source.data));
        }
    }

    /// <summary>
    /// Provides some help methods for <see cref="IPagedList{T}"/> interface.
    /// </summary>
    public static class PagedList
    {
        /// <summary>
        /// Creates an empty of <see cref="PagedList{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type for paging </typeparam>
        /// <returns>An empty instance of <see cref="PagedList{T}"/>.</returns>
        public static PagedList<T> Empty<T>() => new PagedList<T>();

        /// <summary>
        /// Creates a new instance of <see cref="PagedList{TResult}"/> from source of <see cref="PagedList{TSource}"/> instance.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="converter">The converter.</param>
        /// <returns>An instance of <see cref="PagedList{TResult}"/>.</returns>
        public static PagedList<TResult> From<TResult, TSource>(IPagedList<TSource> source, Func<IEnumerable<TSource>, IEnumerable<TResult>> converter) => new PagedList<TSource, TResult>(source, converter);
    }
}
