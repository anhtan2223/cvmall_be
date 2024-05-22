using Application.Common.Abstractions;
using Application.Common.Extensions;
using Framework.Core.Helpers;
using Framework.Core.Helpers.Cache;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers.Core
{
    /// <summary>
    /// 
    /// </summary>
    [ApiController]
    [Route("api/cache")]
    public class CacheController : ControllerBase
    {
        private readonly ICachingService cachingService;
        private readonly ILocalizeServices ls;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_cachingService"></param>
        /// <param name="_ls"></param>
        public CacheController(ICachingService _cachingService, ILocalizeServices _ls)
        {
            cachingService = _cachingService;
            ls = _ls;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await Task.FromResult<string?>(null));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpDelete]
        [Route("clear-all")]
        public async Task<IActionResult> ClearAll()
        {
            await cachingService.ClearAllAsync();
            return Ok(new { code = ResponseCode.Success, message = ls.Get(Modules.Core, "Cache", "ClearAllSuccess") });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("clear-key")]
        public async Task<IActionResult> ClearByKey([FromBody] string[] keys)
        {
            await cachingService.ClearAsync(keys);
            return Ok(new { code = ResponseCode.Success, message = ls.Get(Modules.Core, "Cache", "ClearByKeySuccess") });
        }
    }
}
