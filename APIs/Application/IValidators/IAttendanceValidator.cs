using Application.ViewModels.AttendanceViewModels;
using FluentValidation;

namespace Application.IValidators
{
    public interface IAttendanceValidator
    {
        IValidator<TakeAttendanceModel> TakeAttendanceModel { get; }
    }
}
