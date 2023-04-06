using Application.ViewModels.RoleViewModels;
using FluentValidation;

namespace WebAPI.Validations.RoleValidations
{
    public class CreateRoleValidation : BaseRoleValidation<RoleCreateModel>
    {
        public CreateRoleValidation()
        {
            //Name
            RuleFor(x => x.Name).NotNull().WithMessage("Role Name cannot be null.")
                .NotEmpty().WithMessage("Role Name cannot be empty.");
        }
    }
}
