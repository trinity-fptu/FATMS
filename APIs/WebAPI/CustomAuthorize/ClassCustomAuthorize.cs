using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace WebAPI.CustomAuthorize
{
    public class ClassCustomAuthorize : IAuthorizationFilter
    {
        private readonly string[] _permission;
        private readonly IHttpContextAccessor _context;

        public ClassCustomAuthorize(IHttpContextAccessor context, params string[] permission)
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
                var classClaims = identity.Claims;
                var classPermission = classClaims.FirstOrDefault(x => x.Type == "ClassPermission")?.Value.ToString();

                if (classPermission == null || classPermission.Equals("AccessDenied"))
                {
                    context.Result = new ForbidResult();
                    return;
                }

                if (!_permission.Contains(classPermission))
                {
                    context.Result = new ForbidResult();
                }
            }
        }
    }
}
