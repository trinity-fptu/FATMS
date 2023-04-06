using Application.Commons;
using Application.ViewModels.UserViewModels;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Domain.Models.Users;
using Domain.Enums.ResponeModelEnums;

namespace Application.Interfaces
{
    public interface IUserService
    {
        public Task<string> LoginAsync(UserLoginModel userLogin);

        // Asynchronous method to get user's information by id
        Task<UserDetailModel?> GetUserDetailAsync(int id);

        // Asynchronous method to validate UserUpdateModel
        Task<ValidationResult> ValidateUpdateUserAsync(UserUpdateModel updatedUser);

        // Asynchronous method to get user's information to update
        public Task<bool> EditAsync(UserUpdateModel userUpdateModel, User user);

        // Asynchronous method to delete user
        public Task<bool> DeleteAsync(User user);
        
        // Asynchronous method to get user by id
        public Task<User> GetUserByIdAsync(int id);

        // Asynchronous method to get users list with paging
        Task<Pagination<UserListModel>> GetUserListPaginationAsync(int pageIndex = 0, int pageSize = 10);
        Task<ValidationResult> ValidateCreateUserAsync(UserCreateModel createdUser);
        Task<UserCreateOptions> GetCreateOptionsAsync();
        public Task<bool> CreateUserAsync(UserCreateModel createdUser);
        Task<bool> SendMailCreateUserAsync(string email, string password);
        Task<List<UserCreateModel>?> ImportCsvFileAsync(IFormFile formFile, bool isScanFullName, 
            bool isScanEmail, DuplicateHandle duplicateHandle);
        byte[] ExportCsvFile(string columnSeperator);
        Task<List<UserCreateModel>[]> ImportScanningHandle(List<UserCreateModel> users, bool isScanFullName,
            bool isScanEmail, DuplicateHandle duplicateHandle);
        Task<bool> IsExistsUserAsync(string email);        
        Task<List<UserListModel>> GetUserListAsync();
        string GeneratePasswordToken(User user);
        Task<string?> ForgotPasswordAsync(string email);
        Task<bool> IsValidPasswordTokenAsync(string email, string token);
        Task<bool> ChangePasswordAsync(string email, string newPassword);
        bool IsExpiredPasswordToken(string token);
        Task<bool> HasPasswordTokenMailSentAsync(string email, string token);
        Task<UserDetailModel> GetUserDetailByEmailAsync(string email);
        Task<User> GetUserByEmailAsync(string email);
        Task<List<UserDetailModel>> GetUsersByRoleAsync(string roleName);
        Task<List<UserListModel>> Filter(UserFilterModel filter);
        bool IsValidToken(string token);
        bool Logout();
        Task<Stream> ExportExcelFile();
        Task<List<UserCreateModel>?> ImportExcelFileAsync(IFormFile formFile,
            bool isScanFullName,
            bool isScanEmail,
            DuplicateHandle duplicateHandle);
    }
}