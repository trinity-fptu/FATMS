#nullable disable warnings

using Domain.Enums.ClassEnums;
using Domain.Models.Base;
using Domain.Models.Syllabuses;
using Domain.Models.Users;
using System.Collections;

namespace Domain.Models
{
    public class Class : BaseModel
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public ClassLocation? Location { get; set; }
        public ClassAttendeeType? AttendeeType { get; set; }
        public ClassFSU? FSU { get; set; }
        public ClassTime ClassTime { get; set; }
        public DateTime StartedOn { get; set; }
        public DateTime FinishedOn { get; set; }
        public string LectureStartedTime { get; set; }
        public string LectureFinishedTime { get; set; }
        public int DaysDuration { get; set; }
        public int TimeDuration { get; set; }
        public ClassStatus Status { get; set; }
        public DateTime ApprovedOn { get; set; }
        public int? ApprovedBy { get; set; }
        public User? ApprovedAdmin { get; set; }
        public User? CreatedAdmin { get; set; }
        public int? TrainingProgramId { get; set; }
        public TrainingProgram? TrainingProgram { get; set; }
        public ICollection<ClassUsers> ClassUsers { get; set; }
        public ICollection<ClassUnitDetail> ClassUnitDetails { get; set; }
        public ICollection<Syllabus> Syllabuses { get; set; }
        public ICollection<AuditPlan> AuditPlans { get; set; }
    }
}