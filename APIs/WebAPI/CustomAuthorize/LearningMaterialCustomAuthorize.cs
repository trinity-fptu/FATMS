using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace WebAPI.CustomAuthorize
{
    public class LearningMaterialCustomAuthorize : IAuthorizationFilter
    {
        private readonly string[] _permission;
        private readonly IHttpContextAccessor _context;

        public LearningMaterialCustomAuthorize(IHttpContextAccessor context, params string[] permission)
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
                var learningMaterialClaims = identity.Claims;
                var learningMaterialPermission = learningMaterialClaims.FirstOrDefault(x => x.Type == "LearningMaterialPermission")?.Value.ToString();

                if (learningMaterialPermission == null || learningMaterialPermission.Equals("AccessDenied"))
                {
                    context.Result = new ForbidResult();
                    return;
                }

                if (!_permission.Contains(learningMaterialPermission))
                {
                    context.Result = new ForbidResult();
                }
            }
        }
    }
}
