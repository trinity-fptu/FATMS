namespace Application.ViewModels.UserViewModels
{
    public class UserListModel
    {
        // User list model used for retrieving users list
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string DateOfBirth { get; set; }
        public bool IsMale { get; set; } = true;
        public string? Level { get; set; } = null;
        public string Role { get; set; }
        public string? Status { get; set; } = null;

    }
}
