using Application.Interfaces;
using Application.ResponseModels;
using Application.ViewModels.RoleViewModels;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using WebAPI.CustomAuthorize;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class RoleController : Controller
    {
        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        // Controller Action for get List of Role.
        // path: api/Role/GetListAsync
        #region GetListAsync
        [UserCustomAuthorize(new string[] { "FullAccess", "View" })]
        [HttpGet]
        public async Task<IActionResult> GetListAsync()
        {
            var roles = await _roleService.GetListRoleAsync();
            if (roles.Count == 0)
            {
                return NotFound(new BaseFailedResponseModel
                {
                    Status = StatusCodes.Status404NotFound,
                    Message = "This list is empty."
                });
            }
            return Ok(new BaseResponseModel
            {
                Status = StatusCodes.Status200OK,
                Message = "Succeed.",
                Result = new
                {
                    Roles = roles
                }
            });

        }
        #endregion

        // Controller Action for get List of Permisison.
        // path: api/Role/GetListPermission
        #region GetListPermission
        [HttpGet]
        public IActionResult GetListPermission()
        {
            return Ok(new BaseResponseModel
            {
                Status = StatusCodes.Status200OK,
                Message = "Succeed.",
                Result = new
                {
                    Permissions = _roleService.GetListPermission()
                }
            });
        }
        #endregion

        // Controller Action to update User Permission.
        // path: api/Role/EditPermission
        #region EditPermisisonAsync
        [HttpPatch]
        public async Task<IActionResult> EditPermisisonAsync(List<RoleUpdateModel> roles)
        {
            bool validateIsValid = true;
            foreach (var role in roles)
            {
                var validateResult = await _roleService.UpdateRoleValidationAsync(role);
                if (!validateResult.IsValid)
                {
                    validateIsValid = false;
                    validateResult.AddToModelState(ModelState);
                }
            }

            if (validateIsValid)
            {
                if (await _roleService.EditPermissionAsync(roles))
                {
                    return Ok(new BaseResponseModel
                    {
                        Status = StatusCodes.Status200OK,
                        Message = "Succeed.",
                        Result = new
                        {
                            Roles = roles
                        }
                    });
                }
            }

            return BadRequest(new BaseFailedResponseModel
            {
                Status = StatusCodes.Status400BadRequest,
                Message = "Validate Role Failed.",
                Errors = ModelState
            });
        }
        #endregion

        // Controller Action to add new Role.
        // path: api/Role/AddRole
        #region AddRoleAsync
        [HttpPost]
        public async Task<IActionResult> AddRoleAsync(RoleCreateModel role)
        {
            var validateResult = await _roleService.CreateRoleValidatAsync(role);
            if (validateResult.IsValid)
            {
                if (await _roleService.AddRoleAsync(role))
                {
                    return Created("api/Role/GetListAsync", new BaseResponseModel
                    {
                        Status = StatusCodes.Status201Created,
                        Message = "Created Succeed.",
                        Result = new
                        {
                            Roles = role
                        }
                    });
                }
            }

            validateResult.AddToModelState(ModelState);

            return BadRequest(new BaseFailedResponseModel
            {
                Status = StatusCodes.Status400BadRequest,
                Message = "Validate Role Failed.",
                Errors = ModelState
            });
        }
        #endregion
    }
}