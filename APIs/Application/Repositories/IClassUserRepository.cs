using Domain.Models;

namespace Application.Repositories
{
    public interface IClassUserRepository : IGenericRepository<ClassUsers>
    {
        //add your own func here
        Task<List<ClassUsers>> GetClassUsers(int id);
        Task<bool> IsExist(int classId, int userId);
    }
}