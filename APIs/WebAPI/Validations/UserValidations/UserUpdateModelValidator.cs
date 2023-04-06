using Application.ViewModels.UserViewModels;
using FluentValidation;
using System.Globalization;
using Domain.Enums.UserEnums;
using System.Text.RegularExpressions;

namespace WebAPI.Validations.UserValidations
{
    public class UserUpdateModelValidator : AbstractValidator<UserUpdateModel>
    {
        private bool BeAValidDate(string value)
        {
            var result = DateTime.TryParseExact(value, "dd/MM/yyyy", CultureInfo.InvariantCulture,
                           DateTimeStyles.None, out DateTime date);
            return result;
        }

        private bool OlderThan16(string value)
        {
            var result = DateTime.TryParseExact(value, "dd/MM/yyyy", CultureInfo.InvariantCulture,
                           DateTimeStyles.None, out DateTime date);
            return (date.Year < 2007);
        }

        private bool BeAValidBool(string value)
        {
            return bool.TryParse(value, out bool result);
        }

        public UserUpdateModelValidator()
        {
            RuleFor(p => p.Phone).NotNull().WithMessage("Phone number is required.")
                .MinimumLength(10).WithMessage("Phone number must not be less than 10 characters.")
                .MaximumLength(10).WithMessage("Phone number must not exceed 10 characters.")
                .Matches(new Regex(@"^(84|0[3|5|7|8|9])+([0-9]{8})$")).WithMessage("Phone number is invalid");

            RuleFor(x => x.DateOfBirth).NotNull().WithMessage("User date of birth is required.")
                .Must(BeAValidDate).WithMessage("Invalid date/time.")
                .Must(OlderThan16).WithMessage("User must be older than 16");

            RuleFor(x => x.IsMale).NotNull().WithMessage("User gender is required.")
                .Must(BeAValidBool).WithMessage("Invalid boolean.");

            RuleFor(x => x.RoleId).NotNull().WithMessage("User role ID is required.")
                .GreaterThanOrEqualTo(1).WithMessage("User role ID must be between 1 and 5.")
                .LessThanOrEqualTo(5).WithMessage("User role ID must be between 1 and 5.");

            RuleFor(x => x.Level).NotNull().WithMessage("User level is required.")
                .IsEnumName(typeof(UserLevel)).WithMessage("User level is invalid.");

            RuleFor(x => x.Status).NotNull().WithMessage("User status is required.")
                .IsEnumName(typeof(UserStatus)).WithMessage("User status is invalid.");

        }
    }
}
