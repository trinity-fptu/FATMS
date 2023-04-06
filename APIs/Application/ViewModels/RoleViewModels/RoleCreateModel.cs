#nullable disable warnings
namespace Application.ViewModels.RoleViewModels
{
    public class RoleCreateModel : BaseRoleViewModel
    {
        public string Name { get; set; }
        public string SyllabusPermission { get; set; }
        public string TrainingProgramPermission { get; set; }
        public string ClassPermission { get; set; }
        public string LearningMaterialPermission { get; set; }
        public string UserPermission { get; set; }
    }
}
