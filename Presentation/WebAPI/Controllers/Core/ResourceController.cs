using Microsoft.AspNetCore.Mvc;
using Application.Core.Contracts;
using Framework.Core.Extensions;
using Framework.Core.Helpers;
using Application.Common.Abstractions;
using Application.Common.Extensions;
using Application.Core.Interfaces.Core;

namespace WebAPI.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [ApiController]
    [Route("api/resource")]
    public class ResourceController : ControllerBase
    {
        private IResourceServices resourceServices { get; set; }
        private ILocalizeServices ls { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_resourceServices"></param>
        /// <param name="_localServices"></param>
        public ResourceController(IResourceServices _resourceServices, ILocalizeServices _localServices) : base()
        {
            resourceServices = _resourceServices;
            ls = _localServices;
        }

        /// <summary>
        /// Get list Resource
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] ResourcePagedRequest request)
        {
            var data = await resourceServices.GetPaged(request);
            return Ok(data);
        }
        /// <summary>
        /// Get Resource by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var data = await resourceServices.GetById(id);
            if (data != null)
                return Ok(data.ToResponse());
            else
                return BadRequest(data?.ToResponse());
        }

        /// <summary>
        /// Create new Resource
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ResourceRequest request)
        {
            int count = await resourceServices.Create(request);

            if (count >= 1)
                return Ok(new { code = ResponseCode.Success, message = ls.Get(Modules.Core, "Resource", "CreateSuccess") });
            else
                return BadRequest(new { code = ResponseCode.SystemError, message = ls.Get(Modules.Core, "Common", "HasError") });
        }

        /// <summary>
        /// Update Resource by id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] ResourceRequest request)
        {
            var count = await resourceServices.Update(id, request);
            if (count >= 1)
                return Ok(new { code = ResponseCode.Success, message = ls.Get(Modules.Core, "Resource", "UpdateSuccess") });
            else
                return BadRequest(new { code = ResponseCode.SystemError, message = ls.Get(Modules.Core, "Common", "HasError") });
        }

        /// <summary>
        /// Delete Resource by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var count = await resourceServices.Delete(id);

            if (count >= 1)
                return Ok(new { code = ResponseCode.Success, message = ls.Get(Modules.Core, "Resource", "DeleteSuccess") });
            else
                return BadRequest(new { code = ResponseCode.SystemError, message = ls.Get(Modules.Core, "Common", "HasError") });
        }

        /// <summary>
        /// Get list Screen
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("screens")]
        public async Task<IActionResult> GetListScreen([FromQuery] ResourcePagedRequest request)
        {
            var data = await resourceServices.GetScreensByModule(request);
            return Ok(data);
        }
    }
}
