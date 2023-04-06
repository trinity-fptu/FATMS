using Application.IValidators;
using Application.ViewModels.AttendanceViewModels;
using FluentValidation;

namespace Infracstructures.Validators
{
    public class AttendanceValidator : IAttendanceValidator
    {
        private readonly IValidator<TakeAttendanceModel> _takeAttendanceValidator;

        public AttendanceValidator(IValidator<TakeAttendanceModel> attendanceAddValidator)
        {
            _takeAttendanceValidator = attendanceAddValidator;
        }
        public IValidator<TakeAttendanceModel> TakeAttendanceModel => _takeAttendanceValidator;
    }
}
