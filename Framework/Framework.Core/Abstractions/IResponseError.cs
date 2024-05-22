using Framework.Core.Helpers;

namespace Framework.Core.Abstractions
{
    public interface IResponseError
    {
        string name { get; }
        ResponseCode code { get; }
        string message { get; }
    }
}
