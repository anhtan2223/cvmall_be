using Framework.Core.Helpers;

namespace Framework.Core.Abstractions
{
    public interface IResponse<T>
    {
        ResponseCode code { get; }
        string message { get; }
        IEnumerable<T> errors { get; }
        T data { get; }
    }
}
