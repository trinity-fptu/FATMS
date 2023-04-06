namespace Application.ViewModels.TrainingProgramViewModels
{
    public class CreateTrainingProgramViewModels
    {
        public string Name { get; set; }
        public ICollection<int> SyllabusIds { get; set; }
    }
}
