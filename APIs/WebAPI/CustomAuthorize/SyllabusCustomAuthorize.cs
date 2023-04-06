using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace WebAPI.CustomAuthorize
{
    public class SyllabusCustomAuthorize : IAuthorizationFilter
    {
        private readonly string[] _permission;
        private readonly IHttpContextAccessor _context;

        public SyllabusCustomAuthorize(string[] permission, IHttpContextAccessor context)
        {
            _permission = permission;
            _context = context;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (_context.HttpContext == null)
            {
                context.Result = new ForbidResult();
                return;
            }

            var identity = _context.HttpContext.User.Identity as ClaimsIdentity;
            if (identity != null)
            {
                var syllabusClaims = identity.Claims;
                var syllabusPermission = syllabusClaims.FirstOrDefault(x => x.Type == "SyllabusPermission")?.Value.ToString();

                if (syllabusPermission == null || syllabusPermission.Equals("AccessDenied"))
                {
                    context.Result = new ForbidResult();
                    return;
                }

                if (!_permission.Contains(syllabusPermission))
                {
                    context.Result = new ForbidResult();
                }
            }
        }
    }
}
