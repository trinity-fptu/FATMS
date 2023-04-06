using Domain.Enums.LectureEnums;
using Domain.Models;

namespace Application.ViewModels.SyllabusViewModels;

public class UpdateLectureInSyllabusViewModel
{
    public int SyllabusId { get; set; }
    public int LectureId { get; set; }
    public string Name { get; set; }
    public int Duration { get; set; }
    public LectureLessonType LessonType { get; set; }
    public LectureDeliveryType DeliveryType { get; set; }
    public int? OutputStandardId { get; set; }
    public OutputStandard? OutputStandard { get; set; }
    
    public string LastModifiedOn { get; set; }
    public string LastModifiedBy { get; set; }
}