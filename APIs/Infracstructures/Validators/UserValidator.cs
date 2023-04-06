using Application.IValidators;
using Application.ViewModels.UserViewModels;
using FluentValidation;

namespace Infracstructures.Validators
{
    public class UserValidator : IUserValidator
    {
        private readonly IValidator<UserCreateModel> _userCreateValidator;
        private readonly IValidator<UserUpdateModel> _userUpdateValidator;

        public UserValidator(IValidator<UserUpdateModel> userUpdateValidator, IValidator<UserCreateModel> userCreateValidator)
        {
            _userCreateValidator = userCreateValidator;
            _userUpdateValidator = userUpdateValidator;
        }

        public IValidator<UserCreateModel> UserCreateValidator => _userCreateValidator;
        public IValidator<UserUpdateModel> UserUpdateValidator => _userUpdateValidator;


    }
}
