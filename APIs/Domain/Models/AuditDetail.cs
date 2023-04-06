
using Domain.Models.Users;

namespace Domain.Models
{
    public class AuditDetail
    {
        public int Id { get; set; }
        public string? Feedback { get; set; }
        public bool? Status { get; set; }
        public int PlanId { get; set; }
        public int? AuditorId { get; set; }
        public int? TraineeId { get; set; }
        public AuditPlan AuditPlan { get; set; }
        public User? Auditor { get; set; }
        public User? Trainee { get; set; }
        public ICollection<AuditResult> Results { get; set; }
    }
}
