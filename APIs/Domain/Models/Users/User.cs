using Domain.Enums.UserEnums;

namespace Domain.Models.Users
#nullable disable warnings
{
    public partial class User
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Phone { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int RoleId { get; set; }
        public UserLevel? Level { get; set; } = null;
        public UserStatus? Status { get; set; } = null;
        public bool IsMale { get; set; } = true;
        public string AvatarURL { get; set; }
        public string? ResetToken { get; set; }
        public bool isDeleted { get; set; } = false;
    }
}