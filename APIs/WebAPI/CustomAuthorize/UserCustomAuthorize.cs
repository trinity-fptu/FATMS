using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace WebAPI.CustomAuthorize
{
    public class UserCustomAuthorize : IAuthorizationFilter
    {
        private readonly string[] _permission;
        private readonly IHttpContextAccessor _context;

        public UserCustomAuthorize(IHttpContextAccessor context, params string[] permission)
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
                var userClaims = identity.Claims;
                var userPermission = userClaims.FirstOrDefault(x => x.Type == "UserPermission")?.Value.ToString();

                if (userPermission == null || userPermission.Equals("AccessDenied"))
                {
                    context.Result = new ForbidResult();
                    return;
                }

                if (!_permission.Contains(userPermission))
                {
                    context.Result = new ForbidResult();
                }
            }
        }
    }
}