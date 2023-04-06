using Application.Interfaces;
using Application.Repositories;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Infracstructures.Repositories
{
    public class AuditDetailRepository : GenericRepository<AuditDetail>, IAuditDetailRepository
    {
        public AuditDetailRepository(AppDbContext context, ICurrentTime timeService, IClaimsService claimsService) : base(context, timeService, claimsService)
        {
        }

        //your func
        public override async Task<AuditDetail?> GetByIdAsync(int id)
        {
            var auditDetail = await _dbSet.Include(x => x.AuditPlan)
                .ThenInclude(x => x.Class)
                .Include(x => x.AuditPlan)
                .ThenInclude(x => x.Syllabus)
                .Include(x => x.Results)
                .Include(x => x.Auditor)
                .Include(x => x.Trainee)
                .FirstOrDefaultAsync(e => e.Id == id);
            return auditDetail;
        }
    }
}