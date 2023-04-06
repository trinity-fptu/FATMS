using Application.Interfaces;
using Application.ResponseModels;
using Application.ViewModels.AttendanceViewModels;
using FluentValidation.AspNetCore;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class AttendanceController : ControllerBase
    {
        private readonly IAttendanceService _attendanceService;

        public AttendanceController(IAttendanceService attendanceService)
        {
            _attendanceService = attendanceService;
        }

        //actions

        [HttpPut("{id}")]
        public async Task<IActionResult> EditAttendanceAsync(int id, EditAttendanceViewModel editAttendanceViewModel)
        {
            try
            {
                var result = await _attendanceService.EditAttendanceAsync(editAttendanceViewModel, id);
                return Ok(new BaseResponseModel
                {
                    Status = Ok().StatusCode,
                    Message = "Edit Attendance Succeed",
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new BaseResponseModel
                {
                    Status = BadRequest().StatusCode,
                    Message = ex.Message
                });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAttendanceByClassIdAsync(int id)
        {
            try
            {
                var result = await _attendanceService.GetAllAttendanceAsync(id);
                return Ok(new BaseResponseModel
                {
                    Status = Ok().StatusCode,
                    Message = "Get Attendance Succeed",
                    Result = result
                });
            }
            catch (Exception e)
            {
                return BadRequest(new BaseResponseModel
                {
                    Status = BadRequest().StatusCode,
                    Message = e.Message
                });
            }
        }

        [HttpPost]
        public async Task<IActionResult> TakeAttendance(TakeAttendanceModel model)
        {
            try
            {
                var validateResult = await _attendanceService.ValidateTakeAttendanceAsync(model);

                if (validateResult.IsValid)
                {
                    // return status codes with result according to user object
                    var result = await _attendanceService.TakeAttendance(model);

                    return Ok(new BaseResponseModel
                    {
                        Status = Ok().StatusCode,
                        Message = "Take Attendance Succeed",
                        Result = result
                    });
                }

                validateResult.AddToModelState(ModelState);

                return BadRequest(new BaseFailedResponseModel
                {
                    Status = BadRequest().StatusCode,
                    Message = "Validate Failed",
                    Errors = ModelState
                });
            }
            catch (Exception ex)
            {
                // return status code bad request for validation
                return BadRequest(ex.Message);

            }
        }
    }
}