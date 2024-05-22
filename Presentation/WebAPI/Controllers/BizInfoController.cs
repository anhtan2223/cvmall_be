using Microsoft.AspNetCore.Mvc;
using Application.Core.Contracts;
using Framework.Core.Extensions;
using Framework.Core.Helpers;
using Application.Common.Abstractions;
using Application.Common.Extensions;
using WebAPI.Filters;
using Application.Core.Interfaces.Core;

namespace WebAPI.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [BAuthorize]
    [ApiController]
    [Route("api/biz_info")]
    public class BizInfoController : ControllerBase
    {
        private IBizInfoServices _bizInfoServices { get; set; }
        private ILocalizeServices ls { get; set; }

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="_functionServices"></param>
        /// <param name="_ls"></param>
        public BizInfoController(IBizInfoServices bizInfoServices, ILocalizeServices _ls) : base()
        {
            _bizInfoServices = bizInfoServices;
            ls = _ls;
        }

        /// <summary>
        /// Get list Technical category
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetPaged([FromQuery] RequestPaged request)
        {
            var data = await _bizInfoServices.GetPaged(request);
            return Ok(data);
        }

        /// <summary>
        /// Get Technical category by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var data = await _bizInfoServices.GetById(id);
            if (data != null)
                return Ok(data.ToResponse());
            else
                return Ok(data.ToResponse());
        }

        /// <summary>
        /// Create new Technical category
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("")]
        public async Task<IActionResult> Create([FromBody] BizInfoRequest request)
        {
            int count = await _bizInfoServices.Create(request);

            if (count >= 1)
                return Ok(new { code = ResponseCode.Success, message = ls.Get(Modules.Core, Screen.Message, MessageKey.I_001) });
            else
                return Ok(new { code = ResponseCode.SystemError, message = ls.Get(Modules.Core, Screen.Message, MessageKey.E_001) });
        }

        /// <summary>
        /// Update Technical category by id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] BizInfoRequest request)
        {
            var count = await _bizInfoServices.Update(id, request);
            if (count >= 1)
                return Ok(new { code = ResponseCode.Success, message = ls.Get(Modules.Core, Screen.Message, MessageKey.I_002) });
            else
                return Ok(new { code = ResponseCode.SystemError, message = ls.Get(Modules.Core, Screen.Message, MessageKey.E_002) });
        }

        /// <summary>
        /// Delete Technical category by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var count = await _bizInfoServices.Delete(id);

            if (count >= 1)
                return Ok(new { code = ResponseCode.Success, message = ls.Get(Modules.Core, Screen.Message, MessageKey.I_003) });
            else
                return Ok(new { code = ResponseCode.SystemError, message = ls.Get(Modules.Core, Screen.Message, MessageKey.E_003) });
        }
    }
}

