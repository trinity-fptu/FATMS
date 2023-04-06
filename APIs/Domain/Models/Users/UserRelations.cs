#nullable disable warnings

using Domain.Models.Syllabuses;

namespace Domain.Models.Users
{
    public partial class User
    {
        public Role Role { get; set; }
        public ICollection<TMS> TimeMngSystem { get; set; }
        public ICollection<TrainingProgram> ModifyTrainingProgram { get; set; }
        public ICollection<TrainingProgram> CreatedTrainingProgram { get; set; }
        public ICollection<Syllabus> CreatedSyllabus { get; set; }
        public ICollection<Syllabus> ModifiedSyllabus { get; set; }
        public ICollection<Class> ApprovedClass { get; set; }
        public ICollection<Class> CreatedClass { get; set; }
        public ICollection<FeedbackForm> FeedbackTrainee { get; set; }
        public ICollection<FeedbackForm> FeedbackTrainer { get; set; }
        public ICollection<GradeReport> GradeReports { get; set; }
        public ICollection<ClassUsers> ClassUsers { get; set; }
        public ICollection<AuditPlan> CreatedAuditPlans { get; set; }
        public ICollection<AuditDetail> AuditedAuditDetails { get; set; }
        public ICollection<AuditDetail> TakenAuditDetails { get; set; }
        public ICollection<TMS> TimeMngSystemList { get; set; }
        public ICollection<TrainingMaterial> TrainingMaterials { get; set; }
        public ICollection<ClassUnitDetail> ClassUnitDetails { get; set; }
        public ICollection<QuizBank> QuizBanks { get; set; }
        public ICollection<QuizRecord> QuizRecords { get; set; }
    }
}