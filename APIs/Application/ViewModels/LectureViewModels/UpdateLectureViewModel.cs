using Domain.Enums.LectureEnums;

namespace Application.ViewModels.LectureViewModels;

public class UpdateLectureViewModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Duration { get; set; }
    public LectureLessonType LessonType { get; set; }
    public LectureDeliveryType DeliveryType { get; set; }
    public int OutputStandardId { get; set; }
}