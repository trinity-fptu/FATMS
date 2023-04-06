using Domain.Models.Users;

namespace Application.Repositories
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<User?> GetUserByUserEmailAndPasswordHash(string email, string passwordHash);
        Task<User> GetUserByEmailAsync(string email);
        Task<bool> IsExistsUser(string email);
        Task<List<User>> GetUserByRoleAsync(string roleName);
        Task<bool> IsExistNameAsync(string name);
    }
}