namespace Application.ViewModels.UserViewModels
{
    public class UserUpdateModel
    {
        // User's information apart from username, password, email, phone

        public int Id { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; }
        public string DateOfBirth { get; set; }
        public string IsMale { get; set; }
        public int RoleId { get; set; }
        public string Level { get; set; }
        public string Status { get; set; }
        public string? AvatarURL { get; set; }
    }
}
