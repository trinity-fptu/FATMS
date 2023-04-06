using Application.IValidators;
using Application.ViewModels.RoleViewModels;
using FluentValidation;

namespace Infracstructures.Validators
{
    public class RoleValidator : IRoleValidator
    {
        private readonly IValidator<RoleUpdateModel> _roleUpdateValidator;
        private readonly IValidator<RoleCreateModel> _roleCreateValidator;

        public RoleValidator(IValidator<RoleUpdateModel> roleUpdateValidator, IValidator<RoleCreateModel> roleCreateValidator)
        {
            _roleUpdateValidator = roleUpdateValidator;
            _roleCreateValidator = roleCreateValidator;
        }

        public IValidator<RoleUpdateModel> RoleUpdateModel => _roleUpdateValidator;
        public IValidator<RoleCreateModel> RoleCreateModel => _roleCreateValidator;
    }
}
