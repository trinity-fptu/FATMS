using Application.ViewModels.RoleViewModels;
using FluentValidation;

namespace Application.IValidators
{
    public interface IRoleValidator
    {
       IValidator<RoleUpdateModel> RoleUpdateModel { get; }
       IValidator<RoleCreateModel> RoleCreateModel { get; }
    }
}
