using Domain.Enums.SyllabusEnums;

namespace Application.ViewModels.SyllabusViewModels;

public class BaseSyllabusViewModel
{
    public string Code { get; set; }
    public float Version { get; set; }
    public string Name { get; set; }
    public SyllabusLevel Level { get; set; }
    public int AttendeeNumber { get; set; }
    public string CourseObjectives { get; set; }
    public string TechnicalRequirements { get; set; }
    public string TrainingDeliveryPrinciple { get; set; }
    public float QuizCriteria { get; set; }
    public float AssignmentCriteria { get; set; }
    public float FinalCriteria { get; set; }
    public float FinalTheoryCriteria { get; set; }
    public float FinalPracticalCriteria { get; set; }
    public float PassingGPA { get; set; }
    public bool isActive { get; set; }
}