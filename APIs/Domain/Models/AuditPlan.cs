#nullable disable warnings

using Domain.Enums.AuditPlans;
using Domain.Models.Syllabuses;
using Domain.Models.Users;

namespace Domain.Models
{
    public class AuditPlan
    {
        public int Id { get; set; }
        public DateTime AuditDate { get; set; }
        public int? PlannedBy { get; set; }
        public string Location { get; set; }
        public int SyllabusId { get; set; }
        public int ClassId { get; set; }
        public Syllabus Syllabus { get; set; }
        public Class Class { get; set; }
        public User? CreatedUser { get; set; }
        public ICollection<AuditDetail> AuditDetails { get; set; }
    }
}