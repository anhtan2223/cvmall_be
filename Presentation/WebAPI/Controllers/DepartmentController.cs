using Application.Common.Abstractions;
using Application.Common.Extensions;
using Application.Core.Contracts.Department;
using Application.Core.Interfaces.Core;
using Framework.Core.Helpers;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Filters;

namespace WebAPI.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [BAuthorize]
    [ApiController]
    [Route("api/department/")]
    public class DepartmentController : ControllerBase
    {
        private readonly IDepartmentServices departmentServices;
        private ILocalizeServices ls { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DepartmentController(IDepartmentServices _departmentServices, ILocalizeServices _ls) : base()
        {
            departmentServices = _departmentServices;
            ls = _ls;
        }

        /// <summary>
        /// 
        /// </summary>
        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetAll()
        {
            var response = await departmentServices.GetAll();
            return Ok(response);
        }

        /// <summary>
        /// 
        /// </summary>
        [HttpPost]
        [Route("")]
        public async Task<IActionResult> Create([FromBody] DepartmentRequest request)
        {
            int count = await departmentServices.Create(request);

            if (count >= 1)
                return Ok(new { code = ResponseCode.Success, message = ls.Get(Modules.Core, Screen.Message, MessageKey.I_001) });
            else
                return Ok(new { code = ResponseCode.SystemError, message = ls.Get(Modules.Core, Screen.Message, MessageKey.E_001) });
        }

        /// <summary>
        /// 
        /// </summary>
        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] DepartmentRequest request)
        {
            int count = await departmentServices.Update(id, request);

            if (count >= 1)
                return Ok(new { code = ResponseCode.Success, message = ls.Get(Modules.Core, Screen.Message, MessageKey.I_001) });
            else
                return Ok(new { code = ResponseCode.SystemError, message = ls.Get(Modules.Core, Screen.Message, MessageKey.E_001) });
        }

        /// <summary>
        /// 
        /// </summary>
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Destroy(Guid id)
        {
            int count = await departmentServices.Delete(id);

            if (count >= 1)
                return Ok(new { code = ResponseCode.Success, message = ls.Get(Modules.Core, Screen.Message, MessageKey.I_001) });
            else
                return Ok(new { code = ResponseCode.SystemError, message = ls.Get(Modules.Core, Screen.Message, MessageKey.E_001) });
        }
    }
}
