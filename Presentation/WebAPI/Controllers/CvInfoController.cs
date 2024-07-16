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
    [Route("api/cv_info")]
    public class CvInfoController : ControllerBase
    {
        private ICvInfoServices cvInfoServices { get; set; }
        private ILocalizeServices ls { get; set; }

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="_functionServices"></param>
        /// <param name="_ls"></param>
        public CvInfoController(ICvInfoServices _cvInfoServices, ILocalizeServices _ls) : base()
        {
            cvInfoServices = _cvInfoServices;
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
            var data = await cvInfoServices.GetPaged(request);
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
            var data = await cvInfoServices.GetById(id);
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
        public async Task<IActionResult> Create([FromBody] CvInfoRequest request)
        {
            try
            {
                int count = await cvInfoServices.Create(request);

                if (count >= 1)
                    return Ok(new { code = ResponseCode.Success, message = ls.Get(Modules.Core, Screen.Message, MessageKey.I_001) });
                else
                    return Ok(new { code = ResponseCode.SystemError, message = ls.Get(Modules.Core, Screen.Message, MessageKey.E_001) });
            }
            catch (Exception ex)
            {
                return Ok(new { code = ResponseCode.SystemError, message = ex.Message });
            }
        }

        /// <summary>
        /// Update Technical category by id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] CvInfoRequest request)
        {
            try
            {
                var count = await cvInfoServices.Update(id, request);
                if (count >= 1)
                    return Ok(new { code = ResponseCode.Success, message = ls.Get(Modules.Core, Screen.Message, MessageKey.I_002) });
                else
                    return Ok(new { code = ResponseCode.SystemError, message = ls.Get(Modules.Core, Screen.Message, MessageKey.E_002) });
            }
            catch (Exception ex)
            {
                return Ok(new { code = ResponseCode.SystemError, message = ex.Message });
            }
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
            var count = await cvInfoServices.Delete(id);

            if (count >= 1)
                return Ok(new { code = ResponseCode.Success, message = ls.Get(Modules.Core, Screen.Message, MessageKey.I_003) });
            else
                return Ok(new { code = ResponseCode.SystemError, message = ls.Get(Modules.Core, Screen.Message, MessageKey.E_003) });
        }

        // <summary>
        /// Export excel CV detail
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{id}/export")]
        public async Task<IActionResult> ExportExcelCVDetail(Guid id)
        {
            var fileName = $"CV_{id}_{DateTimeExtensions.ToDateTimeStampString(DateTime.Now)}.zip";

            var fileData = await cvInfoServices.ExportAndZipCVDetail(id);

            if (fileData == null)
                return BadRequest(new { code = ResponseCode.NotFound, message = ls.Get(Modules.Core, Screen.Message, MessageKey.E_007) });

            return File(fileData, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }

        /// <summary>
        /// Export excel CV template
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("export_template")]
        public async Task<IActionResult> ExportExcelCVTemplate()
        {
            var fileName = $"CV_template.zip";

            var fileData = await cvInfoServices.ExportAndZipCVTemplate();

            if (fileData == null)
                return BadRequest(new { code = ResponseCode.NotFound, message = ls.Get(Modules.Core, Screen.Message, MessageKey.E_007) });

            return File(fileData, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }

        // <summary>
        /// Export excel all CV
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("export_all")]
        public async Task<IActionResult> ExportAndZipAllCVs()
        {
            var fileName = $"CVs_{DateTimeExtensions.ToDateTimeStampString(DateTime.Now)}.zip";

            var fileData = await cvInfoServices.ExportAndZipAllCVs();

            if (fileData == null)
                return BadRequest(new { code = ResponseCode.NotFound, message = ls.Get(Modules.Core, Screen.Message, MessageKey.E_007) });

            return File(fileData, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }
    }
}

