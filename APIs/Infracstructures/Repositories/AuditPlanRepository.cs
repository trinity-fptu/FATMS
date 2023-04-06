using Application.Interfaces;
using Application.Repositories;
using Domain.Models;

namespace Infracstructures.Repositories
{
    public class AuditPlanRepository : GenericRepository<AuditPlan>, IAuditPlanRepository
    {
        public AuditPlanRepository(AppDbContext context, ICurrentTime timeService, IClaimsService claimsService) : base(context, timeService, claimsService)
        {
        }

        //your func
    }
}