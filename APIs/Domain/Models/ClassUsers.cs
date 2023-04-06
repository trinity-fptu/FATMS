#nullable disable warnings

using Domain.Enums.ClassUserEnums;
using Domain.Models.Users;

namespace Domain.Models
{
    public class ClassUsers
    {
        public int Id { get; set; }
        public ClassRole Role { get; set; }
        public int ClassId { get; set; }
        public int UserId { get; set; }

        public Class? Class { get; set; }
        public User? User { get; set; }
        public ICollection<Attendance> Attendances { get; set; }
    }
}