using Application.Interfaces;
using Application.ResponseModels;
using Application.Services;
using Application.ViewModels.ClassViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ClassController : ControllerBase
    {
        private readonly IClassService _classService;

        public ClassController(IClassService classService)
        {
            _classService = classService;
        }

        //[Authorize(Roles = "Trainer,ClassAdmin,SuperAdmin")]

        [HttpPost]
        public async Task<IActionResult> CreateClass(CreateClassViewModel createClassViewModel)
        {
            try
            {
                var tempMess = await _classService.CreateClassAsync(createClassViewModel);
                // return status codes with result according to user object
                return Ok(new BaseResponseModel
                {
                    Status = Ok().StatusCode,
                    Message = tempMess,
                    Result = createClassViewModel
                });
            }
            catch (Exception ex)
            {
                // return status code bad request for validation
                return BadRequest(new BaseFailedResponseModel
                {
                    Status = BadRequest().StatusCode,
                    Message = ex.Message,
                    Errors = ex
                });
            }
        }

        [HttpGet]
        public async Task<IActionResult> ListClassAsync()
        {
            try
            {
                var classes = await _classService.GetClassListAsync();
                return Ok(new BaseResponseModel()
                {
                    Status = Ok().StatusCode,
                    Message = "Get All Succeed",
                    Result = new
                    {
                        totalItemsCount = classes.Count,
                        items = classes
                    }
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new BaseFailedResponseModel()
                {
                    Status = BadRequest().StatusCode,
                    Message = "Get All failed",
                    Errors = ex.Message
                });
            }
        }

        [HttpGet]
        public async Task<IActionResult> ListOpeningClassAsync()
        {
            try
            {
                var classes = await _classService.GetOpeningClassListAsync();
                return Ok(new BaseResponseModel()
                {
                    Status = Ok().StatusCode,
                    Message = "Get All Succeed",
                    Result = new
                    {
                        totalItemsCount = classes.Count,
                        items = classes
                    }
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new BaseFailedResponseModel()
                {
                    Status = BadRequest().StatusCode,
                    Message = "Get All failed",
                    Errors = ex.Message
                });
            }
        }
        [HttpPost]
        public async Task<IActionResult> AddUserToClass(AddUserToClassViewModel addUserToClassViewModel)
        {
            try
            {

                var tempMess = await _classService.AddUserToClassAsync(addUserToClassViewModel);
                // return status codes with result according to user object
                return Ok(new BaseResponseModel
                {
                    Status = Ok().StatusCode,
                    Message = tempMess,
                    Result = addUserToClassViewModel
                });
            }
            catch (Exception ex)
            {
                // return status code bad request for validation
                return BadRequest(new BaseFailedResponseModel
                {
                    Status = BadRequest().StatusCode,
                    Message = ex.Message,
                    Errors = ex.Message
                });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateClass(int id, UpdateClassViewModel updateClass)
        {
            try
            {
                var result = await _classService.UpdateClassAsync(id, updateClass);
                return Ok(new BaseResponseModel { 
                    Status = Ok().StatusCode,
                    Message = "Update Succeed"
                });
            }
            catch (Exception ex)
            {
                return NotFound(new BaseFailedResponseModel { 
                    Status = NotFound().StatusCode,
                    Message = ex.Message,
                    Errors = ex.Message
                });
                
            }
        }

        //Class Detail
        [HttpGet("{id}")]
        public async Task<IActionResult> GetClassDetails(int id)
        {
            try
            {
                if (id == 0 || id == null) return BadRequest("ID cannot null or by zero");
                var result = await _classService.GetClassDetail(id);
                return Ok(new BaseResponseModel
                {
                    Status = Ok().StatusCode,
                    Message = "Succeed",
                    Result = result
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new BaseFailedResponseModel { 
                    Status = BadRequest().StatusCode,
                    Message = ex.Message,
                    Errors = ex
                });
            }
        }

        #region Clone Class
        [HttpPost("{id}")]
        public async Task<IActionResult> CloneClassAsync(int id)
        {
            try
            {
                await _classService.CloneClassAsync(id);
                return Ok(new BaseResponseModel
                {
                    Status = Ok().StatusCode,
                    Message = "Clone Succeed",
                    Result = "Success"
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
        #endregion
    }
}