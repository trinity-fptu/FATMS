using Domain.Models.Syllabuses;

namespace Application.Repositories
{
    public interface ISyllabusRepository : IGenericRepository<Syllabus>
    {
        //add your own func here
        Task<Syllabus?> GetSyllabusDetailByIdAsync(int id);
        Task<Syllabus> GetSyllabusByID(int id);
        Task<Syllabus?> GetLatestByNameAsync(string name);
        Task<Syllabus?> GetLatestByCodeAsync(string code);
        Task<List<int>> GetUnitId(List<int> listSyllabusId);
        Task<Syllabus> GetSyllabusToUpdateById(int syllabusId);
        Task<IEnumerable<string>> GetOutputStandardCode();


    }
}