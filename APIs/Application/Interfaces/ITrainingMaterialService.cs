using Application.ViewModels.TrainingMaterialViewModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface ITrainingMaterialService
    {
        Task AddTrainingMaterialAsync(int lectureId, IFormFile fileModel);
    }
}
