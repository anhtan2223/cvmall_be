using Framework.Core.Abstractions;
using Framework.Core.Helpers;

namespace Framework.Core.Collections
{
    public class BaseResponseErrorItem
    {
        public string name { get; set; }

        public ResponseCode code { get; set; }

        public string message { get; set; }

        public BaseResponseErrorItem(string _name, ResponseCode _code, string _message)
        {
            name = _name;
            code = _code;
            message = _message;
        }

        public BaseResponseErrorItem(string _name, string _message)
        {
            name = _name;
            message = _message;
        }
    }
}
