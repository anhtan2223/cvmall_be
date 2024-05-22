using Framework.Core.Abstractions;
using Framework.Core.Helpers;

namespace Framework.Core.Collections
{
    public class BaseResponseError
    {
        public ResponseCode code { get; set; }

        public string message { get; set; }

        public BaseResponseError(ResponseCode _code, string _message)
        {
            code = _code;
            message = _message;
        }

    }
    public class BaseResponseError<T> : IResponse<T> where T : class
    {
        public ResponseCode code { get; set; }

        public string message { get; set; }

        public IEnumerable<T> errors { get; set; }

        public T data => null;

        public BaseResponseError(IEnumerable<T> _errors, ResponseCode _code, string _message)
        {
            code = _code;
            message = _message;
            errors = _errors;
        }
    }
}
