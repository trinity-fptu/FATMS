using Application.Interfaces;
using Application.ResponseModels;
using Application.Services;
using Application.ViewModels.ClassUnitViewModel;
using Application.ViewModels.ClassViewModels;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ClassUnitController : ControllerBase
    {
        private readonly IClassUnitDetailService _classUnitDetailService;

        public ClassUnitController(IClassUnitDetailService classUnitDetailService)
        {
            _classUnitDetailService = classUnitDetailService;
        }

        [HttpPut("{classId}")]
        public async Task<IActionResult> EditClassnUnitAsync(List<EditClassUnitViewModel> listClassUnit,int classId)

        {
            try
            {
                var tempMess = await _classUnitDetailService.EditListClassUnitAsync(listClassUnit, classId);
                // return status codes with result according to user object
                return Ok(new BaseResponseModel
                {
                    Status = Ok().StatusCode,
                    Message = tempMess,
                    Result = listClassUnit
                });
            }
            catch (Exception ex)
            {
                // return status code bad request for validation
                return BadRequest(new BaseResponseModel
                {
                    Status = BadRequest().StatusCode,
                    Message = ex.Message
                });
            }
        }

    }
}
