using Application.Interfaces;
using Application.ResponseModels;
using Application.Services;
using Application.ViewModels.TrainingMaterialViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrainingMaterialController : ControllerBase
    {
        private readonly ITrainingMaterialService _trainingMaterialService;

        public TrainingMaterialController(ITrainingMaterialService trainingMaterialService) 
        {
            _trainingMaterialService = trainingMaterialService;
        }

        [HttpPost("{lectureId}")]
        public async Task<IActionResult> AddNewTrainingMaterialAsync(int lectureId, IFormFile fileModel)
        {
            try
            {
                var file = fileModel;
                var filename = fileModel.FileName;
                await _trainingMaterialService
                    .AddTrainingMaterialAsync(lectureId, fileModel);

                return Ok(new BaseResponseModel()
                {
                    Status = Ok().StatusCode,
                    Message = "Add file to lecture success",
                    Result = fileModel
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new BaseFailedResponseModel()
                {
                    Status = BadRequest().StatusCode,
                    Message = ex.Message,
                    Errors = ex
                });
            }
        }
    }
}
