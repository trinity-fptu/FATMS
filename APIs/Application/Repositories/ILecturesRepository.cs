using Domain.Models;

namespace Application.Repositories
{
    public interface ILecturesRepository : IGenericRepository<Lecture>
    {
        //add your own func here
        Task<List<Lecture>> GetByUnitIdsAsync(List<int> unitIds);
    }
}