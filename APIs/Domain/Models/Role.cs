using Domain.Enums.RoleEnums;
using Domain.Models.Users;

#nullable disable warnings
namespace Domain.Models
{
    public class Role
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public UserPermission? SyllabusPermission { get; set; }
        public UserPermission? TrainingProgramPermission { get; set; }
        public UserPermission? ClassPermission { get; set; }
        public UserPermission? LearningMaterialPermission { get; set; }
        public UserPermission? UserPermission { get; set; }

        public ICollection<User> Users { get; set; } = new List<User>();
    }
}
