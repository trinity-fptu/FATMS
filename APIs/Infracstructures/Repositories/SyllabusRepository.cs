using Application.Interfaces;
using Application.Repositories;
using Domain.Models;
using Domain.Models.Syllabuses;
using Microsoft.EntityFrameworkCore;

namespace Infracstructures.Repositories
{
    public class SyllabusRepository : GenericRepository<Syllabus>, ISyllabusRepository
    {
        public SyllabusRepository(AppDbContext context, ICurrentTime timeService, IClaimsService claimsService) : base(
            context, timeService, claimsService)
        {
        }

        public async Task<Syllabus?> GetSyllabusDetailByIdAsync(int id)
        {
            var syllabusDetail = await _dbSet
                .Include(e => e.Units.Where(e => e.Syllabuses.Any(c => c.Id == id)))
                .ThenInclude(e => e.Lectures)
                .ThenInclude(e => e.TrainingMaterials)
                .Include(e => e.Units.Where(e => e.Syllabuses.Any(c => c.Id == id)))
                .ThenInclude(e => e.Lectures)
                .ThenInclude(e => e.OutputStandard)
                .Include(e => e.ModifiedAdmin)
                .Include(e => e.CreatedAdmin)
                .FirstOrDefaultAsync(e => e.Id == id);
            return syllabusDetail;
        }

        public async Task<Syllabus> GetSyllabusByID(int id)
        {
            var syllabus = await _dbSet.FirstOrDefaultAsync(syl => syl.Id == id && !syl.isDeleted);
            if (syllabus == null)
            {
                throw new Exception("Id not existed or this sysllabus has been deleted!! ");
            }

            return syllabus;
        }

        public async Task<List<int>> GetUnitId(List<int> listSyllabusId)
        {
            List<int> unitIdList = new List<int>();
            foreach (int i in listSyllabusId)
            {
                var temp = await _dbSet.Include(e => e.Units).FirstOrDefaultAsync(x => x.Id == i);
                unitIdList.AddRange(temp.Units.Select(x => x.Id).ToList());
            }

            return unitIdList;
        }

        #region GetSyllabusByName

        public async Task<Syllabus?> GetLatestByNameAsync(string name)
        {
            var listSyllabusName = await _dbSet
                .Where(x => x.Name.ToUpper().Equals(name.Trim().ToUpper()) && !x.isDeleted).ToListAsync();
            listSyllabusName = listSyllabusName.OrderBy(x => x.Version).ToList();
            return listSyllabusName.LastOrDefault();
        }

        #endregion

        #region GetSyllabusByCode

        public async Task<Syllabus?> GetLatestByCodeAsync(string code)
        {
            var listSyllabusCode = await _dbSet.Where(x => x.Code.ToUpper().Equals(code.ToUpper()) && !x.isDeleted)
                .ToListAsync();
            listSyllabusCode = listSyllabusCode.OrderBy(x => x.Version).ToList();
            return listSyllabusCode.LastOrDefault();
        }

        #endregion

        public Task<Syllabus> GetSyllabusToUpdateById(int syllabusId)
        {
            return _dbSet.Include(e => e.Units.Where(e => e.Syllabuses.Any(c => c.Id == syllabusId)))
                .ThenInclude(e => e.Lectures)
                .Include(e => e.ModifiedAdmin)
                .Include(e => e.CreatedAdmin)
                .FirstOrDefaultAsync(e => e.Id == syllabusId);
        }
        
        public Task<IEnumerable<string>> GetOutputStandardCode()
        {
            return _dbSet.Include(e => e.Units)
                .ThenInclude(e => e.Lectures)
                .ThenInclude(e => e.OutputStandard)
                .Select(e => e.Units.SelectMany(x => x.Lectures.Select(y => y.OutputStandard.Code)))
                .FirstOrDefaultAsync();
        }
    }
}