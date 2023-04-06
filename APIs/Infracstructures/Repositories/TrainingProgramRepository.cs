using Application.Interfaces;
using Application.Repositories;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Infracstructures.Repositories
{
    public class TrainingProgramRepository : GenericRepository<TrainingProgram>, ITrainingProgramRepository
    {
        private readonly AppDbContext _context;

        public TrainingProgramRepository(AppDbContext context, ICurrentTime timeService, IClaimsService claimsService) : base(context, timeService, claimsService)
        {
            _context = context;
        }

        public async Task<bool> getStatusTrainingProgram(int id)
        {
            var trainingProgram = await GetByIdAsync(id);
            return trainingProgram.IsActive;
        }

        public override async Task<TrainingProgram> GetByIdAsync(int id)
        {
            return await _dbSet.Include(x => x.CreatedAdmin).Include(x => x.ModifiedAdmin).Include(x => x.Syllabuses).FirstAsync(x => x.Id == id);
        }

        public override async Task<List<TrainingProgram>> GetAllAsync()
        {
            return await _dbSet.Include(x => x.CreatedAdmin).Include(x => x.ModifiedAdmin).Include(x => x.Syllabuses).Where(x => !x.isDeleted).ToListAsync();
        }

        public async Task<bool> IsExistNameAsync(string name)
        {
            return await _dbSet.FirstOrDefaultAsync(x => x.Name.Equals(name) && !x.isDeleted) != null;
        }

        public async Task<TrainingProgram?> GetByNameAsync(string name)
        {
            return await _dbSet.Include(x => x.Syllabuses).FirstOrDefaultAsync(x => x.Name.Equals(name) && !x.isDeleted);
        }
    }
}
