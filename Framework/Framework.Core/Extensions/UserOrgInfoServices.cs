using Framework.Core.Abstractions;
using Microsoft.AspNetCore.Http;

namespace Framework.Core.Extenstions
{
    public class UserOrgInfoServices : IUserOrgInfoServices
    {
        public Guid? _orgId { get; set; }
        public Guid? _groupId { get; set; }
        public int? _accYear { get; set; }
        public UserOrgInfoServices(IHttpContextAccessor httpContextAccessor)
        {
            string orgStr = httpContextAccessor.HttpContext?.Request?.Headers["x-org-id"].FirstOrDefault();
            string groupStr = httpContextAccessor.HttpContext?.Request?.Headers["x-group-id"].FirstOrDefault();
            string accYear = httpContextAccessor.HttpContext?.Request?.Headers["x-acc-year"].FirstOrDefault();
            _orgId = !string.IsNullOrEmpty(orgStr) ? Guid.Parse(orgStr) : Guid.Empty;
            _groupId = !string.IsNullOrEmpty(groupStr) ? Guid.Parse(groupStr) : Guid.Empty;
            _accYear = !string.IsNullOrEmpty(accYear) ? int.Parse(accYear) : null;
        }
    }
}
