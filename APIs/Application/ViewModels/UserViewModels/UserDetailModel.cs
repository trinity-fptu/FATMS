namespace Application.ViewModels.UserViewModels
{
    public class UserDetailModel
    {
        // User detail model containing user's information (except user's password)
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string DateOfBirth { get; set; }
        public bool IsMale { get; set; } = true;
        public string? Level { get; set; } = null;
        public string Role { get; set; }
        public string? Status { get; set; } = null;
        public string AvatarURL { get; set; }
    }
}
