using Application.Common.Abstractions;
using Domain.Entities;
using Framework.Core.Helpers.Auth;
using Infrastructure.Contracts;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;

namespace WebAPI.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public class ApiServiceContext : ICurrentUserService
    {
        /// <summary>
        /// 
        /// </summary>
        public Guid? user_id { get; set; }
        public string? user_name { get; set; }
        public string? full_name { get; set; }

        private IOptions<AuthSetting> _settings { get; set; }
        private IHttpContextAccessor _accessor { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="settings"></param>
        /// <param name="accessor"></param>
        public ApiServiceContext(IOptions<AuthSetting> settings, IHttpContextAccessor accessor)
        {
            _settings = settings;
            _accessor = accessor;
            try
            {
                var bearerToken = accessor.HttpContext.Request.Headers["Authorization"];
                var token = !string.IsNullOrEmpty(bearerToken) ? bearerToken.ToString().Substring("Bearer ".Length) : null;
                var claims = JwtHelpers.GetClaimsByValidateToken(token, settings.Value.JWTSecret);
                if (claims != default)
                {
                    var _user_id = claims.FirstOrDefault(x => x.Type == nameof(User.id));
                    if (_user_id != default)
                    {
                        user_id = Guid.Parse(_user_id.Value);
                    }

                    var _user_name = claims.FirstOrDefault(x => x.Type == nameof(User.user_name));
                    if (_user_name != default)
                    {
                        user_name = _user_name.Value;
                    }

                    var _full_name = claims.FirstOrDefault(x => x.Type == nameof(User.full_name));
                    if (_full_name != default)
                    {
                        full_name = _full_name.Value;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            
        }
    }
}
