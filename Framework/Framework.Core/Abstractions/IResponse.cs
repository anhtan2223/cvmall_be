using Framework.Core.Helpers;

namespace Framework.Core.Abstractions
{
    public interface IResponse
    {
        ResponseCode code { get; }
        string message { get; }
    }
}
