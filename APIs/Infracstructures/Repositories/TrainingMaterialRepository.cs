using Application.Interfaces;
using Application.Repositories;
using Application.ViewModels.TrainingProgramViewModels;
using Domain.Models;

namespace Infracstructures.Repositories
{
    public class TrainingMaterialRepository : GenericRepository<TrainingMaterial>, ITrainingMaterialRepository
    {
        public TrainingMaterialRepository(AppDbContext context, ICurrentTime timeService, IClaimsService claimsService) : base(context, timeService, claimsService)
        {
        }

       



        //your func
    }
}