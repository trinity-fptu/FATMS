#nullable disable warnings

using Domain.Enums.AttendanceEnums;

namespace Domain.Models
{
    public class Attendance
    {
        public int AttendanceId { get; set; }
        public int ClassUserId { get; set; }
        public DateTime Day { get; set; }
        public AttendanceStatus? AttendanceStatus { get; set; }
        public string Reason { get; set; }

        public ClassUsers? ClassUser { get; set; }
    }
}