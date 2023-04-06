using Application.ViewModels.RoleViewModels;
using Domain.Enums.RoleEnums;
using FluentValidation;

namespace WebAPI.Validations.RoleValidations
{
    public class UpdateRoleValidation : BaseRoleValidation<RoleUpdateModel>
    {
        public UpdateRoleValidation() { }
    }
}
