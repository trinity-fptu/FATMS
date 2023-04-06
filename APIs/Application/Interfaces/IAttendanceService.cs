using Application.ViewModels.AttendanceViewModels;
using FluentValidation.Results;

namespace Application.Interfaces
{
    public interface IAttendanceService
    {
        //your action is here
        Task<bool> EditAttendanceAsync(EditAttendanceViewModel editAttendanceViewModel,int id);
        Task<List<AttendanceViewModel>> GetAllAttendanceAsync(int id);
        Task<AttendanceViewModel> TakeAttendance(TakeAttendanceModel view);
        Task<ValidationResult> ValidateTakeAttendanceAsync(TakeAttendanceModel attendance);
    }
}