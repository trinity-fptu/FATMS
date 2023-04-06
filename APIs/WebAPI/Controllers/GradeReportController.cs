using Application.Interfaces;
using Application.ResponseModels;
using Application.ViewModels.ClassViewModels;
using Application.ViewModels.GradeViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class GradeReportController : ControllerBase
    {
        private readonly IGradeReportService _gradeReportService;

        public GradeReportController(IGradeReportService gradeReportService)
        {
            _gradeReportService = gradeReportService;
        }

        #region Create GradeReport

        [HttpPost]
        public async Task<IActionResult> CreateGradeReportAsync(GradeViewModel gradeViewModel)
        {
            try
            {
                 await _gradeReportService.CreateReportAsync(gradeViewModel);
                return Ok(new 
                {
                    Status = "Ok",
                    Message = "Grade success",
                    Result = gradeViewModel
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

        #endregion
        #region ViewGradeReport
        [HttpGet]
        public async Task<IActionResult> ViewGradeReport(int id)
        {
            if (id == 0 || id == null) return BadRequest("Id must be 1 or more than!");
            var result = await _gradeReportService.ViewGradeReportByIdAsync(id);
            if (result != null)
            {
                return Ok(result);
            }
            return BadRequest("This Student not be grade or not exsist!!");
        }
        [HttpGet]
        public async Task<IActionResult> ViewAllGradeReport()
        {
            var result = await _gradeReportService.ViewAllGradeReportAsync();
            if (result != null)
            {
                return Ok(result);
            }
            return BadRequest("Nobody be grade!!!");
        }
        #endregion
    }
}