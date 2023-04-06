using Application.Interfaces;
using Application.Repositories;
using Domain.Models;

namespace Infracstructures.Repositories
{
    public class OutputStandardRepository : GenericRepository<OutputStandard>, IOutputStandardRepository
    {
        public OutputStandardRepository(AppDbContext context, ICurrentTime timeService, IClaimsService claimsService) : base(context, timeService, claimsService)
        {
        }

        //your func
    }
}