using Microsoft.AspNetCore.Mvc;
using Application.Core.Contracts;
using Framework.Core.Extensions;
using Framework.Core.Helpers;
using Application.Common.Abstractions;
using Application.Common.Extensions;
using Framework.Core.Collections;
using WebAPI.Filters;
using Application.Core.Interfaces.Core;

namespace WebAPI.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Route("api/role")]
    [ApiController]
    [BAuthorize]
    public class RoleController : ControllerBase
    {
        private IRoleServices roleServices { get; set; }
        private ILocalizeServices ls { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_roleServices"></param>
        /// <param name="_ls"></param>
        public RoleController(IRoleServices _roleServices, ILocalizeServices _ls) : base()
        {
            roleServices = _roleServices;
            ls = _ls;
        }

        /// <summary>
        /// Get list role
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] PagedRequest request)
        {
            var data = await roleServices.GetPaged(request);
            return Ok(data);
        }

        /// <summary>
        /// Get role by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var data = await roleServices.GetById(id);
            if (data != null)
                return Ok(data.ToResponse());
            else
                return BadRequest(data?.ToResponse());
        }

        /// <summary>
        /// Create new role
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] RoleRequest request)
        {
            int count = await roleServices.Create(request);

            if (count >= 1)
                return Ok(new { code = ResponseCode.Success, message = ls.Get(Modules.Core, "Role", "CreateSuccess") });
            else
                return BadRequest(new { code = ResponseCode.SystemError, message = ls.Get(Modules.Core, "Role", "CreateError") });
        }

        /// <summary>
        /// Update role by id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] RoleRequest request)
        {
            var count = await roleServices.Update(id, request);
            if (count >= 1)
                return Ok(new { code = ResponseCode.Success, message = ls.Get(Modules.Core, "Role", "UpdateSuccess") });
            else
                return BadRequest(new { code = ResponseCode.SystemError, message = ls.Get(Modules.Core, "Role", "UpdateError") });
        }

        /// <summary>
        /// Delete role by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var count = await roleServices.Delete(id);

            if (count >= 1)
                return Ok(new { code = ResponseCode.Success, message = ls.Get(Modules.Core, "Role", "DeleteSuccess") });
            else
                return BadRequest(new { code = ResponseCode.SystemError, message = ls.Get(Modules.Core, "Role", "DeleteError") });
        }

        /// <summary>
        /// Delete role by ids
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("delete-multi")]
        public async Task<IActionResult> Delete([FromBody] Guid[] ids)
        {
            var count = await roleServices.Delete(ids);

            if (count >= 1)
                return Ok(new { code = ResponseCode.Success, message = ls.Get(Modules.Core, "Role", "DeleteSuccess") });
            else
                return BadRequest(new { code = ResponseCode.SystemError, message = ls.Get(Modules.Core, "Role", "DeleteError") });
        }
    }
}
