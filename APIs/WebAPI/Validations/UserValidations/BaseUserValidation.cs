using Application.ViewModels.UserViewModels;
using Domain.Enums.UserEnums;
using FluentValidation;

namespace WebAPI.Validations.UserValidations
{
    /// <summary>
    /// Base validator for user
    /// </summary>
    public class BaseUserValidation<T> : AbstractValidator<T> where T : BaseUserViewModel
    {
        public BaseUserValidation()
        {
            //Full Name
            RuleFor(x => x.FullName).NotNull().WithMessage("User name must not be null.")
                .NotEmpty().WithMessage("User name cannot be empty.");

            //Email
            RuleFor(x => x.Email).NotNull().WithMessage("User email must not be null.")
                .NotEmpty().WithMessage("User email cannot be empty.")
                .EmailAddress().WithMessage("Email address is invalid.");

            //Phone
            RuleFor(x => x.Phone).NotNull().WithMessage("User phone must not be null")
                .NotEmpty().WithMessage("User phone cannot be empty.")
                .MaximumLength(15)
                .Matches("^\\+?[0-9][0-9]{7,14}$");

            //Date of Birth
            RuleFor(x => x.DateOfBirth).NotNull().WithMessage("Date of birth must not be null.")
                .LessThan(x => DateTime.Now.AddYears(-15)).WithMessage("User must be 16 or older.")
                .GreaterThan(x => new DateTime(1899, 12, 31, 23, 59, 59, 999)).WithMessage("Date of birth year must be after 1900s.");

            //Role
            RuleFor(x => x.Role).NotNull().WithMessage("User role must not be null.")
                .NotEmpty().WithMessage("User role must not be empty.");

            //Level
            RuleFor(x => x.Level).NotNull().WithMessage("User level must not be null.")
                .IsEnumName(typeof(UserLevel)).WithMessage("User level is invalid.");

            //Status
            RuleFor(x => x.Status).NotNull().WithMessage("User status must not be null.")
                .IsEnumName(typeof(UserStatus)).WithMessage("User status is invalid.");
        }
    }
}