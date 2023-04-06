using Application.Interfaces;
using Application.Repositories;
using Domain.Models;

namespace Infracstructures.Repositories
{
    public class AuditResultRepository : GenericRepository<AuditResult>, IAuditResultRepository
    {
        public AuditResultRepository(AppDbContext context, ICurrentTime timeService, IClaimsService claimsService) : base(context, timeService, claimsService)
        {
        }

        //your func
    }
}