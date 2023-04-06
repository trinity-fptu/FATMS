using Application.Interfaces;
using Application.Repositories;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Infracstructures.Repositories
{
    public class GradeReportsRepository : GenericRepository<GradeReport>, IGradeReportsRepository
    {
        public GradeReportsRepository(AppDbContext context, ICurrentTime timeService, IClaimsService claimsService) : base(context, timeService, claimsService)
        {
        }

        //your func
        public override async Task<GradeReport> GetByIdAsync(int id)
        {
            return await _dbSet.Include(x => x.User).Include(x => x.Lecture).FirstAsync(x => x.Id == id);
        }
        public override async Task<List<GradeReport>> GetAllAsync()
        {
            return await _dbSet.Include(x => x.User).Include(x => x.Lecture).ToListAsync();
        }
    }
}