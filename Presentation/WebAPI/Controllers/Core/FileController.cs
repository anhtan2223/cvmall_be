using Application.Common.Abstractions;
using Application.Common.Extensions;
using Framework.Core.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using WebAPI.Filters;
using Framework.Core.Collections;
using Application.Core.Interfaces.Core;

namespace WebAPI.Controllers.Core
{
    [BAuthorize]
    [ApiController]
    [Route("api/[controller]")]
    public class FileController : ControllerBase
    {
        private readonly ILocalizeServices ls;
        private readonly IWebHostEnvironment env;
        private readonly ILogServices logService;
        private readonly string DIR_UPLOAD = "ASPNETCORE_DIR_UPLOAD";
        public FileController(
            ILogServices _logService,
            ILocalizeServices _ls,
            IWebHostEnvironment _env
            )
        {
            ls = _ls;
            env = _env;
            logService = _logService;
        }
        private class ImageExtension
        {
            public string Type { get; set; }
            public string[] Extensions { get; set; }
        }
        private List<ImageExtension> Extensions = new()
        {
           new ImageExtension
           {
                Type = "Images",
                Extensions = new string[3]
                {
                    ".jpg",".jpeg",".png"
                }
           },
           new ImageExtension
            {
                Type="Documents",
                Extensions=new string[7]
                {
                    ".doc",".docx",".xlsx",".xls",".pdf",".pptx",".ppt"
                }
            },
            new ImageExtension
            {
                Type="Zips",
                Extensions=new string[2]
                {
                    ".rar",".zip"
                }
            },
        };
        [HttpPost]
        [Route("{path}")]
        public async Task<IActionResult> Index([FromRoute] string path, [FromForm] IFormFile file)
        {
            if (string.IsNullOrWhiteSpace(path)) return NotFound();

            if (file.Length < 0) return Ok(new BaseResponse(ResponseCode.Invalid, ls.Get(Modules.Core, Screen.Message, MessageKey.E_FILE_VALID)));

            var fileExtension = Path.GetExtension(file.FileName).ToLower();

            var ImagesExtensions = Extensions.FirstOrDefault(x => x.Extensions.Contains(fileExtension));

            if (ImagesExtensions == default) return Ok(new BaseResponse(ResponseCode.Invalid, ls.Get(Modules.Core, Screen.Message, MessageKey.E_FILE_FORMAT)));

            var fileName = Path.GetFileName(file.FileName) + DateTime.Now.ToString("yyyyMMddHHmmssffff");

            var fileNameMd5 = string.Empty;
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(fileName);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                fileNameMd5 = Convert.ToHexString(hashBytes) + fileExtension;
            }

            var pathDataStore = Path.Combine(Environment.GetEnvironmentVariable(DIR_UPLOAD), ImagesExtensions.Type, path);

            if (!Directory.Exists(pathDataStore))
                Directory.CreateDirectory(pathDataStore);

            using (FileStream fs = new FileStream(Path.Combine(pathDataStore, fileNameMd5), FileMode.CreateNew))
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    await file.CopyToAsync(ms);
                    await fs.WriteAsync(ms.ToArray());
                }
            }

            return Ok(new BaseResponse<string>(path + "/" + fileNameMd5, ResponseCode.Success));
        }

        [HttpGet]
        [Route("{path}/{fileName}")]
        public async Task<IActionResult> GetFile([FromRoute] string path, [FromRoute] string fileName)
        {
            if (string.IsNullOrWhiteSpace(path)) return NotFound();

            var fileExtension = Path.GetExtension(fileName).ToLower();

            if (string.IsNullOrWhiteSpace(fileExtension))
                return Ok(new BaseResponse(ResponseCode.Invalid, ls.Get(Modules.Core, Screen.Message, MessageKey.E_FILE_FORMAT)));

            var ImagesExtensions = Extensions.FirstOrDefault(x => x.Extensions.Contains(fileExtension));

            if (ImagesExtensions == default) return Ok(new BaseResponse(ResponseCode.Invalid, ls.Get(Modules.Core, Screen.Message, MessageKey.E_FILE_FORMAT)));

            var pathDataStore = Path.Combine(Environment.GetEnvironmentVariable(DIR_UPLOAD), ImagesExtensions.Type, path);

            var pathFile = Path.Combine(pathDataStore, fileName);
            var fileInfo = new FileInfo(pathFile);
            
            if (!fileInfo.Exists) return Ok(new BaseResponse(ResponseCode.Invalid, ls.Get(Modules.Core, Screen.Message, MessageKey.E_FILE_NOT_FOUND)));

            using (FileStream fs = new FileStream(pathFile, FileMode.Open))
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    await fs.CopyToAsync(ms);

                    var provider = new FileExtensionContentTypeProvider();
                    string contentType;
                    if (!provider.TryGetContentType(fileName, out contentType))
                    {
                        contentType = "application/octet-stream";
                    }

                    return File(ms.ToArray(), contentType, fileName);
                }
            }
        }
    }
}
