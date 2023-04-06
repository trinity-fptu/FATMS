using Application.ViewModels.SyllabusViewModels;

namespace Application.ViewModels.TrainingProgramViewModels
{
    public class TrainingProgramViewModels
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public int DaysDuration { get; set; }
        public int TimeDuration { get; set; }
        public string CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public string LastModify { get; set; }
        public string LastModifyBy { get; set; }
        public List<int> syllabusDetailIds { get; set; }
    }
}

