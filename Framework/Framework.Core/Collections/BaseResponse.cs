using Framework.Core.Abstractions;
using Framework.Core.Helpers;

namespace Framework.Core.Collections
{
    public class BaseResponse
    {
        public ResponseCode code { get; set; }

        public string message { get; set; }

        public BaseResponse(ResponseCode _code, string _message)
        {
            code = _code;
            message = _message;
        }
    }
}
