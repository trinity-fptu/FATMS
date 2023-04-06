using Application.ViewModels.TrainingMaterialViewModels;

namespace Application.ViewModels.AddLectureViewModels
{
    public class LectureDetailModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string OutputStandard { get; set; }
        public int Duration { get; set; }
        public string LessonType { get; set; }
        public string DeliveryType { get; set; }
        public List<TrainingMaterialDetailModel> TrainingMaterials { get; set; }
    }
}