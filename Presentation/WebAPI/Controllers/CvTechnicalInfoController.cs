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
    [Route("api/cv_technical_info")]
    public class CvTechnicalInfoController : ControllerBase
    {
        private ICvTechnicalInfoServices _cvTechnicalInfoServices { get; set; }
        private ILocalizeServices ls { get; set; }

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="_functionServices"></param>
        /// <param name="_ls"></param>
        public CvTechnicalInfoController(ICvTechnicalInfoServices cvTechnicalInfoServices, ILocalizeServices _ls) : base()
        {
            _cvTechnicalInfoServices = cvTechnicalInfoServices;
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
            var data = await _cvTechnicalInfoServices.GetPaged(request);
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
            var data = await _cvTechnicalInfoServices.GetById(id);
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
        public async Task<IActionResult> Create([FromBody] CvTechnicalInfoRequest request)
        {
            int count = await _cvTechnicalInfoServices.Create(request);

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
        public async Task<IActionResult> Update(Guid id, [FromBody] CvTechnicalInfoRequest request)
        {
            var count = await _cvTechnicalInfoServices.Update(id, request);
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
            var count = await _cvTechnicalInfoServices.Delete(id);

            if (count >= 1)
                return Ok(new { code = ResponseCode.Success, message = ls.Get(Modules.Core, Screen.Message, MessageKey.I_003) });
            else
                return Ok(new { code = ResponseCode.SystemError, message = ls.Get(Modules.Core, Screen.Message, MessageKey.E_003) });
        }
    }
}

