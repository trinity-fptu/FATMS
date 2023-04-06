using FluentValidation;
using Application.ViewModels.AttendanceViewModels;

namespace WebAPI.Validations.AttendanceValidations
{
    public class TakeAttendanceValidation : AbstractValidator<TakeAttendanceModel>
    {
        public TakeAttendanceValidation()
        {
            //ClassUserID
            RuleFor(x => x.ClassUserID).NotNull().WithMessage("Class User ID must not be null.")
                .NotEmpty().WithMessage("Class User ID must not be empty.");

            RuleFor(x => x.ClassUserID)
           .GreaterThan(0)
           .WithMessage("Class User ID must greater than 0");

            //Status
            RuleFor(x => x.AttendanceStatus).NotNull().WithMessage("Attendance status must not be null.")
                .NotEmpty().WithMessage("Attendance status must not be empty.")
                .IsInEnum().WithMessage("Attendance status is invalid.");
        }
    }
}
