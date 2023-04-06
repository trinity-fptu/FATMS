namespace Application.ViewModels.TrainingMaterialViewModels
{
    public class TrainingMaterialDetailModel
    {
        public int MaterialId { get; set; }
        public int LectureId { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedOn { get; set; }
    }
}