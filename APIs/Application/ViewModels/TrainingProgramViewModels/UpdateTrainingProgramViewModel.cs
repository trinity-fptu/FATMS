namespace Application.ViewModels.TrainingProgramViewModels
{
    public class UpdateTrainingProgramViewModel
    {
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public ICollection<int> SyllabusId { get; set; }
    }
}
