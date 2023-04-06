using Application.Interfaces;
using Application.Repositories;
using Domain.Models;
using Microsoft.EntityFrameworkCore;


namespace Infracstructures.Repositories
{
    public class ClassUnitDetailRepository : GenericRepository<ClassUnitDetail>, IClassUnitDetailRepository
    {        
        public ClassUnitDetailRepository(AppDbContext context, ICurrentTime timeService, IClaimsService claimsService) : base(context, timeService, claimsService)
        {
        }

        public virtual async Task<ClassUnitDetail> GetClassUnit(int classId, int unitId) => await _dbSet.FirstOrDefaultAsync(x => x.ClassId == classId && x.UnitId == unitId);
    }
}
