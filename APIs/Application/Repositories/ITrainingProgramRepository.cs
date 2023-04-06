using Application.ViewModels.TrainingProgramViewModels;
using Domain.Models;

namespace Application.Repositories
{
    public interface ITrainingProgramRepository : IGenericRepository<TrainingProgram>
    {
        Task<bool> getStatusTrainingProgram(int id);
        Task<bool> IsExistNameAsync(string name);
        Task<TrainingProgram?> GetByNameAsync(string name);
    }
}