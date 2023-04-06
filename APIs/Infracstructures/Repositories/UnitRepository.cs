using Application.Interfaces;
using Application.Repositories;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Infracstructures.Repositories
{
    public class UnitRepository : GenericRepository<Unit>, IUnitRepository
    {
        public UnitRepository(AppDbContext context, ICurrentTime timeService, IClaimsService claimsService) : base(context, timeService, claimsService)
        {
        }

        public async Task<List<Unit>> GetUnitsBySyllabusId(int syllabusId)
        {
            var units = await _dbSet.Where(e => e.Syllabuses.Any(c => c.Id == syllabusId))
                .Include(e => e.Lectures)
                .ThenInclude(e => e.TrainingMaterials)
                .Include(e => e.Lectures)
                .ThenInclude(e => e.OutputStandard)
                .ToListAsync();
            return units;
        }

        public async Task<Unit> GetUnitById(int id)
        {
            var unit = await _dbSet
                .Include(e => e.Lectures)
                .FirstOrDefaultAsync(e => e.Id == id);
            return unit;
        }
        //your func

    }
}