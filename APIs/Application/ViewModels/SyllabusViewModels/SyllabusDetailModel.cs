using Application.ViewModels.UnitViewModels;
using Newtonsoft.Json;

namespace Application.ViewModels.SyllabusViewModels
{
    public class SyllabusDetailModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public float Version { get; set; }
        public SyllabusDuration Duration { get; set; } = new SyllabusDuration();
        public string CreatedBy { get; set; }
        public string CreatedOn { get; set; }
        public string LastModifiedBy { get; set; }
        public string LastModifiedOn { get; set; }
        public bool isActive { get; set; }
        public string TechnicalRequirements { get; set; }
        public string CourseObjectives { get; set; }
        public string Level { get; set; } = "AllLevels";
        public int AttendeeNumber { get; set; }
        public string TrainingDeliveryPrinciple { get; set; }
        public float QuizCriteria { get; set; }
        public float AssignmentCriteria { get; set; }
        public float FinalCriteria { get; set; }
        public float PassingGPA { get; set; }
        public List<Content> Days { get; set; } = new List<Content>();
        [JsonIgnore]
        public ICollection<UnitDetailModel> Units { get; set; }
        public SyllabusTimeAllocation TimeAllocation { get; set; }
    }
    public class SyllabusDuration
    {
        public int Days { get; set;}
        public Decimal Hours { get; set;}
    }
    public class Content
    {
        public int Day { get; set; }
        public ICollection<UnitDetailModel> Units { get; set; }
    }
    public class SyllabusTimeAllocation

    {
        public float AssignmentLab { get; set; }
        public float ConceptLecture { get; set; }
        public float GuideReview { get; set; }
        public float TestQuiz { get; set; }
        public float Exam { get; set; }
    }
    
}