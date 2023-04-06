using Application.Interfaces;
using Application.Repositories;
using Domain.Models;

namespace Infracstructures.Repositories
{
    public class TimeMngSystemsRepository : GenericRepository<TMS>, ITimeMngSystemsRepository
    {
        public TimeMngSystemsRepository(AppDbContext context, ICurrentTime timeService, IClaimsService claimsService) : base(context, timeService, claimsService)
        {
        }

        //your func
    }
}