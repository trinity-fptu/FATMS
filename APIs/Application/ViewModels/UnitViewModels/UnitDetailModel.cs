using Application.ViewModels.AddLectureViewModels;

namespace Application.ViewModels.UnitViewModels
{
    public class UnitDetailModel
    {
        public int Id { get; set; }
        public int Session { get; set; }
        public string Name { get; set; }
        public int Duration { get; set; }
        public List<LectureDetailModel> Lectures { get; set; }
    }
}