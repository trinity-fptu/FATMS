using Application.ViewModels.RoleViewModels;
using FluentValidation.Results;

namespace Application.Interfaces
{
    public interface IRoleService
    {
        Task<ValidationResult> UpdateRoleValidationAsync(RoleUpdateModel updateRole);
        Task<ValidationResult> CreateRoleValidatAsync(RoleCreateModel createRole);
        Task<List<RoleViewModel>> GetListRoleAsync();
        List<string> GetListPermission();
        Task<bool> EditPermissionAsync(List<RoleUpdateModel> roles);
        Task<bool> AddRoleAsync(RoleCreateModel role);
    }
}
