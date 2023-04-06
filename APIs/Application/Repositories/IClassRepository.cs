using Domain.Models;

namespace Application.Repositories
{
    public interface IClassRepository : IGenericRepository<Class>
    {
        //add your own func here
        Task<List<int>> GetSyllabusId(int trainingProgramId);
    }
}