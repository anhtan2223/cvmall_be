using Application.Common.Abstractions;
using Application.Common.Extensions;
using Application.Core.Contracts;
using Application.Core.Interfaces.Core;
using Framework.Core.Extensions;
using Framework.Core.Helpers;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Filters;

namespace WebAPI.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private IAuthServices authServices { get; set; }
        private IUserServices userServices { get; set; }
        private ICurrentUserService servicesContext { get; set; }
        private ILocalizeServices ls { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_authServices"></param>
        /// <param name="_userServices"></param>
        /// <param name="_ls"></param>
        /// <param name="_servicesContext"></param>
        public AuthController(IAuthServices _authServices,
            IUserServices _userServices,
            ILocalizeServices _ls,
            ICurrentUserService _servicesContext) 
        {
            authServices = _authServices;
            userServices = _userServices;
            ls = _ls;
            servicesContext = _servicesContext;
        }

        /// <summary>
        /// Login
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] AuthLoginRequest request)
        {
            var res = await authServices.Login(request);
            if(res != null)
                return Ok(res.ToResponse());
            else
                return Unauthorized(res.ToResponse());
            
        }

        /// <summary>
        /// Refresh Token
        /// </summary>
        /// <param name="refresh_token"></param>
        /// <returns></returns>
        [HttpPost]
        [BAuthorize]
        [Route("refresh")]
        public async Task<IActionResult> RefreshToken([FromBody] string refresh_token)
        {
            var res = await authServices.Refresh(refresh_token);
            if (res != null)
                return Ok(res.ToResponse());
            else
                return Unauthorized(res.ToResponse());
        }

        /// <summary>
        /// Get info user login
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [BAuthorize]
        [Route("info")]
        public async Task<IActionResult> GetInfoLogin()
        {
            var data = await userServices.GetInfoLoginById((Guid)servicesContext.user_id);
            if (data != null)
                return Ok(data.ToResponse());
            else
                return BadRequest(data.ToResponse());
        }

        /// <summary>
        /// Change password
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut]
        [BAuthorize]
        [Route("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody]AuthChangePassRequest request)
        {
            var count = await authServices.ChangePassword((Guid)servicesContext.user_id, request);

            if (count >= 1)
                return Ok(new { code = ResponseCode.Success, message = ls.Get(Modules.Core, "Common", "UpdateSuccess") });
            else
                return BadRequest(new { code = ResponseCode.SystemError, message = ls.Get(Modules.Core, "Common", "UpdateError") });
        }
    }
}
