using Application.ViewModels.UserViewModels;
using FluentValidation;

namespace Application.IValidators
{
    public interface IUserValidator
    {
        IValidator<UserCreateModel> UserCreateValidator { get; }
        IValidator<UserUpdateModel> UserUpdateValidator { get; }

    }
}

