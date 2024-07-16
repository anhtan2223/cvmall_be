using Microsoft.AspNetCore.Mvc;
using Application.Core.Contracts;
using Framework.Core.Extensions;
using Framework.Core.Helpers;
using Application.Common.Abstractions;
using Application.Common.Extensions;
using Framework.Core.Collections;
using WebAPI.Filters;
using Framework.Core.Helpers.Excel;
using Application.Core.Interfaces.Core;

namespace WebAPI.Controllers.Core
{
    /// <summary>
    /// 
    /// </summary>
    [ApiController]
    [BAuthorize]
    [Route("api/master_code")]
    public class MasterCodeController : ControllerBase
    {
        private IMasterCodeServices masterCodeServices { get; set; }
        private ILocalizeServices ls { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_masterCodeServices"></param>
        /// <param name="_localServices"></param>
        public MasterCodeController(IMasterCodeServices _masterCodeServices,ILocalizeServices _localServices) : base()
        {
            masterCodeServices = _masterCodeServices;
            ls = _localServices;
        }

        /// <summary>
        /// Get list master code
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] PagedRequest request)
        {
            var data = await masterCodeServices.GetPaged(request);
            return Ok(data);
        }

        /// <summary>
        /// Get list master code by type
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("get_by_type")]
        public async Task<IActionResult> GetByTypePaged([FromQuery] MasterCodePagedRequest request)
        {
            var data = await masterCodeServices.GetByTypePaged(request);
            return Ok(data);
        }

        /// <summary>
        /// Get masterCode by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var data = await masterCodeServices.GetById(id);
            if (data != null)
                return Ok(data.ToResponse());
            else
                return BadRequest(data?.ToResponse());
        }

        /// <summary>
        /// Create new masterCode
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] MasterCodeRequest request)
        {
            int count = await masterCodeServices.Create(request);

            if (count >= 1)
                return Ok(new { code = ResponseCode.Success, message = ls.Get(Modules.Core, "MasterCode", "CreateSuccess") });
            else
                return BadRequest(new { code = ResponseCode.SystemError, message = ls.Get(Modules.Core, "Common", "HasError") });
        }

        /// <summary>
        /// Update masterCode by id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] MasterCodeRequest request)
        {
            var count = await masterCodeServices.Update(id, request);
            if (count >= 1)
                return Ok(new { code = ResponseCode.Success, message = ls.Get(Modules.Core, "MasterCode", "UpdateSuccess") });
            else
                return BadRequest(new { code = ResponseCode.SystemError, message = ls.Get(Modules.Core, "Common", "HasError") });
        }

        /// <summary>
        /// Delete masterCode by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var count = await masterCodeServices.Delete(id);

            if (count >= 1)
                return Ok(new { code = ResponseCode.Success, message = ls.Get(Modules.Core, "MasterCode", "DeleteSuccess") });
            else
                return BadRequest(new { code = ResponseCode.SystemError, message = ls.Get(Modules.Core, "Common", "HasError") });
        }

        /// <summary>
        /// Delete masterCode by ids
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("delete-multi")]
        public async Task<IActionResult> DeleteMulti([FromBody] Guid[] ids)
        {
            var count = await masterCodeServices.Delete(ids);

            if (count >= 1)
                return Ok(new { code = ResponseCode.Success, message = ls.Get(Modules.Core, "MasterCode", "DeleteMultipleSuccess") });
            else
                return BadRequest(new { code = ResponseCode.SystemError, message = ls.Get(Modules.Core, "Common", "HasError") });
        }

        /// <summary>
        /// ExportCsv
        /// </summary>
        /// <param name="request"></param>
        /// <param name="file_name"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("export_csv")]
        public async Task<FileResult> ExportCsv([FromQuery] PagedRequest request, [FromQuery] string file_name)
        {

            var dataResponse = await masterCodeServices.GetPaged(request);

            List<CsvItem> csvItems = new List<CsvItem>()
            {   
                new CsvItem() { header = await ls.GetAsync(Modules.Core, "MasterCode", "Type"), key = "type", type = DataType.TEXT },
                new CsvItem() {  header = await ls.GetAsync(Modules.Core, "MasterCode", "Key"), key = "key", type = DataType.TEXT },
                new CsvItem() {  header = await ls.GetAsync(Modules.Core, "MasterCode", "Value"), key = "value", type = DataType.TEXT }
            };

            var fileStream = await CsvHelpers.ExportCsv(dataResponse.data.ToList(), csvItems);
            return File(fileStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", file_name);
        }

        /// <summary>
        /// ExportExel
        /// </summary>
        /// <param name="request"></param>
        /// <param name="file_name"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("export_excel")]
        public async Task<IActionResult> ExportExel([FromQuery] PagedRequest request, [FromQuery] string file_name)
        {
            var dataResponse = await masterCodeServices.GetPaged(request);

            List<ExcelItem> excelItems = new List<ExcelItem>()
            {
                new ExcelItem() { header = await ls.GetAsync(Modules.Core, "MasterCode", "Type"), key = "type", type = DataType.TEXT, header_align = CellAlign.CENTER, content_align = CellAlign.LEFT },
                new ExcelItem() {  header = await ls.GetAsync(Modules.Core, "MasterCode", "Key"), key = "key", type = DataType.TEXT, header_align = CellAlign.CENTER, content_align = CellAlign.LEFT },
                new ExcelItem() {  header = await ls.GetAsync(Modules.Core, "MasterCode", "Value"), key = "value", type = DataType.TEXT, header_align = CellAlign.CENTER, content_align = CellAlign.LEFT },
                new ExcelItem() {  header = await ls.GetAsync(Modules.Core, "MasterCode", "asd"), key = "created_at", type = DataType.TEXT, header_align = CellAlign.CENTER, content_align = CellAlign.LEFT }
            };
            var fileStream = await ExcelHelpers.Export(dataResponse.data.ToList(), excelItems);

            return File(fileStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", file_name);
        }
    }
}
