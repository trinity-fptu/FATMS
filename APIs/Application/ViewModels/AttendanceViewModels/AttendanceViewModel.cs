using Domain.Enums.AttendanceEnums;

namespace Application.ViewModels.AttendanceViewModels;

public class AttendanceViewModel
{
    public int AttendanceId { get; set; }
    public int ClassUserId { get; set; }
    public string  AttendanceStatus { get; set; }
    public string Reason { get; set; }
    public string Day { get; set; }
}