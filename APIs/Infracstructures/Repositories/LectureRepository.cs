using Application.Interfaces;
using Application.Repositories;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Infracstructures.Repositories
{
    public class LectureRepository : GenericRepository<Lecture>, ILecturesRepository
    {
        public LectureRepository(AppDbContext context, ICurrentTime timeService, IClaimsService claimsService) : base(context, timeService, claimsService)
        {
        }

        public async Task<List<Lecture>> GetByUnitIdsAsync(List<int> unitIds)
        {
            var lectures = await _dbSet.Where(l => unitIds.Contains((int)l.UnitId)).ToListAsync();
            return lectures;
        }

        //your func
       
    }
}