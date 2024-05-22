namespace Framework.Core.Abstractions
{
    public interface IPagedList<T>
    {
        /// <summary>
        /// Gets the page index (current).
        /// </summary>
        int page { get; }
        /// <summary>
        /// Gets the page size.
        /// </summary>
        int size { get; }
        /// <summary>
        /// Gets the total count of the list of type <typeparamref name="T"/>
        /// </summary>
        int total { get; }
        /// <summary>
        /// Gets the current page items.
        /// </summary>
        IList<T> data { get; }
    }
}
