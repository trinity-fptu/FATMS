#nullable disable warnings
namespace Application.ViewModels.UserViewModels
{
    public class BaseUserViewModel
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Role { get; set; }
        public string Level { get; set; }
        public string Status { get; set; }
        public bool IsMale { get; set; } = true;
    }
}
