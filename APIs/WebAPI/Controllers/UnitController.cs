using Application.Interfaces;
using Application.IValidators;
using Application.ResponseModels;
using Application.ViewModels.UnitViewModels;
using Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UnitController : ControllerBase
    {
        private readonly IUnitService _unitService;

        public UnitController(IUnitService unitService)
        {
            _unitService = unitService;
        }

        //[HttpPost]
        //public async Task<IActionResult> Create(UnitAddViewModel unitViewModel)
        //{
        //    try
        //    {
        //        var validationResult = await _unitService.ValidateAddAsync(unitViewModel);
        //        if (!validationResult.IsValid)
        //        {
        //            return BadRequest(new BaseFailedResponseModel()
        //            {
        //                Status = BadRequest().StatusCode,
        //                Message = "Validation failed",
        //                Errors = validationResult.Errors
        //            });
        //        }

        //        await _unitService.AddAsync(unitViewModel);

        //        return Ok(new BaseResponseModel()
        //        {
        //            Status = Ok().StatusCode,
        //            Message = "Create unit success"
        //        });
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(new BaseFailedResponseModel()
        //        {
        //            Status = BadRequest().StatusCode,
        //            Message = ex.Message
        //        });
        //    }

        //}

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUnitByIdAsync(int id)
        {
            try
            {
                var unit = await _unitService.GetUnitByIdAsync(id);

                return Ok(new BaseResponseModel
                {
                    Status = Ok().StatusCode,
                    Message = "Get unit success",
                    Result = unit
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new BaseFailedResponseModel
                {
                    Status = BadRequest().StatusCode,
                    Message = ex.Message,
                    Errors = ex
                });
            }
        }
    }
}
