using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace WebAPI.CustomAuthorize
{
    public class TrainingProgramCustomAuthorize : IAuthorizationFilter
    {
        private readonly string[] _permission;
        private readonly IHttpContextAccessor _context;

        public TrainingProgramCustomAuthorize(IHttpContextAccessor context, params string[] permission)
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
                var trainingProgramClaims = identity.Claims;
                var trainingProgramPermission = trainingProgramClaims.FirstOrDefault(x => x.Type == "TrainingProgramPermission")?.Value.ToString();

                if (trainingProgramPermission == null || trainingProgramPermission.Equals("AccessDenied"))
                {
                    context.Result = new ForbidResult();
                    return;
                }

                if (!_permission.Contains(trainingProgramPermission))
                {
                    context.Result = new ForbidResult();
                }
            }
        }
    }
}
