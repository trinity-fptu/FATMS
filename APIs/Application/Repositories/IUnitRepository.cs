using Domain.Models;

namespace Application.Repositories
{
    public interface IUnitRepository : IGenericRepository<Unit>
    {
        Task<List<Unit>> GetUnitsBySyllabusId(int syllabusId);
        Task<Unit> GetUnitById(int id);
    }
}