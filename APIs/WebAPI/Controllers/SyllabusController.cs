using Application.Interfaces;
using Application.ViewModels.SyllabusViewModels;
using Application.ViewModels.AddLectureViewModels;
using Microsoft.AspNetCore.Mvc;
using Application.ResponseModels;
using Application.ViewModels.LectureViewModels;
using Application.ViewModels.UnitViewModels;
using Domain.Enums.ResponeModelEnums;
using Application.Services;
using FluentValidation.AspNetCore;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class SyllabusController : ControllerBase
    {
        private readonly ISyllabusService _syllabusService;

        public SyllabusController(ISyllabusService syllabusService)
        {
            _syllabusService = syllabusService;
        }

        #region Controller GetSyllabusDetail

        // Controller action to get SyllabusDetail by Id
        // path: /api/Syllabus/GetSyllabusDetail/0

        [HttpGet("{syllabusId}")]
        public async Task<IActionResult> GetSyllabusDetail(int syllabusId)
        {
            try
            {
                var syllabusDetail = await _syllabusService.GetSyllabusDetailByIdAsync(syllabusId);
                return Ok(new BaseResponseModel()
                {
                    Status = Ok().StatusCode,
                    Message = "Success",
                    Result = syllabusDetail
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

        #endregion

        #region Controller SearchSyllabus

        [HttpGet("{searchString?}")]
        public async Task<IActionResult> GetSyllabusByNameAsync(string? searchString)
        {
            return Ok(new BaseResponseModel()
            {
                Status = Ok().StatusCode,
                Message = "Success",
                Result = await _syllabusService.GetSyllabusByNameAsync(searchString)
            });
        }

        [HttpPost]
        public async Task<IActionResult> GetSyllabusByDateRangeAsync(DateTime[] fromToDate)
        {
            return Ok(new BaseResponseModel()
            {
                Status = Ok().StatusCode,
                Message = "Success",
                Result = await _syllabusService.GetSyllabusByDateRangeAsync(fromToDate)
            });
        }

        #endregion

        #region Controller CloneSyllabus
        [HttpPost("{id}")]
        public async Task<IActionResult> CloneSyllabusAsync(int id)
        {
            try
            {
                await _syllabusService.CloneSyllabusAsync(id);
                return Ok(new BaseResponseModel()
                {
                    Status = Ok().StatusCode,
                    Message = "Success",
                    Result = "Success"
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
        #endregion

        #region Update

        [HttpPut("{syllabusId}")]
        public async Task<IActionResult> UpdateSyllabusAsync(UpdateSyllabusViewModel syllabusViewModel, int syllabusId)
        {
            try
            {
                var syllabus = await _syllabusService.UpdateSyllabusAsync(syllabusViewModel, syllabusId);
                if (syllabus == null)
                {
                    return NotFound(new BaseResponseModel
                    {
                        Status = NotFound().StatusCode,
                        Message = "Syllabus not found"
                    });
                }

                return Ok(new BaseResponseModel
                {
                    Status = Ok().StatusCode,
                    Message = "Update syllabus success",
                    Result = syllabus
                });
            }
            catch (Exception e)
            {
                return BadRequest(new BaseResponseModel
                {
                    Status = BadRequest().StatusCode,
                    Message = "Invalid parameters: " + e.Message,
                    Result = e
                });
            }
        }
        
        [HttpPut("{syllabusId}")]
        public async Task<IActionResult> UpdateUnitInSyllabusAsync(UnitUpdateViewModel unitUpdateViewModel, int syllabusId)
        {
            try
            {
                var syllabus = await _syllabusService.UpdateUnitInSyllabusAsync(unitUpdateViewModel, syllabusId);
                if (syllabus == null)
                {
                    return NotFound(new BaseResponseModel
                    {
                        Status = NotFound().StatusCode,
                        Message = "Syllabus not found"
                    });
                }

                return Ok(new BaseResponseModel
                {
                    Status = Ok().StatusCode,
                    Message = "Update syllabus success",
                    Result = syllabus
                });
            }
            catch (Exception e)
            {
                return BadRequest(new BaseResponseModel
                {
                    Status = BadRequest().StatusCode,
                    Message = "Invalid parameters: " + e.Message,
                    Result = e
                });
            }
        }
        
        [HttpPut("{syllabusId}")]
        public async Task<IActionResult> UpdateLectureInSyllabusAsync(UpdateLectureViewModel lectureUpdateViewModel, int syllabusId)
        {
            try
            {
                var syllabus = await _syllabusService.UpdateLectureInSyllabusAsync(lectureUpdateViewModel, syllabusId);
                if (syllabus == null)
                {
                    return NotFound(new BaseResponseModel
                    {
                        Status = NotFound().StatusCode,
                        Message = "Syllabus not found"
                    });
                }

                return Ok(new BaseResponseModel
                {
                    Status = Ok().StatusCode,
                    Message = "Update syllabus success",
                    Result = syllabus
                });
            }
            catch (Exception e)
            {
                return BadRequest(new BaseResponseModel
                {
                    Status = BadRequest().StatusCode,
                    Message = "Invalid parameters: " + e.Message,
                    Result = e
                });
            }
        }

        #endregion

        #region DeleteSyllabus
        [HttpPut("{id}")]
        public async Task<IActionResult> DeleteSyllabusAsync(int id)
        {
            try
            {
                var result = await _syllabusService.DeleteSyllabusAsync(id);
                return Ok(new BaseResponseModel()
                {
                    Status = Ok().StatusCode,
                    Message = "Delete Succeed",
                    Result = result
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest("Invalid parameters: " + ex.Message);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
        #endregion

        #region AddLecture
        [HttpPost]
        public async Task<IActionResult> AddLectureAsync(AddLectureViewModel lec)
        {
            try
            {
                var validateResult = await _syllabusService.ValidateAddLectureAsync(lec);

                if (validateResult.IsValid)
                {
                    var result = await _syllabusService.AddLectureAsync(lec);
                    return Ok(new BaseResponseModel()
                    {
                        Status = Ok().StatusCode,
                        Message = "Add Succeed",
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
            catch (ArgumentException ex)
            {
                return BadRequest("Invalid parameters: " + ex.Message);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
        #endregion

        #region AddLectureToUnit
        [HttpPost]
        public async Task<IActionResult> AddLectureToUnitAsync(int Lecture_ID, int Unit_ID)
        {
            try
            {
                var result = await _syllabusService.AddLectureToUnitAsync(Lecture_ID, Unit_ID);
                return Ok(new BaseResponseModel()
                {
                    Status = Ok().StatusCode,
                    Message = "Add Succeed",
                    Result = result
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest("Invalid parameters: " + ex.Message);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
        #endregion

        #region GetSyllabus
        [HttpGet]
        public async Task<IActionResult> GetSyllabusAsync()
        {
            return Ok(new BaseResponseModel()
            {
                Status = Ok().StatusCode,
                Message = "Get Syllabus successful",
                Result = await _syllabusService.GetSyllabusAsync()
            });
            //return Ok(await _syllabusService.GetSyllabusAsync());
        }
        #endregion

        #region GetAllOutputStandard
        [HttpGet]
        public async Task<IActionResult> GetAllOutputStandardsAsync()
        {
            return Ok(new BaseResponseModel()
            {
                Status = Ok().StatusCode,
                Message = "Get Syllabus successful",
                Result = await _syllabusService.GetOutputStandardsAsync()
            });
        }
        #endregion
        #region AddSyllabus
        [HttpPost]
        public async Task<IActionResult> AddSyllabusAsync(AddSyllabusViewModel syllabus)
        {
            try
            {
                var resultModel = await _syllabusService.AddSyllabusAsync(syllabus);
                return Ok(new BaseResponseModel()
                {
                    Status = Ok().StatusCode,
                    Message = "Add Syllabus successful",
                    Result = resultModel
                });
                //return Ok(await _syllabusService.AddSyllabusAsync(syllabus));
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        #endregion

        //Controller Action for Export Csv Template
        //path: api/Syllabus/ExportCsvFile
        #region ExportCsvFile
        [HttpPost]
        public IActionResult ExportCsvFile(string columnSeperator = ",") => File(_syllabusService.ExportCsvFile(columnSeperator), "text/csv", "template.csv");
        #endregion

        //Controller Action for Import Syllabus from Csv
        //path: api/Syllabus/ImportCsvFile?isScanCode=false&isScanName=false&duplicateHandle=1
        #region ImportCsvFile
        [HttpPost]
        public async Task<IActionResult> ImportCsvFileAsync(IFormFile formFile,
            bool isScanCode, bool isScanName,
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
                return Ok(new BaseResponseModel
                {
                    Status = StatusCodes.Status200OK,
                    Message = "Succeed",
                    Result = new
                    {
                        Syllabus = await _syllabusService.ImportCsvFileAsync(formFile, isScanCode, isScanName, duplicateHandle)
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
        //path: api/Syllabus/ExportExcelFile
        #region ExportExcelFile
        [HttpGet]
        public async Task<IActionResult> ExportExcelFile() => File(await _syllabusService.ExportExcelFileAsync(),
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            "template.xlsx");
        #endregion

        //Controller Action for Import Syllabus from Excel
        //path: api/Syllabus/ImportExcelFile?isScanCode=false&isScanName=false&duplicateHandle=1
        #region ImportExcelFile
        [HttpPost]
        public async Task<IActionResult> ImportExcelFile(IFormFile formFile,
            bool isScanCode, bool isScanName,
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
                return Ok(new BaseResponseModel
                {
                    Status = StatusCodes.Status200OK,
                    Message = "Succeed",
                    Result = new
                    {
                        Syllabus = await _syllabusService.ImportExcelFileAsync(formFile, isScanCode, isScanName, duplicateHandle)
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