using Application.Commons;
using Application.Interfaces;
using Application.IValidators;
using Application.Utils;
using Application.ViewModels.MailViewModels;
using Application.ViewModels.UserViewModels;
using AutoMapper;
using Domain.Enums.UserEnums;
using Domain.Models.Users;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Domain.Enums.ResponeModelEnums;
using CsvHelper.Configuration;
using System.Globalization;
using CsvHelper;
using System.Reflection;
using OfficeOpenXml;
using System.IO;

namespace Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentTime _currentTime;
        private readonly IConfiguration _configuration;
        private readonly IEmailService _emailService;
        private readonly IUserValidator _userValidator;
        private readonly ISessionServices _sessionServices;
        private readonly IClaimsService _claimsService;

        // adding mapper in user service using DI
        private readonly IMapper _mapper;

        public UserService(IUnitOfWork unitOfWork, ICurrentTime currentTime,
            IConfiguration configuration, IEmailService emailService,
            IUserValidator userValidator, ISessionServices sessionServices,
            IClaimsService claimsService, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _currentTime = currentTime;
            _configuration = configuration;
            _emailService = emailService;
            _userValidator = userValidator;
            _sessionServices = sessionServices;
            _claimsService = claimsService;
            _mapper = mapper;
        }

        // implement method GetUserByIdAsync
        #region GetUserDetailAsync
        public async Task<UserDetailModel?> GetUserDetailAsync(int id)
        {
            // validate user id
            // id cannot be less than 1
            if (id <= 0)
            {
                string msg = "User id cannot be less than 1. Input user id: " + id;

                // Log error message to console
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid parameters: " + msg);

                // throw argument exception with error message
                throw new ArgumentException(msg);
            }
            // retrieve user with the id
            var user = await _unitOfWork.UserRepo.GetByIdAsync(id);

            // map user entity to user detail model
            var result = _mapper.Map<UserDetailModel>(user);

            return result;
        }
        #endregion

        #region GetUserListPaginationAsync
        // implement method GetUserListPaginationAsync
        public async Task<Pagination<UserListModel>> GetUserListPaginationAsync(int pageIndex = 0, int pageSize = 10)
        {
            // Validate page index
            // Page index must begin at 0
            if (pageIndex < 0)
            {
                string msg = "Page index cannot be less than 0. Input page index: " + pageIndex;

                // Log error message to console
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid parameters: " + msg);

                // throw argument exception with error message
                throw new ArgumentException(msg);
            }

            // Validate page size
            // Page must contain at least one item
            if (pageSize <= 0)
            {
                string msg = "Page size cannot be less than 1. Input page size: " + pageSize;

                // Log error message to console
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid parameters: " + msg);

                // throw argument exception with error message
                throw new ArgumentException(msg);
            }

            // Get paginated user entities list
            var users = await _unitOfWork.UserRepo.ToPaginationAsync(pageIndex, pageSize);

            // Map user entities to user list model
            var result = _mapper.Map<Pagination<UserListModel>>(users);

            return result;

        }
        #endregion

        #region EditUserAsync
        public async Task<User> GetUserByIdAsync(int id) => await _unitOfWork.UserRepo.GetByIdAsync(id);

        public async Task<ValidationResult> ValidateUpdateUserAsync(UserUpdateModel updatedUser)
        {
            return await _userValidator.UserUpdateValidator.ValidateAsync(updatedUser);
        }

        public async Task<bool> EditAsync(UserUpdateModel userUpdateModel, User user)
        {
            // Map UserUpdateModel to User
            _mapper.Map(userUpdateModel, user, typeof(UserUpdateModel), typeof(User));

            // Update User
            _unitOfWork.UserRepo.Update(user);

            // SaveChanges
            var saveResult = await _unitOfWork.SaveChangesAsync();

            // Return true if SaveChanges was success, false otherwise
            return (saveResult > 0) ? true : false;

        }
        #endregion

        #region DeleteUserAsync
        public async Task<bool> DeleteAsync(User user)
        {
            // Delete User
            user.isDeleted = true;
            _unitOfWork.UserRepo.Update(user);

            // SaveChanges
            var saveResult = await _unitOfWork.SaveChangesAsync();

            // Return true if SaveChanges was success, false otherwise
            return (saveResult > 0) ? true : false;
        }

        #endregion

        #region LoginAsync
        public async Task<string> LoginAsync(UserLoginModel userLogin)
        {
            var user = await _unitOfWork.UserRepo.GetUserByUserEmailAndPasswordHash(userLogin.Email, userLogin.Password.Hash());
            if (user != null)
            {
                var token = user.GenerateJsonWebToken(_configuration["Jwt:Key"]!, _currentTime.GetCurrentTime(), _configuration);
                _sessionServices.SaveToken(user.Id, token);
                return token;
            };

            return string.Empty;
        }
        #endregion

        #region ValidateCreateUser
        /// <summary>
        /// Method for validating a user create model
        /// </summary>
        /// <param name="createdUser"> A User Create Model </param>
        /// <returns> return an Validate Result after validating </returns>
        public async Task<ValidationResult> ValidateCreateUserAsync(UserCreateModel createdUser)
        {
            return await _userValidator.UserCreateValidator.ValidateAsync(createdUser);
        }
        #endregion

        //Implement IsExistsUser
        #region IsExistsUser
        public async Task<bool> IsExistsUserAsync(string email)
        {
            return await _unitOfWork.UserRepo.IsExistsUser(email);
        }
        #endregion

        //Implement CreateUserAsync
        #region CreateUserAsync
        public async Task<bool> CreateUserAsync(UserCreateModel createdUser)
        {
            //Genereate password from email and date of birth
            var password = GenerateUserPassword(createdUser.Email, createdUser.DateOfBirth);
            var user = _mapper.Map<User>(createdUser);

            //Assign roleId for create user
            user.RoleId = (await _unitOfWork.RoleRepo.GetRoleByNameAsync(createdUser.Role)).Id;

            //Assign hash password for create user
            user.Password = password.Hash();
            await _unitOfWork.UserRepo.AddAsync(user);
            var result = await _unitOfWork.SaveChangesAsync() > 0;

            if (result)
            {
                return await SendMailCreateUserAsync(createdUser.Email, password);
            }
            return false;
        }
        #endregion

        //Implement SendMailCreateUserAsync
        #region SendMailCreateUserAsync
        /// <summary>
        /// Method for sending email after user is created
        /// </summary>
        /// <param name="email"> an user email </param>
        /// <param name="password"> an user password </param>
        /// <returns> return true if send mail successfully, otherwise false </returns>
        public async Task<bool> SendMailCreateUserAsync(string email, string password)
        {
            var to = new List<string> { email };
            var title = "Welcome to FPT Software Academy";
            var speech = "Greetings,\nWelcome to FPT Software Academy and thank you for joining our course. This is your login password:";
            var mainContent = password;
            var sign = "FPT Academy Team";
            var body = _emailService.GetMailBody(title, speech, mainContent, sign: sign);
            var subject = "Welcome to FPT Software Academy";
            var mailData = new MailDataModel(to, subject, body);
            return await _emailService.SendAsync(mailData, new CancellationToken());
        }
        #endregion

        //Implement GetCreateOptions
        #region GetCreateOptions
        public async Task<UserCreateOptions> GetCreateOptionsAsync()
        {
            var levels = new List<string>();
            var statuses = new List<string>();

            var roles = await _unitOfWork.RoleRepo.GetAllAsync();

            var roleName = roles.Select(x => x.Name).ToList();

            levels = Enum.GetValues(typeof(UserLevel))
                .Cast<UserLevel>()
                .Select(x => x.ToString())
                .ToList();

            statuses = Enum.GetValues(typeof(UserStatus))
                .Cast<UserStatus>()
                .Select(x => x.ToString())
                .ToList();

            return new UserCreateOptions
            {
                Roles = roleName,
                Levels = levels,
                Statuses = statuses
            };
        }
        #endregion

        #region GenerateUserPassword
        private string GenerateUserPassword(string email, DateTime dob)
        {
            return email.Substring(0, email.IndexOf("@")) +
                dob.ToString("dd/MM").Replace(@"/", "");
        }
        #endregion

        //Implement ImportFileAsync
        #region ImportCsvFileAsync
        public async Task<List<UserCreateModel>?> ImportCsvFileAsync(IFormFile formFile,
            bool isScanFullName,
            bool isScanEmail,
            DuplicateHandle duplicateHandle)
        {
            //Create a List of User that Read from CSV File.
            var userListFromCsv = new List<UserCreateModel>();

            #region ReadCsvFile
            var fileName = Guid.NewGuid().ToString() + ".csv";
            var binPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var filePath = Path.Combine(binPath!, fileName);

            using (FileStream fs = File.Create(filePath))
            {
                await formFile.CopyToAsync(fs);
            }

            using (var streamReader = new StreamReader(filePath))
            {
                var csvConfig = new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    HasHeaderRecord = true,
                    Comment = '#',
                    AllowComments = true,
                };

                var csv = new CsvReader(streamReader, csvConfig);

                try
                {
                    var userRecords = csv.GetRecords<dynamic>();

                    foreach (var record in userRecords)
                    {
                        if (string.IsNullOrWhiteSpace(record.Name))
                        {
                            break;
                        }

                        UserCreateModel createdUser = new UserCreateModel
                        {
                            FullName = record.Name,
                            Email = record.Email,
                            Phone = record.Phone,
                            DateOfBirth = DateTime.Parse(record.Date_of_Birth),
                            Role = record.Role,
                            Level = record.Level,
                            Status = record.Status,
                            IsMale = (record.Gender == "Male"),
                            AvatarURL = "temp.png"
                        };

                        if (!await _unitOfWork.RoleRepo.IsExistRoleAsync(createdUser.Role))
                        {
                            streamReader.Dispose();
                            File.Delete(filePath);
                            throw new Exception($"Error at line {csv.Context.Parser.Row}, ColumnName: {csv.HeaderRecord?[csv.GetFieldIndex("Role")]}, Error: This role doesn't exists or is incorrect format. Example format: Super Admin, Class Admin,...");
                        }

                        if (!Enum.IsDefined(typeof(UserLevel), createdUser.Level))
                        {
                            streamReader.Dispose();
                            File.Delete(filePath);
                            throw new Exception($"Error at line {csv.Context.Parser.Row}, ColumnName: {csv.HeaderRecord?[csv.GetFieldIndex("Level")]}, Error: This level doesn't exists or is incorrect format. Example format: AA, AB,...");
                        }

                        createdUser.Status = createdUser.Status.RegenerateStringFormat();

                        if (!Enum.IsDefined(typeof(UserStatus), createdUser.Status))
                        {
                            streamReader.Dispose();
                            File.Delete(filePath);
                            throw new Exception($"Error at line {csv.Context.Parser.Row}, ColumnName: {csv.HeaderRecord?[csv.GetFieldIndex("Status")]}, Error: This status doesn't exsists or is incorrect format. Example format: In Class,...");
                        }

                        var result = _userValidator.UserCreateValidator.Validate(createdUser);

                        if (!result.IsValid)
                        {
                            var error = $"Error at line {csv.Context.Parser.Row}: ";
                            foreach (var failure in result.Errors)
                            {
                                error += failure + "\n";
                            }
                            streamReader.Dispose();
                            File.Delete(filePath);
                            throw new Exception(error);
                        }

                        if (!isScanEmail && (duplicateHandle != DuplicateHandle.Skip ||
                             (!await _unitOfWork.UserRepo.IsExistNameAsync(createdUser.FullName) &&
                             duplicateHandle == DuplicateHandle.Skip)))
                        {
                            if (await _unitOfWork.UserRepo.IsExistsUser(createdUser.Email))
                            {
                                streamReader.Dispose();
                                File.Delete(filePath);
                                throw new Exception($"Error at line {csv.Context.Parser.Row}, ColumnName: {csv.HeaderRecord?[csv.GetFieldIndex("Email")]}, Error: This email has already exist.");
                            }
                        }

                        userListFromCsv.Add(createdUser);
                    }
                }
                catch (CsvHelperException)
                {
                    streamReader.Dispose();
                    File.Delete(filePath);
                    throw new Exception($"Error at line {csv.Context.Parser.Row}, " +
                        $"Column: {csv.CurrentIndex}, " +
                        $"Column Name: {csv.HeaderRecord?[csv.CurrentIndex]}");
                }
            }

            File.Delete(filePath);
            #endregion

            #region Insert Users to Database
            //Filter User List by The Scanning Options
            var userListArray = await ImportScanningHandle(userListFromCsv, isScanFullName, isScanEmail, duplicateHandle);
            userListFromCsv = userListArray[0];

            //Get User Update List if Scanning Option is 'Replace'.
            var userUpdateList = userListArray[1];

            //Create a List of Add User.
            var createUsers = _mapper.Map<List<UserCreateModel>, List<User>>(userListFromCsv);

            //Create and Update a List of Edit User if User Update List is not empty.
            if (userUpdateList.Count != 0)
            {
                var updateUsers = new List<User>();
                foreach (var user in userUpdateList)
                {
                    if (!isScanFullName)
                    {
                        //Create a List of Update User Scanning By Email.
                        #region Replace When Scanning Email
                        var tempUser = await _unitOfWork.UserRepo.GetUserByEmailAsync(user.Email);
                        _mapper.Map(user, tempUser);
                        tempUser.RoleId = (await _unitOfWork.RoleRepo.GetRoleByNameAsync(user.Role)).Id;
                        updateUsers.Add(tempUser);
                        #endregion
                    }
                }
                _unitOfWork.UserRepo.UpdateRange(updateUsers);
            }

            //Create a List of User Password.
            var userPasswords = new List<string>();

            //Generate User Password.
            foreach (var user in createUsers)
            {
                var password = GenerateUserPassword(user.Email, user.DateOfBirth);
                userPasswords.Add(password);
                user.Password = password.Hash();
                user.RoleId = (await _unitOfWork.RoleRepo
                    .GetRoleByNameAsync(userListFromCsv[createUsers.IndexOf(user)].Role))
                    .Id;
            }

            await _unitOfWork.UserRepo.AddRangeAsync(createUsers);

            if (await _unitOfWork.SaveChangesAsync() >= createUsers.Count)
            {
                foreach (var user in createUsers)
                {
                    await SendMailCreateUserAsync(user.Email, userPasswords[createUsers.IndexOf(user)]);
                }
                return userListFromCsv;
            }

            return null;
            #endregion
        }
        #endregion

        //Implement ExportCsvFile
        #region ExportCsvFile
        public byte[] ExportCsvFile(string columnSeperator)
        {
            string[] columnNames = new string[] { "Name", "Email", "Phone", "Date_of_Birth", "Role", "Level", "Status", "Gender" };
            string csv = string.Empty;

            foreach (var column in columnNames)
            {
                csv += column + columnSeperator;
            }

            csv += "\n#Example Template: Insert User with this format (Must Insert with Correct Format.)\n" +
                "#Name, #email@example.com, #09xxxxxxxx (x is number), #01/01/2000 (dd/MM/yyyy), #Super Admin (Trainee...), #AA (AB...), #In Class (Off Class...), #Male or Female";
            csv.Replace(",", columnSeperator);

            csv += "\r\n";

            return Encoding.UTF8.GetBytes(csv);
        }
        #endregion

        //Implement ImportScanningHandle
        #region ImportScanningHandle
        public async Task<List<UserCreateModel>[]> ImportScanningHandle(List<UserCreateModel> users, bool isScanFullName,
            bool isScanEmail, DuplicateHandle duplicateHandle)
        {
            List<UserCreateModel> updateUser = new List<UserCreateModel>();
            switch (duplicateHandle)
            {
                case DuplicateHandle.Allow:
                    if (isScanEmail)
                    {
                        throw new Exception("Email cannot be duplicated.");
                    }
                    break;
                case DuplicateHandle.Replace:
                    if (isScanFullName)
                    {
                        throw new Exception("Cannot Replace User with Full Name.");
                    }
                    if (isScanEmail)
                    {
                        foreach (var user in users)
                        {
                            if (await _unitOfWork.UserRepo.IsExistsUser(user.Email))
                            {
                                updateUser.Add(user);
                            }
                        }
                        users = users.Except(updateUser).ToList();
                    }
                    break;
                case DuplicateHandle.Skip:
                    if (isScanFullName)
                    {
                        foreach (var user in users)
                        {
                            if (await _unitOfWork.UserRepo.IsExistNameAsync(user.FullName))
                            {
                                updateUser.Add(user);
                            }
                        }
                        users = users.Except(updateUser).ToList();
                    }
                    if (isScanEmail)
                    {
                        foreach (var user in users)
                        {
                            if (await _unitOfWork.UserRepo.IsExistsUser(user.Email))
                            {
                                updateUser.Add(user);
                            }
                        }
                        users = users.Except(updateUser).ToList();
                    }
                    updateUser.Clear();
                    break;
            }

            return new List<UserCreateModel>[] { users, updateUser };
        }
        #endregion

        #region GetUserListAsync
        public async Task<List<UserListModel>> GetUserListAsync()
        {
            var users = await _unitOfWork.UserRepo.GetAllAsync();

            if (users.Count == 0 || users.IsNullOrEmpty())
            {
                throw new InvalidOperationException();
            }

            var result = _mapper.Map<List<UserListModel>>(users);

            return result;

        }
        #endregion

        #region GeneratePasswordToken
        public string GeneratePasswordToken(User user)
        {
            return user.GenerateJsonWebTokenCustomExpireMinute(user.Password, _currentTime.GetCurrentTime(), minutes: 60 * 24, _configuration);
        }
        #endregion

        #region ForgotPasswordAsync
        public async Task<string?> ForgotPasswordAsync(string email)
        {
            User user = await _unitOfWork.UserRepo.GetUserByEmailAsync(email);
            var token = GeneratePasswordToken(user);
            user.ResetToken = token;
            _unitOfWork.UserRepo.Update(user);
            if (await _unitOfWork.SaveChangesAsync() > 0) return token;
            return null;
        }
        #endregion

        #region IsValidPasswordTokenAsync
        public async Task<bool> IsValidPasswordTokenAsync(string email, string token)
        {
            User user = await _unitOfWork.UserRepo.GetUserByEmailAsync(email);
            if (user.ResetToken != token) return false;
            return true;
        }
        #endregion

        #region ChangePasswordAsync
        public async Task<bool> ChangePasswordAsync(string email, string newPassword)
        {
            User user = await _unitOfWork.UserRepo.GetUserByEmailAsync(email);
            user.Password = newPassword.Hash();
            _unitOfWork.UserRepo.Update(user);
            if (await _unitOfWork.SaveChangesAsync() > 0) return true;
            return false;
        }
        #endregion

        #region IsExpiredPasswordToken
        public bool IsExpiredPasswordToken(string token)
        {
            return token.IsExpiredToken(_currentTime.GetCurrentTime());
        }
        #endregion

        #region HasPasswordTokenMailSentAsync
        public async Task<bool> HasPasswordTokenMailSentAsync(string email, string token)
        {
            var to = new List<string> { email };
            var title = "Reset Password";
            var speech = "This is your reset password token";
            var mainContent = token;
            var sign = "FPT Academy Team";
            var body = _emailService.GetMailBody(title, speech, mainContent, sign: sign);
            var subject = "Reset Password";
            var mailData = new MailDataModel(to, subject, body);
            return await _emailService.SendAsync(mailData, new CancellationToken());
        }
        #endregion

        //Implement GetUserByEmailAsync
        #region GetUserByEmailAsync
        public async Task<UserDetailModel> GetUserDetailByEmailAsync(string email)
        {
            return _mapper.Map<UserDetailModel>(await _unitOfWork.UserRepo.GetUserByEmailAsync(email));
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _unitOfWork.UserRepo.GetUserByEmailAsync(email);
        }
        #endregion

        //Implement GetUsersByRoleAsync
        #region GetUsersByRoleAsync
        public async Task<List<UserDetailModel>> GetUsersByRoleAsync(string roleName)
        {
            return _mapper.Map<List<UserDetailModel>>(await _unitOfWork.UserRepo.GetUserByRoleAsync(roleName));
        }
        #endregion

        #region Filter
        public async Task<List<UserListModel>> Filter(UserFilterModel filter)
        {
            var users = await _unitOfWork.UserRepo.GetAllAsync();
            if (!string.IsNullOrEmpty(filter.FullName))
            {
                users = users.Where(x => x.FullName.ToUpper().Contains(filter.FullName.ToUpper())).ToList();
            }

            if (!string.IsNullOrEmpty(filter.Level))
            {
                users = users.Where(x => x.Level == (UserLevel)Enum.Parse(typeof(UserLevel), filter.Level)).ToList();
            }

            if (!string.IsNullOrEmpty(filter.DateOfBirth[0]) && !string.IsNullOrEmpty(filter.DateOfBirth[1]))
            {
                users = users.Where(x => x.DateOfBirth >= DateTime.ParseExact(filter.DateOfBirth[0], "dd/MM/yyyy", CultureInfo.InvariantCulture) && x.DateOfBirth <= DateTime.ParseExact(filter.DateOfBirth[1], "dd/MM/yyyy", CultureInfo.InvariantCulture)).ToList();
            }

            if (filter.IsMale != null)
            {
                users = users.Where(x => x.IsMale.Equals(filter.IsMale)).ToList();
            }

            if (!string.IsNullOrEmpty(filter.Status))
            {
                users = users.Where(x => x.Status == (UserStatus)Enum.Parse(typeof(UserStatus), filter.Status)).ToList();
            }

            var list = _mapper.Map<List<UserListModel>>(users);
            return list;
        }
        #endregion

        //Implement IsValidToken
        #region ValidateToken
        public bool IsValidToken(string token)
        {
            var currentUserId = _claimsService.GetCurrentUserId;
            var verifyToken = _sessionServices.GetTokenByKey(currentUserId);
            if (string.IsNullOrEmpty(verifyToken) || !verifyToken.Equals(token))
            {
                return false;
            }
            return true;
        }
        #endregion

        //Implement Logout
        #region Logout
        public bool Logout()
        {
            var currentUserId = _claimsService.GetCurrentUserId;
            return _sessionServices.RemoveToken(currentUserId);
        }
        #endregion

        //Implement ExportExcelFile
        #region ExportExcelFile
        public async Task<Stream> ExportExcelFile()
        {
            string[] columnNames = new string[] { "Full Name", "Email", "Phone", "Date of Birth", "Role", "Level", "Status", "Gender" };
            string header = string.Empty;
            var createOptions = await GetCreateOptionsAsync();

            foreach (var column in columnNames)
            {
                header += column + ",";
            }

            var stream = new MemoryStream();

            using (var package = new ExcelPackage(stream))
            {
                var worksheet = package.Workbook.Worksheets.Add("Users");
                worksheet.Cells.LoadFromText(header);

                //Create dropdown list option for Role
                var roleOptions = worksheet.DataValidations.AddListValidation("E2:E1000");
                roleOptions.ShowInputMessage = roleOptions.ShowErrorMessage = true;
                foreach (var role in createOptions.Roles!)
                {
                    roleOptions.Formula.Values.Add(role);
                }

                //Create dropdown list option for Level
                var levelOptions = worksheet.DataValidations.AddListValidation("F2:F1000");
                levelOptions.ShowInputMessage = levelOptions.ShowErrorMessage = true;
                foreach (var level in createOptions.Levels!)
                {
                    levelOptions.Formula.Values.Add(level);
                }

                //Create dropdown list option for Status
                var statusOptions = worksheet.DataValidations.AddListValidation("G2:G1000");
                statusOptions.ShowInputMessage = statusOptions.ShowErrorMessage = true;
                foreach (var status in createOptions.Statuses!)
                {
                    statusOptions.Formula.Values.Add(status.GenerateStringFormat());
                }

                //Create dropdown list option for Gender
                var genderOptions = worksheet.DataValidations.AddListValidation("H2:H1000");
                genderOptions.ShowInputMessage = genderOptions.ShowErrorMessage = true;
                genderOptions.Formula.Values.Add("Male");
                genderOptions.Formula.Values.Add("Female");

                await package.SaveAsync();
            }

            stream.Position = 0;

            return stream;
        }
        #endregion

        //Implement ImportExcelFile
        #region ImportExcelFileAsync
        public async Task<List<UserCreateModel>?> ImportExcelFileAsync(IFormFile formFile,
            bool isScanFullName,
            bool isScanEmail,
            DuplicateHandle duplicateHandle)
        {
            //Create a List of User that Read from Excel File.
            var userListFromFile = new List<UserCreateModel>();

            #region ReadExcelFile
            using (var stream = new MemoryStream())
            {
                await formFile.CopyToAsync(stream);

                using (var excelPackage = new ExcelPackage(stream))
                {
                    ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets[0];
                    var rowCount = worksheet.Dimension.Rows;

                    for (int row = 2; row <= rowCount; row++)
                    {
                        var col = 1;
                        try
                        {
                            userListFromFile.Add(new UserCreateModel
                            {
                                FullName = worksheet.Cells[row, col++].Value.ToString()!.Trim(),
                                Email = worksheet.Cells[row, col++].Value.ToString()!.Trim(),
                                Phone = worksheet.Cells[row, col++].Value.ToString()!.Trim(),
                                DateOfBirth = DateTime.Parse(worksheet.Cells[row, col++].Value.ToString()!.Trim()),
                                Role = worksheet.Cells[row, col++].Value.ToString()!.Trim(),
                                Level = worksheet.Cells[row, col++].Value.ToString()!.Trim(),
                                Status = worksheet.Cells[row, col++].Value.ToString()!.Trim().RegenerateStringFormat(),
                                IsMale = worksheet.Cells[row, col++].Value.ToString()!.Trim().ToUpper().Equals("Male".ToUpper()),
                                AvatarURL = "temp.png"
                            });
                        }
                        catch (Exception)
                        {
                            await stream.DisposeAsync();
                            throw new InvalidDataException($"Exception at line {row}, Column Name: {worksheet.Cells[1, col - 1].Value}, Error: This Cell has an empty value or a not valid value.");
                        }

                        var createdUser = userListFromFile.Last();

                        var validateResult = _userValidator.UserCreateValidator.Validate(createdUser);

                        if (!validateResult.IsValid)
                        {
                            var error = $"Error at line {row}: ";
                            foreach (var failure in validateResult.Errors)
                            {
                                error += failure + "\n";
                            }
                            await stream.DisposeAsync();
                            throw new InvalidDataException(error);
                        }

                        if (!isScanEmail)
                        {
                            if (await _unitOfWork.UserRepo.IsExistsUser(createdUser.Email))
                            {
                                await stream.DisposeAsync();
                                throw new Exception($"Error at line {row}, Column Name: Email, " +
                                    $"Error: We found that this email has already exist. " +
                                    $"Please choose 'Scan Email' options in order to handle it.");
                            }
                        }
                    }
                }
            }
            #endregion

            #region InsertToDb
            //Filter User List by The Scanning Options
            //First Element is list create user
            //Second Element is list update user
            var userListArray = await ImportScanningHandle(userListFromFile, isScanFullName, isScanEmail, duplicateHandle);
            userListFromFile = userListArray[0];

            //Get User Update List if Scanning Option is 'Replace'.
            var userUpdateList = userListArray[1];

            //Create a List of Add User.
            var createUsers = _mapper.Map<List<UserCreateModel>, List<User>>(userListFromFile);

            //Create and Update a List of Edit User if User Update List is not empty.
            if (userUpdateList.Count != 0)
            {
                var updateUsers = new List<User>();
                foreach (var user in userUpdateList)
                {
                    if (!isScanFullName)
                    {
                        //Create a List of Update User Scanning By Email.
                        #region Replace When Scanning Email
                        var tempUser = await _unitOfWork.UserRepo.GetUserByEmailAsync(user.Email);
                        _mapper.Map(user, tempUser);
                        tempUser.RoleId = (await _unitOfWork.RoleRepo.GetRoleByNameAsync(user.Role)).Id;
                        updateUsers.Add(tempUser);
                        #endregion
                    }
                }
                _unitOfWork.UserRepo.UpdateRange(updateUsers);
            }

            //Create a List of User Password.
            var userPasswords = new List<string>();

            //Generate User Password.
            foreach (var user in createUsers)
            {
                var password = GenerateUserPassword(user.Email, user.DateOfBirth);
                userPasswords.Add(password);
                user.Password = password.Hash();
                user.RoleId = (await _unitOfWork.RoleRepo
                    .GetRoleByNameAsync(userListFromFile[createUsers.IndexOf(user)].Role))
                    .Id;
            }

            await _unitOfWork.UserRepo.AddRangeAsync(createUsers);

            if (await _unitOfWork.SaveChangesAsync() >= createUsers.Count)
            {
                foreach (var user in createUsers)
                {
                    await SendMailCreateUserAsync(user.Email, userPasswords[createUsers.IndexOf(user)]);
                }
                return userListFromFile;
            }
            #endregion

            return null;
        }
        #endregion
    }
}