using Application.Interfaces;
using Application.ResponseModels;
using Application.ViewModels.AuditViewModels;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class AuditController : ControllerBase
    {
        private readonly IAuditService _auditService;

        public AuditController(IAuditService auditService)
        {
            _auditService = auditService;
        }

        //actions
        [HttpPost]
        public async Task<IActionResult> AddAuditPlanAsync(AddAuditPlanViewModel model)
        {
            try
            {
                var result = await _auditService.AddAuditPlanAsync(model);
                return Ok(new BaseResponseModel()
                {
                    Status = Ok().StatusCode,
                    Message = "Add audit plan success",
                    Result = result
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new BaseFailedResponseModel()
                {
                    Status = BadRequest().StatusCode,
                    Message = ex.Message
                });
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddAuditDetailAsync(AddAuditDetailViewModel model)
        {
            try
            {
                var result = await _auditService.AddAuditDetailAsync(model);
                return Ok(new BaseResponseModel()
                {
                    Status = Ok().StatusCode,
                    Message = "Add audit detail success",
                    Result = result
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new BaseFailedResponseModel()
                {
                    Status = BadRequest().StatusCode,
                    Message = ex.Message
                });
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddAuditResultAsync(AddAuditResultViewModel model)
        {
            try
            {
                var result = await _auditService.AddAuditResultAsync(model);
                return Ok(new BaseResponseModel()
                {
                    Status = Ok().StatusCode,
                    Message = "Add audit result success",
                    Result = result
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new BaseFailedResponseModel()
                {
                    Status = BadRequest().StatusCode,
                    Message = ex.Message
                });
            }
        }

        [HttpGet("{auditId}")]
        public async Task<IActionResult> GetAuditResultDetailAsync(int auditId)
        {
            try
            {
                var result = await _auditService.GetAuditResultDetailAsync(auditId);
                return Ok(new BaseResponseModel()
                {
                    Status = Ok().StatusCode,
                    Message = "Success",
                    Result = result
                });
            }
            catch (NullReferenceException ex)
            {
                return NotFound(new BaseFailedResponseModel()
                {
                    Status = NotFound().StatusCode,
                    Message = ex.Message
                });
            }
        }

        [HttpPost("{id}")]
        public async Task<IActionResult> UpdateFeedbackAuditDeatilsAsync(int id, string feedback)
        {
            try
            {
                var result = await _auditService.UpdateFeedbackAuditDeatilsAsync(id, feedback);
                return Ok(new BaseResponseModel
                {
                    Status = Ok().StatusCode,
                    Message = "Update Succeed",
                    Result = "Update Succeed"
                });
            }
            catch (NullReferenceException ex)
            {
                return NotFound(new BaseFailedResponseModel
                {
                    Status = NotFound().StatusCode,
                    Message = ex.Message,
                    Errors = ex
                });
            }
        }
    }
}