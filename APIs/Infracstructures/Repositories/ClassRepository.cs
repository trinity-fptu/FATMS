using Application.Interfaces;
using Application.Repositories;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Infracstructures.Repositories
{
    public class ClassRepository : GenericRepository<Class>, IClassRepository
    {
        public ClassRepository(AppDbContext context, ICurrentTime timeService, IClaimsService claimsService) : base(context, timeService, claimsService)
        {
        }
        public async Task<List<int>> GetSyllabusId(int trainingProgramId)
        {
            List<int> syllabusIdList = new List<int>();
            var temp = await _dbSet.Include(e => e.TrainingProgram)
                .ThenInclude(e => e.Syllabuses)
                .FirstOrDefaultAsync(x => x.Id == trainingProgramId);
            syllabusIdList.AddRange(temp.TrainingProgram.Syllabuses.Select(x => x.Id).ToList());
            return syllabusIdList;
        }

        public override async Task<List<Class>> GetAllAsync()
        {
            return await _dbSet.Include(x => x.CreatedAdmin).ToListAsync();
        }

        public override async Task<Class> GetByIdAsync(int id)
        {
            Class result = await _dbSet.Include(x => x.CreatedAdmin)
                        .Include(x => x.ClassUsers)
                            .ThenInclude(x => x.User)
                        .Include(x => x.TrainingProgram)
                            .ThenInclude(x => x.Syllabuses)
                            .ThenInclude(x => x.Units)
                        .Include(x => x.ClassUnitDetails)
                            .ThenInclude(x => x.Trainer)
                        .FirstAsync(x => x.Id == id);
            return result;
        }
    }
}