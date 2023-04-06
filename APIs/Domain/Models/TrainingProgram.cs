#nullable disable warnings

using Domain.Models.Base;
using Domain.Models.Syllabuses;
using Domain.Models.Users;

namespace Domain.Models
{
    public class TrainingProgram : BaseModel
    {
        public string Name { get; set; }
        public int Duration { get; set; }
        public DateTime LastModify { get; set; }
        public bool IsActive { get; set; } = true;
        public int DaysDuration { get; set; }
        public int TimeDuration { get; set; }

        public int? LastModifyBy { get; set; }
        public User? ModifiedAdmin { get; set; }
        public User? CreatedAdmin { get; set; }
        public ICollection<Class> Classes { get; set; }
        public ICollection<Syllabus> Syllabuses { get; set; }
    }
}