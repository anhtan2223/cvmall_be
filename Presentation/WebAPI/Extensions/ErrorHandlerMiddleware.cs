using Application.Common.Abstractions;
using Application.Common.Extensions;
using Application.Core.Interfaces.Core;
using FluentValidation;
using Framework.Core.Collections;
using Framework.Core.Extensions;
using Framework.Core.Helpers;
using System.Net;
using System.Text.Json;

namespace WebAPI.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="next"></param>
        public ErrorHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="logService"></param>
        /// <param name="ls"></param>
        /// <returns></returns>
        public async Task Invoke(HttpContext context, ILogServices logService, ILocalizeServices ls)
        {
            try
            {
                await _next(context);
            }
            catch (ValidationException vex)
            {
                var response = context.Response;
                response.ContentType = "application/json";
                response.StatusCode = (int)HttpStatusCode.OK;

                var result = JsonSerializer.Serialize(vex.ToResponse());
                await response.WriteAsync(result);
            }
            catch (Exception error)
            {
                var response = context.Response;
                response.ContentType = "application/json";

                switch (error)
                {
                    case KeyNotFoundException e:
                        // not found error
                        response.StatusCode = (int)HttpStatusCode.NotFound;
                        break;
                    default:
                        // unhandled error
                        response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        break;
                }

                // Write log exception
                if (error != null)
                {
                    string path = context.Request.Path;
                    string action = context.Request.Method;

                    await logService.WriteLogException($"{action} - {path}", error.Message, error.StackTrace);
                }
                //var result = JsonSerializer.Serialize(new BaseResponseError(ResponseCode.SystemError, error?.Message));
                var result = JsonSerializer.Serialize(new BaseResponseError(ResponseCode.SystemError, ls.Get(Modules.Core, "Message", MessageKey.BE_003)));
                await response.WriteAsync(result);
            }
        }
    }
}
