using Application.Interfaces;
using Application.ResponseModels;
using Application.Utils;
using Application.ViewModels.UserViewModels;
using Domain.Enums.ResponeModelEnums;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        // Controller action to login
        // path: /api/User/Login
        #region LoginAsync
        [HttpPost]
        public async Task<IActionResult> LoginAsync(UserLoginModel userLogin)
        {
            var token = await _userService.LoginAsync(userLogin);
            if (!string.IsNullOrEmpty(token))
                return Ok(new BaseResponseModel
                {
                    Status = StatusCodes.Status200OK,
                    Message = "Login Succeed.",
                    Result = new
                    {
                        Token = token,
                        User = await _userService.GetUserDetailByEmailAsync(userLogin.Email)
                    }
                });
            return NotFound(new BaseFailedResponseModel
            {
                Status = StatusCodes.Status400BadRequest,
                Message = "Incorrect Email or Password."
            });
        }
        #endregion

        // Controller action to get user's information by id
        // path: /api/User/Detail/1
        #region Detail
        [HttpGet("{id:int}")]
        public async Task<IActionResult> Detail(int id)
        {
            try
            {
                var user = await _userService.GetUserDetailAsync(id);

                // return status codes with result according to user object
                return (user == null) ? NotFound(new BaseFailedResponseModel
                {
                    Status = StatusCodes.Status404NotFound,
                    Message = "User not found.",
                })
                    :
                Ok(new BaseResponseModel
                {
                    Status = StatusCodes.Status200OK,
                    Message = "Succeed.",
                    Result = new
                    {
                        User = user
                    }
                });
            }
            catch (ArgumentException ex)
            {
                // return status code bad request for validation
                return BadRequest(new BaseFailedResponseModel
                {
                    Status = StatusCodes.Status400BadRequest,
                    Message = "Invalid parameters.",
                    Errors = ex.Message
                });
            }
        }
        #endregion

        // Controller action to get users list with pagination
        // path: /api/User/ListPagination?pageIndex=0&pageSize=10
        #region ListPagination
        [HttpGet]
        [Route("/api/User/ListPagination")]
        public async Task<IActionResult> List(int pageIndex = 0, int pageSize = 10)
        {
            try
            {
                var usersList = await _userService.GetUserListPaginationAsync(pageIndex, pageSize);

                if (usersList == null)
                {
                    return NotFound(new BaseFailedResponseModel
                    {
                        Status = StatusCodes.Status404NotFound,
                        Message = "Cannot Found This List."
                    });
                }

                return (usersList.Items.Count == 0) ?
                    NoContent()
                    :
                    Ok(new BaseResponseModel
                    {
                        Status = StatusCodes.Status200OK,
                        Message = "Succeed.",
                        Result = usersList
                    });
            }
            catch (ArgumentException ex)
            {
                // return status code bad request for validation
                return BadRequest(new BaseFailedResponseModel
                {
                    Status = StatusCodes.Status400BadRequest,
                    Message = "Invalid parameters.",
                    Errors = ex.Message
                });
            }
        }
        #endregion

        // Controller action to get users list contains all users
        // path: /api/User/List
        #region List
        [HttpGet]
        public async Task<IActionResult> ListAsync()
        {
            try
            {
                var users = await _userService.GetUserListAsync();

                return Ok(new BaseResponseModel
                {
                    Status = StatusCodes.Status200OK,
                    Message = "Succeed.",
                    Result = new
                    {
                        Users = users
                    }
                });
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
            catch (Exception ex)
            {
                // return status code bad request for validation
                return BadRequest(new BaseFailedResponseModel
                {
                    Status = StatusCodes.Status400BadRequest,
                    Message = "Internal Server Error",
                    Errors = ex.Message
                });
            }
        }
        #endregion

        //Controller action to get create options
        //path: /api/User/GetCreateOptions
        #region GetCreateOptions
        [HttpGet]
        public async Task<IActionResult> GetCreateOptionsAsync()
        {
            var options = await _userService.GetCreateOptionsAsync();
            if (options == null) return NotFound(new BaseFailedResponseModel
            {
                Status = StatusCodes.Status404NotFound,
                Message = "Cannot Get Create Options.",
            });
            return Ok(new BaseResponseModel
            {
                Status = StatusCodes.Status200OK,
                Message = "Succeed.",
                Result = options
            });
        }
        #endregion

        //Controller action to create new user
        //path: /api/User/CreateUser
        #region CreateUserAsync
        [HttpPost]
        public async Task<IActionResult> CreateUserAsync(UserCreateModel createdUser)
        {
            createdUser.Status = createdUser.Status.RegenerateStringFormat();

            var result = await _userService.ValidateCreateUserAsync(createdUser);

            if (result.IsValid)
            {
                if (await _userService.IsExistsUserAsync(createdUser.Email))
                {
                    return BadRequest(new BaseFailedResponseModel
                    {
                        Status = StatusCodes.Status400BadRequest,
                        Message = "This email is already exists.",
                    });
                }

                if (await _userService.CreateUserAsync(createdUser))
                {
                    return Created("/api/User/List", new BaseResponseModel
                    {
                        Status = StatusCodes.Status201Created,
                        Message = "User Created Succeed",
                        Result = new { Users = createdUser }
                    });
                }

                return NotFound(new BaseFailedResponseModel
                {
                    Status = StatusCodes.Status400BadRequest,
                    Message = "Created Unsuccessfully."
                });
            }

            result.AddToModelState(ModelState);

            return BadRequest(new BaseFailedResponseModel
            {
                Status = StatusCodes.Status400BadRequest,
                Message = "User Validated Failed",
                Errors = result.Errors
            });
        }
        #endregion

        //Controller action to import excel file for create user
        //path: /api/User/ImportUser
        #region ImportUserAsync
        [HttpPost]
        public async Task<IActionResult> ImportUserAsync(IFormFile formFile, bool isScanFullName, bool isScanEmail, DuplicateHandle duplicateHandle)
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
                var list = await _userService.ImportCsvFileAsync(formFile, isScanFullName, isScanEmail, duplicateHandle);

                if (list == null)
                {
                    return BadRequest(new BaseFailedResponseModel
                    {
                        Status = StatusCodes.Status400BadRequest,
                        Message = "Import File Unsucceed"
                    });
                }

                return Created("UserDetailsAsync",
                    new BaseResponseModel
                    {
                        Status = StatusCodes.Status201Created,
                        Message = "Import File Succeed",
                        Result = new { Users = list }
                    });
            }
            catch (Exception ex)
            {
                return BadRequest(new BaseFailedResponseModel
                {
                    Status = StatusCodes.Status400BadRequest,
                    Message = "Import File Errrors",
                    Errors = ex.Message
                });
            }
        }
        #endregion

        //Controller Action to Export Csv Template File.
        //path: /api/User/ExportCsvFile
        #region ExportCsvFile
        [HttpPost]
        public IActionResult ExportCsvFile(string columnSeperator) => File(_userService.ExportCsvFile(columnSeperator), "text/csv", "template.csv");
        #endregion

        #region EditUserAsync
        [HttpPut("{id}")]
        public async Task<IActionResult> EditAsync(int id, [FromBody] UserUpdateModel userUpdateModel)
        {

            // FromBody: Input in the correct format
            //  - ID: int and must exist (1 - 20 in the current database)
            //  - DateOfBirth: DateTime
            //  - IsMale: true/false
            //  - Role: ClassAdmin/SuperAdmin/Trainee/Auditor/Trainee
            //  - Level: A/B/C
            //  - Status: InClass/OffClass/Onboarding
            //  - AvatarURL can be null

            if (id != userUpdateModel.Id)
            {
                return BadRequest(new BaseFailedResponseModel()
                {
                    Status = BadRequest().StatusCode,
                    Message = "IDs do not match!"
                });
            }

            var user = await _userService.GetUserByIdAsync(userUpdateModel.Id);
            if (user == null) return NotFound(new BaseFailedResponseModel()
            {
                Status = NotFound().StatusCode,
                Message = "User with ID " + userUpdateModel.Id + " does not exist!"
            });

            var validate = await _userService.ValidateUpdateUserAsync(userUpdateModel);
            if (validate.IsValid)
            {
                var result = await _userService.EditAsync(userUpdateModel, user);
                return result ? Ok(new BaseResponseModel()
                {
                    Status = Ok().StatusCode,
                    Message = "User with ID " + userUpdateModel.Id + " has been updated!"
                }) : BadRequest(new BaseFailedResponseModel()
                {
                    Status = BadRequest().StatusCode,
                    Message = "Update failed!"
                });
            }

            // result = true if all input are correct and the user gets updated successfully, false if otherwise

            validate.AddToModelState(ModelState);

            return BadRequest(new BaseFailedResponseModel()
            {
                Status = BadRequest().StatusCode,
                Message = "Validation error!",
                Errors = ModelState
            });

            // false if validation error
        }
        #endregion

        #region DeleteUserAsync
        [HttpPut("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {

            var user = await _userService.GetUserByIdAsync(id);
            if (user == null) return NotFound(new BaseFailedResponseModel()
            {
                Status = NotFound().StatusCode,
                Message = "User with ID " + id + " does not exist!"
            });

            var result = await _userService.DeleteAsync(user);
            return result ? Ok(new BaseResponseModel()
            {
                Status = Ok().StatusCode,
                Message = "User with ID " + id + " has been set to Deleted!"
            }) : BadRequest(new BaseFailedResponseModel()
            {
                Status = BadRequest().StatusCode,
                Message = "Deletion failed!"
            });
        }
        #endregion

        //Controller action to Generate Reset Password Token when User is Forgot Password.
        //path: /api/User/ForgotPassword
        #region ForgotPasswordAsync
        [HttpPost]
        public async Task<IActionResult> ForgotPasswordAsync(string email)
        {
            if (await _userService.IsExistsUserAsync(email) == false) return NotFound(new BaseFailedResponseModel
            {
                Status = StatusCodes.Status404NotFound,
                Message = "Email not found."
            });
            var token = await _userService.ForgotPasswordAsync(email);
            if (token == null)
                return StatusCode(StatusCodes.Status500InternalServerError, new BaseFailedResponseModel
                {
                    Status = StatusCodes.Status500InternalServerError,
                    Message = "Save Token Unsucceed."
                });
            if (!await _userService.HasPasswordTokenMailSentAsync(email, token))
                return StatusCode(StatusCodes.Status500InternalServerError, new BaseFailedResponseModel
                {
                    Status = StatusCodes.Status500InternalServerError,
                    Message = "Mail Sent Failed."
                });
            return Ok(new BaseResponseModel
            {
                Status = StatusCodes.Status200OK,
                Message = "Password token mail has been sent."
            });
        }
        #endregion

        //Controlle action to Reset User Password
        //path: /api/User/ResetPassword
        #region ResetPasswordAsync
        [HttpPost]
        public async Task<IActionResult> ResetPasswordAsync(UserResetPasswordModel model)
        {
            if (_userService.IsExpiredPasswordToken(model.Token)) return BadRequest(new BaseFailedResponseModel
            {
                Status = StatusCodes.Status400BadRequest,
                Message = "Reset Token is expired."
            });
            if (!await _userService.IsExistsUserAsync(model.Email)) return NotFound(new BaseFailedResponseModel
            {
                Status = StatusCodes.Status404NotFound,
                Message = "Email not found."
            });
            var isValidToken = await _userService.IsValidPasswordTokenAsync(model.Email, model.Token);
            if (!isValidToken) return BadRequest(new BaseFailedResponseModel
            {
                Status = StatusCodes.Status400BadRequest,
                Message = "Reset Token is incorrect."
            });
            if (await _userService.ChangePasswordAsync(model.Email, model.NewPassword))
                return Ok(new BaseResponseModel
                {
                    Status = StatusCodes.Status200OK,
                    Message = "Password reset succeed."
                });
            return StatusCode(StatusCodes.Status500InternalServerError,
                new BaseFailedResponseModel
                {
                    Status = StatusCodes.Status500InternalServerError,
                    Message = "Save new password failed."
                });
        }
        #endregion

        //Controller Action to get List of User by its Role.
        //path: /api/User/GetUsersByRole/Super%20Admin
        #region GetUsersByRoleAsync
        [HttpGet("{roleName}")]
        public async Task<IActionResult> GetUsersByRoleAsync(string roleName)
        {
            var userList = await _userService.GetUsersByRoleAsync(roleName);
            return userList.Count == 0 ?
                NotFound(new BaseFailedResponseModel
                {
                    Status = StatusCodes.Status404NotFound,
                    Message = "Could not found any user."
                })
                :
                Ok(new BaseResponseModel
                {
                    Status = StatusCodes.Status200OK,
                    Message = "Succeed.",
                    Result = new
                    {
                        Users = userList
                    }
                });
        }
        #endregion

        #region Filter
        [HttpPost]
        public async Task<IActionResult> Filter(UserFilterModel filter)
        {
            try
            {
                var result = await _userService.Filter(filter);
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

        //Controller Action to Validate User Token for Authenticate
        //path: /api/User/ValidateToken
        #region ValidateToken
        [HttpPost]
        public IActionResult ValidateToken([FromForm] string token)
        {
            if (!string.IsNullOrEmpty(token))
            {
                if (_userService.IsValidToken(token))
                {
                    return Ok(new BaseResponseModel
                    {
                        Status = StatusCodes.Status200OK,
                        Message = "Valid User"
                    });
                }
            }

            return BadRequest(new BaseFailedResponseModel
            {
                Status = StatusCodes.Status400BadRequest,
                Message = "User Token is not valid"
            });
        }
        #endregion

        //Controller Action to Remove Login Token in Session
        //path: /api/User/Logout
        #region Logout
        [HttpPost]
        public IActionResult Logout()
        {
            if (_userService.Logout())
            {
                return Ok(new BaseResponseModel
                {
                    Status = StatusCodes.Status200OK,
                    Message = "Succeed"
                });
            }

            return BadRequest(new BaseFailedResponseModel
            {
                Status = StatusCodes.Status400BadRequest,
                Message = "Logout Failed"
            });
        }
        #endregion

        //Controller Action to Export Excel Template File
        //path: /api/User/ExportExcelFile
        #region ExportExcelFile
        [HttpGet]
        public async Task<IActionResult> ExportExcelFile() => File(await _userService.ExportExcelFile(),
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            "template.xlsx");
        #endregion

        //Controller Action to Import User From Excel File.
        //path: /api/User/ImportExcelFile
        #region ImportExcelFile
        [HttpPost]
        public async Task<IActionResult> ImportExcelFileAsync(IFormFile formFile,
            bool isScanFullName, bool isScanEmail, 
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
                var list = await _userService.ImportExcelFileAsync(formFile, isScanFullName, isScanEmail, duplicateHandle);

                if (list == null)
                {
                    return BadRequest(new BaseFailedResponseModel
                    {
                        Status = StatusCodes.Status400BadRequest,
                        Message = "Import File Unsucceed"
                    });
                }

                return Created("UserDetailsAsync",
                    new BaseResponseModel
                    {
                        Status = StatusCodes.Status201Created,
                        Message = "Import File Succeed",
                        Result = new { Users = list }
                    });
            }
            catch (Exception ex)
            {
                return BadRequest(new BaseFailedResponseModel
                {
                    Status = StatusCodes.Status400BadRequest,
                    Message = "Import File Errrors",
                    Errors = ex.Message
                });
            }
        }
        #endregion
    }
}