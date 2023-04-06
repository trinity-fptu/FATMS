using Application.Interfaces;
using Application.ViewModels.TrainingProgramViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Application.ResponseModels;
using Domain.Enums.ResponeModelEnums;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class TrainingProgramController : ControllerBase
    {
        private readonly ITrainingProgramService _trainingProgramService;

        public TrainingProgramController(ITrainingProgramService trainingProgramService)
        {
            _trainingProgramService = trainingProgramService;
        }

        #region Training Program Get Training Program By ID
        //Controller Training Program Get Training Program By ID
        [HttpGet("{Id}")]
        public async Task<IActionResult> GetByIdAsync(int Id)
        {
            try
            {
                var result = await _trainingProgramService.GetByIdAsync(Id);
                return Ok(new BaseResponseModel()
                {
                    Status = Ok().StatusCode,
                    Message = "Get All Succeed",
                    Result = new
                    {
                        totalItemCount = 1,
                        item = result
                    }
                });
            }

            catch (ArgumentException ex)
            {
                return BadRequest(new BaseFailedResponseModel()
                {
                    Status = BadRequest().StatusCode,
                    Message = "Invalid parameters",
                    Errors = ex.Message
                });
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new BaseFailedResponseModel()
                {
                    Status = NotFound().StatusCode,
                    Message = "Not Found",
                    Errors = ex.Message
                });
            }
        }
        #endregion

        #region Training Program Get All 
        //Controller Training Program Get All 
        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            try
            {
                var result = await _trainingProgramService.GetAllAsync();
                return Ok(new
                {
                    status = "Ok",
                    message = "Get All Succeed",
                    result = new
                    {
                        totalItemsCount = result.Count,
                        items = result
                    }
                });
            }
            catch (Exception ex)
            {
                return NotFound(new BaseFailedResponseModel()
                {
                    Status = NotFound().StatusCode,
                    Message = "Not Found",
                    Errors = ex.Message
                });
            }
        }
        #endregion

        #region Training Program Get All TrainingProgram is Active
        //Controller Training Program Get All TrainingProgram is Active
        [HttpGet]
        public async Task<IActionResult> GetAllTrainingProgramIsActiveAsync()
        {
            try
            {
                var result = await _trainingProgramService.GetAllTrainingProgramIsActiveAsync();
                return Ok(new
                {
                    status = "Ok",
                    message = "Get All Succeed",
                    result = new
                    {
                        totalItemsCount = result.Count,
                        items = result
                    }
                });
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
        #endregion

        #region Get TrainingProgram Is Active By Name
        //Controller Training Program Get TrainingProgram is Active By Name
        [HttpGet("{name?}")]
        public async Task<IActionResult> GetTrainingProgramIsActiveByNameAsync(string? name)
        {
            //If name null change to method GetAllTrainingProgramIsActiveAsync()
            if (name.IsNullOrEmpty())
            {
                return await GetAllTrainingProgramIsActiveAsync();
            }
            var result = await _trainingProgramService.GetTrainingProgramIsActiveByNameAsync(name);
            return Ok(new BaseResponseModel()
            {
                Status = Ok().StatusCode,
                Message = "Get All Succeed",
                Result = new
                {
                    totalItemsCount = result.Count,
                    items = result
                }
            });
        }
        #endregion

        #region Get TrainingProgram By Name
        //Controller Training Program Get TrainingProgram is Active By Name
        [HttpGet("{name?}")]
        public async Task<IActionResult> GetTrainingProgramByNameAsync(string? name)
        {
            //If name null change to method GetAllTrainingProgramIsActiveAsync()
            if (name.IsNullOrEmpty())
            {
                return await GetAllAsync();
            }
            var result = await _trainingProgramService.GetTrainingProgramByNameAsync(name);
            return Ok(new BaseResponseModel()
            {
                Status = Ok().StatusCode,
                Message = "Get All Succeed",
                Result = new
                {
                    totalItemsCount = result.Count,
                    items = result
                }
            });
        }
        #endregion

        #region DeleteTrainingProgram
        [HttpPut("{id}")]
        public async Task<IActionResult> DeleteTrainingProgramsAsync(int id)
        {
            try
            {
                await _trainingProgramService.DeleteTrainingProgramsAsync(id);
                // return status codes with result according to user object
                return Ok(new BaseResponseModel
                {
                    Status = Ok().StatusCode,
                    Message = "Delete Succeed"
                });
            }
            catch (Exception ex)
            {
                // return status code bad request for validation
                return BadRequest(new BaseFailedResponseModel
                {
                    Status = BadRequest().StatusCode,
                    Message = ex.Message
                });
            }
        }
        #endregion

        #region UpdateTrainingProgram
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTrainingProgramsAsync(int id, UpdateTrainingProgramViewModel trainingProgramViewModels)
        {
            try
            {
                var result = await _trainingProgramService.UpdateTrainingProgramsAsync(id, trainingProgramViewModels);
                return Ok(new BaseResponseModel()
                {
                    Status = Ok().StatusCode,
                    Message = "Update Succeed",
                    Result = result
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new BaseFailedResponseModel()
                {
                    Status = BadRequest().StatusCode,
                    Message = "Update Failed",
                    Errors = ex.Message
                });
            }
        }
        #endregion UpdateTrainingProgram

        #region  Create Training Program + Add Syllabus
        //Add New Training Program
        [HttpPost]
        public async Task<IActionResult> CreateTrainingProgram(CreateTrainingProgramViewModels trainingProgram)
        {
            try
            {
                if (trainingProgram.Name.IsNullOrEmpty())
                    return BadRequest(new BaseFailedResponseModel {
                        Status = BadRequest().StatusCode,
                        Message = "The name TrainingProgram isn't empty."
                    });

                var result = await _trainingProgramService.CreateTrainingProgramAsync(trainingProgram);

                return Ok(new
                {
                    Success = "Created Successfully",
                    TrainingProgram = result
                });
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(new BaseFailedResponseModel
                {
                    Status = BadRequest().StatusCode,
                    Message = "Name duplication TrainingProgram.",
                    Errors = ex
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

        #endregion

        #region Get TrainingProgram By Date Range CreateOn
        [HttpPost]
        public async Task<IActionResult> GetTrainingProgramByDateRangeCreateOnAsync(DateTime[] fromToDate)
        {
            var result = await _trainingProgramService.GetTrainingProgramByDateRangeCreateOnAsync(fromToDate);
            return Ok(new BaseResponseModel()
            {
                Status = Ok().StatusCode,
                Message = "Get All Succeed",
                Result = new
                {
                    totalItemsCount = result.Count,
                    items = result
                }
            });
        }
        #endregion

        #region Get TrainingProgram By Date Range LastModify
        [HttpPost]
        public async Task<IActionResult> GetTrainingProgramByDateRangeLastModifyAsync(DateTime[] fromToDate)
        {
            var result = await _trainingProgramService.GetTrainingProgramByDateRangeLastModifyAsync(fromToDate);
            return Ok(new BaseResponseModel()
            {
                Status = Ok().StatusCode,
                Message = "Get All Succeed",
                Result = new
                {
                    totalItemsCount = result.Count,
                    items = result
                }
            });
        }
        #endregion

        #region Clone TrainingProgram
        [HttpPost("{id}")]
        public async Task<IActionResult> CloneTrainingProgramAsync(int id)
        {
            try
            {
                await _trainingProgramService.CloneTrainingProgramAsync(id);
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

        //Controller Action for Export Csv Template for Create TrainingProgram
        //path: api/TrainingProgram/ExportCsvFile
        #region ExportCsvFile
        [HttpPost]
        public IActionResult ExportCsvFile(string columnSeperator = ",")
        {
            if (columnSeperator == "-")
            {
                return BadRequest(new BaseFailedResponseModel
                {
                    Status = StatusCodes.Status400BadRequest,
                    Message = "Cannot use '-' to be the seperator in this situation."
                });
            }
            return File(_trainingProgramService.ExportCsvFile(columnSeperator), "text/csv", "template.csv");
        }
        #endregion

        #region Filter
        [HttpPost]
        public async Task<IActionResult> Filter(TrainingProgramFilterModel filter)
        {
            var result = await _trainingProgramService.Filter(filter);
            return Ok(new BaseResponseModel()
            {
                Status = Ok().StatusCode,
                Message = "Filter Succeed",
                Result = new
                {
                    totalItemsCount = result.Count,
                    items = result
                }
            });
        }
        #endregion

        //Controller Action for Import Csv File to Create TrainingProgram
        //path: api/TrainingProgram/ImportCsvFile?isScanName=false&duplicateHandle=1
        #region ImportCsvFile
        [HttpPost]
        public async Task<IActionResult> ImportCsvFile(IFormFile formFile,
            bool isScanName,
            DuplicateHandle duplicateHandle)
        {
            if (formFile == null || formFile.Length <= 0)
            {
                return NotFound(new BaseFailedResponseModel
                {
                    Status = StatusCodes.Status404NotFound,
                    Message = "Your file doesn't exists."
                });
            }

            if (!Path.GetExtension(formFile.FileName).Equals(".csv", StringComparison.OrdinalIgnoreCase))
            {
                return BadRequest(new BaseFailedResponseModel
                {
                    Status = StatusCodes.Status400BadRequest,
                    Message = "This file doesn't support."
                });
            }

            try
            {
                var listTrainingProgram = await _trainingProgramService.ImportCsvFileAsync(formFile, isScanName, duplicateHandle);
                return Ok(new BaseResponseModel
                {
                    Status = StatusCodes.Status200OK,
                    Message = "Succeed",
                    Result = new
                    {
                        TrainingPrograms = listTrainingProgram
                    }
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new BaseFailedResponseModel
                {
                    Status = StatusCodes.Status400BadRequest,
                    Message = "Import File Errors",
                    Errors = ex.Message
                });
            }
        }
        #endregion

        //Controller Action for Export Excel Template
        //path: api/TrainingProgram/ExportExcelFile
        #region ExportExcelFile
        [HttpGet]
        public async Task<IActionResult> ExportExcelFile() => File(await _trainingProgramService.ExportExcelFileAsync(),
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            "template.xlsx");
        #endregion

        //Controller Action for Import Excel Template
        //path: api/TrainingProgram/ImportExcelFile?isScanName=false&duplicateHandle=1
        #region ImportExcelFile
        [HttpPost]
        public async Task<IActionResult> ImportExcelFile(IFormFile formFile,
            bool isScanName,
            DuplicateHandle duplicateHandle)
        {
            if (formFile == null || formFile.Length <= 0)
            {
                return NotFound(new BaseFailedResponseModel
                {
                    Status = StatusCodes.Status404NotFound,
                    Message = "Your file doesn't exists."
                });
            }

            if (!Path.GetExtension(formFile.FileName).Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
            {
                return BadRequest(new BaseFailedResponseModel
                {
                    Status = StatusCodes.Status400BadRequest,
                    Message = "This file doesn't support."
                });
            }

            try
            {
                var listTrainingProgram = await _trainingProgramService.ImportExcelFileAsync(formFile, isScanName, duplicateHandle);
                return Ok(new BaseResponseModel
                {
                    Status = StatusCodes.Status200OK,
                    Message = "Succeed",
                    Result = new
                    {
                        TrainingPrograms = listTrainingProgram
                    }
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new BaseFailedResponseModel
                {
                    Status = StatusCodes.Status400BadRequest,
                    Message = "Import File Errors",
                    Errors = ex.Message
                });
            }
        }
        #endregion
    }
}