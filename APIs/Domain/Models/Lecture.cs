#nullable disable warnings

using Domain.Enums.LectureEnums;
                                                   
namespace Domain.Models
{
    public class Lecture
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Duration { get; set; }
        public LectureLessonType LessonType { get; set; }
        public LectureDeliveryType DeliveryType { get; set; }

        public int? UnitId { get; set; }
        public Unit? Unit { get; set; }
        public int? OutputStandardId { get; set; }
        public OutputStandard? OutputStandard { get; set; }
        public ICollection<GradeReport> GradeReports { get; set; }
        public ICollection<TrainingMaterial> TrainingMaterials { get; set; }
    }
}