namespace Application.ViewModels.UserViewModels
{
    public class UserResetPasswordModel
    {
        public string Email { get; set; }
        public string Token { get; set; }
        public string NewPassword { get; set; }
    }
}
