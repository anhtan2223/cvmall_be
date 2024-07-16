using Microsoft.AspNetCore.Mvc;
using Application.Core.Contracts;
using Framework.Core.Extensions;
using Framework.Core.Helpers;
using Application.Common.Abstractions;
using Application.Common.Extensions;
using WebAPI.Filters;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml;
using Framework.Core.Helpers.Excel;
using Application.Core.Interfaces.Core;

namespace WebAPI.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [ApiController]
    [BAuthorize]
    [Route("api/user")]
    public class UserController : ControllerBase
    {
        private IUserServices userServices { get; set; }
        private ILocalizeServices ls { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_userServices"></param>
        /// <param name="_localServices"></param>
        public UserController(IUserServices _userServices, ILocalizeServices _localServices) : base()
        {
            userServices = _userServices;
            ls = _localServices;
        }

        /// <summary>
        /// Get list user
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] UserSearchRequest request)
        {
            var data = await userServices.GetPaged(request);
            return Ok(data);
        }

        /// <summary>
        /// Export excel
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("export-excel")]
        public async Task<IActionResult> ExportExcel([FromQuery] UserSearchRequest request)
        {
            var fileName = $"User_{DateTimeExtensions.ToDateTimeStampString(DateTime.Now)}.xlsx";

            var data = await userServices.GetPaged(request);

            if (data == null)
                return BadRequest(new { code = ResponseCode.NotFound, message = ls.Get(Modules.Core, Screen.Message, MessageKey.E_007) });

            using (var ms = new MemoryStream())
            {
                using (SpreadsheetDocument document = SpreadsheetDocument.Create(ms, SpreadsheetDocumentType.Workbook))
                {
                    document.CreateWorkbookPart();
                    WorkbookPart? workbookpart = document.WorkbookPart;
                    workbookpart?.CreateSheet("Sheet1");

                    List<ExcelItem> excelItems = new() {
                        new ExcelItem(){ header =  ls.Get(Modules.Core, Screen.User, "UserName"), key = "user_name", type = DataType.TEXT, header_align = CellAlign.CENTER, content_align = CellAlign.LEFT ,width=15},
                        new ExcelItem(){ header =  ls.Get(Modules.Core, Screen.User, "FullName"), key = "full_name", type = DataType.TEXT, header_align = CellAlign.CENTER, content_align = CellAlign.LEFT ,width=30},
                        new ExcelItem(){ header =  ls.Get(Modules.Core, Screen.User, "Email"), key = "mail", type = DataType.TEXT, header_align = CellAlign.CENTER, content_align = CellAlign.LEFT ,width=15},
                        new ExcelItem(){ header =  ls.Get(Modules.Core, Screen.User, "Phone"), key = "phone", type = DataType.TEXT, header_align = CellAlign.CENTER, content_align = CellAlign.LEFT ,width=15},
                    };

                    workbookpart?.FillGridData(data.data.ToList(), excelItems);

                }
                return File(ms.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
            }
        }

        /// <summary>
        /// Get user by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var data = await userServices.GetById(id);
            if (data != null)
                return Ok(data.ToResponse());
            else
                return BadRequest(new { code = ResponseCode.NotFound, message = ls.Get(Modules.Core, Screen.Message, MessageKey.E_007) });
        }

        /// <summary>
        /// Create new user
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> Create([FromBody] UserRequest request)
        {
            int count = await userServices.Create(request);

            if(count >= 1)
                return Ok(new { code = ResponseCode.Success, message = ls.Get(Modules.Core, Screen.Message, MessageKey.I_001) });
            else
                return BadRequest(new { code = ResponseCode.SystemError, message = ls.Get(Modules.Core, Screen.Message, MessageKey.E_001) });
        }

        /// <summary>
        /// Update user by id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("update/{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UserRequest request)
        {
           var count = await userServices.Update(id, request);
            if (count >= 1)
                return Ok(new { code = ResponseCode.Success, message = ls.Get(Modules.Core, Screen.Message, MessageKey.I_002) });
            else if (count == -1)
                return Ok(new { code = ResponseCode.NotFound, message = ls.Get(Modules.Core, Screen.Message, MessageKey.E_007) });
            else
                return BadRequest(new { code = ResponseCode.SystemError, message = ls.Get(Modules.Core, Screen.Message, MessageKey.E_002) });
        }

        /// <summary>
        /// Delete user by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("delete/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var count = await userServices.Delete(id);

            if (count >= 1)
                return Ok(new { code = ResponseCode.Success, message = ls.Get(Modules.Core, Screen.Message, MessageKey.I_003) });
            else if (count == -1)
                return Ok(new { code = ResponseCode.NotFound, message = ls.Get(Modules.Core, Screen.Message, MessageKey.E_007) });
            else
                return BadRequest(new { code = ResponseCode.SystemError, message = ls.Get(Modules.Core, Screen.Message, MessageKey.E_003) });
        }
    }
}
