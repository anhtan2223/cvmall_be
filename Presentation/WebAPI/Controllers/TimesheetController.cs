﻿using Microsoft.AspNetCore.Mvc;
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
    [Route("api/timesheet")]
    public class TimesheetController : ControllerBase
    {
        private ITimesheetServices timesheetServices { get; set; }
        private ILocalizeServices ls { get; set; }

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="_functionServices"></param>
        /// <param name="_ls"></param>
        public TimesheetController(ITimesheetServices _timesheetServices, ILocalizeServices _ls) : base()
        {
            timesheetServices = _timesheetServices;
            ls = _ls;
        }

        /// <summary>
        /// Get list timesheet
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetPaged([FromQuery] TimesheetRequestPaged request)
        {
            var data = await timesheetServices.GetPaged(request);
            return Ok(data);
        }

        // /// <summary>
        // /// Get timesheet by id
        // /// </summary>
        // /// <param name="id"></param>
        // /// <returns></returns>
        // [HttpGet]
        // [Route("{id}")]
        // public async Task<IActionResult> GetById(Guid id)
        // {
        //     var data = await timesheetServices.GetById(id);
        //     if (data != null)
        //         return Ok(data.ToResponse());
        //     else
        //         return Ok(data.ToResponse());
        // }

        // /// <summary>
        // /// Create new timesheet
        // /// </summary>
        // /// <param name="request"></param>
        // /// <returns></returns>
        // [HttpPost]
        // [Route("")]
        // public async Task<IActionResult> Create([FromBody] TimesheetRequest request)
        // {
        //     try
        //     {
        //         int count = await timesheetServices.Create(request);

        //         if (count >= 1)
        //             return Ok(new { code = ResponseCode.Success, message = ls.Get(Modules.Core, Screen.Message, MessageKey.I_001) });
        //         else
        //             return Ok(new { code = ResponseCode.SystemError, message = ls.Get(Modules.Core, Screen.Message, MessageKey.E_001) });
        //     }
        //     catch (Exception ex)
        //     {
        //         return Ok(new { code = ResponseCode.SystemError, message = ex.Message });
        //     }
        // }

        // /// <summary>
        // /// Update timesheet by id
        // /// </summary>
        // /// <param name="id"></param>
        // /// <param name="request"></param>
        // /// <returns></returns>
        // [HttpPut]
        // [Route("{id}")]
        // public async Task<IActionResult> Update(Guid id, [FromBody] TimesheetRequest request)
        // {
        //     try
        //     {
        //         var count = await timesheetServices.Update(id, request);
        //         if (count >= 1)
        //             return Ok(new { code = ResponseCode.Success, message = ls.Get(Modules.Core, Screen.Message, MessageKey.I_002) });
        //         else
        //             return Ok(new { code = ResponseCode.SystemError, message = ls.Get(Modules.Core, Screen.Message, MessageKey.E_002) });
        //     }
        //     catch (Exception ex)
        //     {
        //         return Ok(new { code = ResponseCode.SystemError, message = ex.Message });
        //     }
        // }


        /// <summary>
        /// Update timesheet list
        /// </summary>
        /// <param name="requestList"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("update-multi")]
        public async Task<IActionResult> UpdateMulti([FromBody] List<TimesheetRequest> requestList)
        {
            try
            {
                var count = await timesheetServices.UpdateMulti(requestList);
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

        // /// <summary>
        // /// Delete timesheet by id
        // /// </summary>
        // /// <param name="id"></param>
        // /// <returns></returns>
        // [HttpDelete]
        // [Route("{id}")]
        // public async Task<IActionResult> Delete(Guid id)
        // {
        //     var count = await timesheetServices.Delete(id);

        //     if (count >= 1)
        //         return Ok(new { code = ResponseCode.Success, message = ls.Get(Modules.Core, Screen.Message, MessageKey.I_003) });
        //     else
        //         return Ok(new { code = ResponseCode.SystemError, message = ls.Get(Modules.Core, Screen.Message, MessageKey.E_003) });
        // }


        /// <summary>
        /// Export all excel by month year
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("export-all-excel-by-month-year")]
        public async Task<IActionResult> ExportAllExcel([FromQuery] TimesheetRequestPaged request)
        {
            var month = request.month != 0 ? request.month : DateTime.Now.Month;
            var year = request.year != 0 ? request.year : DateTime.Now.Year;

            var fileName = $"timesheet_{month}_{year}_{DateTimeExtensions.ToDateTimeStampString(DateTime.Now)}.xlsx";

            var fileData = await timesheetServices.ExportAllExcel(request);

            if (fileData == null)
                return BadRequest(new { code = ResponseCode.NotFound, message = ls.Get(Modules.Core, Screen.Message, MessageKey.E_007) });

            return File(fileData, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);

        }
        /// <summary>
        /// Import Late Early Departure From Excels
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("import-late-early")]
        public async Task<IActionResult> Test2(IFormFile file, DateTime month)
        {
            var count = await timesheetServices.ImportLateEarly(file, month);
            if (count >= 1)
                return Ok(new { code = ResponseCode.Success, message = $"Add Success : {count} Element At {month.Month}/{month.Year} "  });
            else
                return Ok(new { code = ResponseCode.SystemError, message = ls.Get(Modules.Core, Screen.Message, MessageKey.E_002) });
        }
    }
}

