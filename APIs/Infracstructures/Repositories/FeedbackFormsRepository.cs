using Application.Interfaces;
using Application.Repositories;
using Domain.Models;

namespace Infracstructures.Repositories
{
    public class FeedbackFormsRepository : GenericRepository<FeedbackForm>, IFeedbackFormsRepository
    {
        public FeedbackFormsRepository(AppDbContext context, ICurrentTime timeService, IClaimsService claimsService) : base(context, timeService, claimsService)
        {
        }

        //your func
    }
}