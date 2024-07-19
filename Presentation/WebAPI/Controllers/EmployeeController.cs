using Microsoft.AspNetCore.Mvc;
using Application.Core.Contracts;
using Framework.Core.Extensions;
using Framework.Core.Helpers;
using Application.Common.Abstractions;
using Application.Common.Extensions;
using WebAPI.Filters;
using Application.Core.Interfaces.Core;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;

namespace WebAPI.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [BAuthorize]
    [ApiController]
    [Route("api/employee")]
    public class EmployeeController : ControllerBase
    {
        private IEmployeeServices _employeeServices { get; set; }
        private ILocalizeServices ls { get; set; }

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="_functionServices"></param>
        /// <param name="_ls"></param>
        public EmployeeController(IEmployeeServices bizInfoServices, ILocalizeServices _ls) : base()
        {
            _employeeServices = bizInfoServices;
            ls = _ls;
        }

        /// <summary>
        /// Get list Employee
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetPaged([FromQuery] RequestEmployeePaged request)
        {
            var data = await _employeeServices.GetPaged(request);
            return Ok(data);
        }

        /// <summary>
        /// Get Employee by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var data = await _employeeServices.GetById(id);
            if (data != null)
                return Ok(data.ToResponse());
            else
                return Ok(data.ToResponse());
        }

        /// <summary>
        /// Create new Employee
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("")]
        public async Task<IActionResult> Create([FromBody] EmployeeRequest request)
        {
            int count = await _employeeServices.Create(request);

            if (count >= 1)
                return Ok(new { code = ResponseCode.Success, message = ls.Get(Modules.Core, Screen.Message, MessageKey.I_001) });
            else
                return Ok(new { code = ResponseCode.SystemError, message = ls.Get(Modules.Core, Screen.Message, MessageKey.E_001) });
        }

        /// <summary>
        /// Update Employee by id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] EmployeeRequest request)
        {
            var count = await _employeeServices.Update(id, request);
            if (count >= 1)
                return Ok(new { code = ResponseCode.Success, message = ls.Get(Modules.Core, Screen.Message, MessageKey.I_002) });
            else
                return Ok(new { code = ResponseCode.SystemError, message = ls.Get(Modules.Core, Screen.Message, MessageKey.E_002) });
        }

        /// <summary>
        /// Delete Employee by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var count = await _employeeServices.Delete(id);

            if (count >= 1)
                return Ok(new { code = ResponseCode.Success, message = ls.Get(Modules.Core, Screen.Message, MessageKey.I_003) });
            else
                return Ok(new { code = ResponseCode.SystemError, message = ls.Get(Modules.Core, Screen.Message, MessageKey.E_003) });
        }

        /// <summary>
        /// Get list team leaders
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("group")]
        public async Task<IActionResult> GetGroups()
        {
            var data = await _employeeServices.GetGroups();
            return Ok(data);
        }

        /// <summary>
        /// Export all excel
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("export_all")]
        public async Task<IActionResult> ExportAllExcel()
        {
            var fileName = $"Employees_{DateTimeExtensions.ToDateTimeStampString(DateTime.Now)}.xlsx";

            var fileData = await _employeeServices.ExportAllExcel();

            if (fileData == null)
                return BadRequest(new { code = ResponseCode.NotFound, message = ls.Get(Modules.Core, Screen.Message, MessageKey.E_007) });

            return File(fileData, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);

        }

        /// <summary>
        /// Export template excel
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("export_template")]
        public async Task<IActionResult> ExportTemplateExcel()
        {
            var fileName = $"Employee_Template_{DateTimeExtensions.ToDateTimeStampString(DateTime.Now)}.xlsx";

            var fileData = await _employeeServices.ExportTemplateExcel();

            if (fileData == null)
                return BadRequest(new { code = ResponseCode.NotFound, message = ls.Get(Modules.Core, Screen.Message, MessageKey.E_007) });

            return File(fileData, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);

        }
        /// <summary>
        /// Import Employee From Excels
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("import")]
        public async Task<IActionResult> ImportEmployee(IFormFile file)
        {
            var message = await _employeeServices.Import(file);

            if (message == ls.Get(Modules.Core, Screen.Message, MessageKey.I_001))
                return Ok(new { code = ResponseCode.Success, message  });
            else
                return Ok(new { code = ResponseCode.SystemError, message });
        }

        /// <summary>
        /// Check employee code
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("check_code")]
        public async Task<IActionResult> CheckEmployeeCode(string employee_code)
        {
            var isExisted = await _employeeServices.CheckEmployeeCode(employee_code);
            return Ok(isExisted);
        }
    }
}

