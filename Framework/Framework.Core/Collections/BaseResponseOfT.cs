using Framework.Core.Abstractions;
using Framework.Core.Helpers;

namespace Framework.Core.Collections
{
    public class BaseResponse<T> where T : class
    {
        public ResponseCode code { get; set; }

        public string message { get; set; }

        public T data { get; set; }

        public IEnumerable<T> errors => null;

        public BaseResponse(T _data, ResponseCode _code, string _message)
        {
            code = _code;
            message = _message;
            data = _data;
        }

        public BaseResponse(T _data, ResponseCode _code)
        {
            code = _code;
            data = _data;
        }
    }
}
