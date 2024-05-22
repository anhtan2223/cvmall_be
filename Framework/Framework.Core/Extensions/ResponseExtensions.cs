using Framework.Core.Abstractions;
using Framework.Core.Collections;
using Framework.Core.Helpers;
using Microsoft.EntityFrameworkCore;

namespace Framework.Core.Extensions
{
    public static class ResponseExtensions
    {
        /// <summary>
        /// Converts the specified source to <see cref="IResponse{T}"/> from <see cref="T"/>.
        /// </summary>
        /// <typeparam name="T">The type of the source.</typeparam>
        /// <param name="source">The source to paging.</param>
        /// <param name="cancellationToken">
        ///     A <see cref="CancellationToken" /> to observe while waiting for the task to complete.
        /// </param>
        /// <returns>An instance of the inherited from <see cref="IResponse{T}"/> interface.</returns>
        public static BaseResponse<T> ToResponse<T>(this T source, CancellationToken cancellationToken = default(CancellationToken)) where T : class
        {
            var data = source;
            if (data != null)
                return new BaseResponse<T>(data, ResponseCode.Success, "Success");
            else
                return new BaseResponse<T>(data, ResponseCode.NotFound, "Data not found");
        }

        /// <summary>
        /// Converts the specified source to <see cref="IResponse{T}"/> from <see cref="Task{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of the source.</typeparam>
        /// <param name="source">The source to paging.</param>
        /// <param name="cancellationToken">
        ///     A <see cref="CancellationToken" /> to observe while waiting for the task to complete.
        /// </param>
        /// <returns>An instance of the inherited from <see cref="IResponse{T}"/> interface.</returns>
        public static async Task<BaseResponse<T>> ToResponse<T>(this Task<T> source, CancellationToken cancellationToken = default(CancellationToken)) where T : class
        {
            var data = await source;
            if (data != null)
                return new BaseResponse<T>(data, ResponseCode.Success, "Success");
            else
                return new BaseResponse<T>(data, ResponseCode.NotFound, "Data not found");
        }

        /// <summary>
        /// Converts the specified source to <see cref="IResponse{T}"/> .
        /// </summary>
        /// <typeparam name="T">The type of the source.</typeparam>
        /// <param name="source">The source to paging.</param>
        /// <param name="cancellationToken">
        ///     A <see cref="CancellationToken" /> to observe while waiting for the task to complete.
        /// </param>
        /// <returns>An instance of the inherited from <see cref="IResponse{T}"/> interface.</returns>
        public static async Task<BaseResponse<T>> ToResponse<T>(this IQueryable<T> source, CancellationToken cancellationToken = default(CancellationToken)) where T : class
        {
            var data = await source.FirstOrDefaultAsync();
            if (data != null)
                return new BaseResponse<T>(data, ResponseCode.Success, "Success");
            else
                return new BaseResponse<T>(data, ResponseCode.NotFound, "Data not found");
        }
    }
}
