#nullable disable warnings

using Domain.Models.Users;

namespace Domain.Models.Syllabuses
{
    public partial class Syllabus
    {
        public User? CreatedAdmin { get; set; }
        public User? ModifiedAdmin { get; set; }
        public ICollection<Unit> Units { get; set; }
        public ICollection<TrainingProgram> TrainingPrograms { get; set; }
        public ICollection<AuditPlan> AuditPlans { get; set; }
        public ICollection<Class> Classes { get; set; }
        
    }
}