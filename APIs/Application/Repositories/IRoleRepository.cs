using Domain.Models;

namespace Application.Repositories
{
    public interface IRoleRepository : IGenericRepository<Role>
    {
        Task<bool> IsExistRoleAsync(string name);
        Task<Role> GetRoleByNameAsync(string name);
        Task<int> GenerateRoleIdAsync();
    }
}
