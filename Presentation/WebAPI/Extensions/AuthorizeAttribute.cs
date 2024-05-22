using Application.Common.Abstractions;
using Application.Common.Extensions;
using Application.Core.Interfaces.Core;
using Application.Core.Services;
using Framework.Core.Helpers;
using Framework.Core.Helpers.Auth;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace WebAPI.Filters
{
    /// <summary>
    /// 
    /// </summary>
    public class BAuthorizeAttribute : TypeFilterAttribute
    {
        /// <summary>
        /// 
        /// </summary>
        public BAuthorizeAttribute() : base(typeof(AuthorizeAttributeImpl))
        {

        }

        private class AuthorizeAttributeImpl : Attribute, IActionFilter, IAsyncActionFilter
        {
            private readonly IAuthServices _authService;
            private readonly ILocalizeServices _ls;
            private readonly IOptions<AuthSetting> _settings;
            private readonly ILogServices _logService;

            public AuthorizeAttributeImpl(IAuthServices authService, ILocalizeServices ls, IOptions<AuthSetting> settings, ILogServices logService)
            {
                _ls = ls;
                _authService = authService;
                _settings = settings;
                _logService = logService;
            }

            public void OnActionExecuting(ActionExecutingContext context)
            {
                //CheckUserPemission(context);
            }

            public void OnActionExecuted(ActionExecutedContext context)
            {
                // Do something after the action executes.
            }

            public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
            {
                //OnActionExecuting(context);

                await WriteLogAction(context);
                await next();
            }

            #region Private method
            private void CheckUserPemission(ActionExecutingContext context)
            {
                HttpRequest httpRequest = context.HttpContext.Request;
                string path = context.ActionDescriptor.AttributeRouteInfo.Template;
                string action = httpRequest.Method;

                var bearerToken = httpRequest.Headers["Authorization"];
                var token = !string.IsNullOrEmpty(bearerToken) ? bearerToken.ToString().Substring("Bearer ".Length) : null;
                var user_id = JwtHelpers.ValidateToken(token, _settings.Value.JWTSecret);
                if (user_id == null)
                {
                    context.Result = new UnauthorizedObjectResult(new { ResponseCode.UnAuthorized, message = _ls.Get(Modules.Core, "Common", "NotHavePermission") });
                    return;
                }

                // Get & check permission: user_id, path, action
                var check_auth = _authService.CheckUserAuthorized((Guid)user_id, path, action);

                if (!check_auth)
                {
                    context.Result = new UnauthorizedObjectResult(new { code = ResponseCode.UnAuthorized, message = _ls.Get(Modules.Core, "Common", "NotHavePermission") });
                    return;
                }
            }

            private async Task WriteLogAction(ActionExecutingContext context)
            {
                HttpRequest httpRequest = context.HttpContext.Request;
                string path = context.ActionDescriptor.AttributeRouteInfo.Template;
                string action = httpRequest.Method;

                string ipAddress = string.IsNullOrEmpty(httpRequest.Headers["X-Forwarded-For"]) ? "" : httpRequest.Headers["X-Forwarded-For"].FirstOrDefault();
                if (string.IsNullOrEmpty(ipAddress))
                    ipAddress = context.HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();

                 if (!string.IsNullOrEmpty(action) && action.ToUpper() != "GET")
                 {
                    // Lấy ra body
                    string body = string.Empty;
                    foreach (var item in context.ActionArguments)
                    {
                        body += $"{item.Key}: {JsonConvert.SerializeObject(item.Value)}\n";
                    }

                    //await _logService.WriteLogAction(path, action, ipAddress, body);
                 }
            }
            #endregion
        }
    }
}
