using Application.Commons;
using Application.Interfaces;
using Application.Services;
using Application.Utils;
using Application.ViewModels.MailViewModels;
using Application.ViewModels.TrainingProgramViewModels;
using Application.ViewModels.UserViewModels;
using AutoFixture;
using AutoMapper;
using Domain.Enums.UserEnums;
using Domain.Models;
using Domain.Models.Users;
using Domain.Tests;
using FluentAssertions;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Moq;

namespace Application.Tests.Services
{
    public class UserServiceTests : SetupTest
    {
        private readonly IUserService _userService;
        private Fixture _customFixture;

        public UserServiceTests()
        {
            _userService = new UserService(_unitOfWorkMock.Object,
                _currentTimeMock.Object, _configuration, _emailServiceMock.Object,
                _userValidator.Object, _sessionServicesMock.Object, _claimsServiceMock.Object,
                _mapperConfig);
            _customFixture = new Fixture();
            _customFixture.Customizations.Add(new TypePropertyOmitter(
                "Role",
                "TimeMngSystem",
                "ModifyTrainingProgram",
                "CreatedTrainingProgram",
                "CreatedSyllabus",
                "ModifiedSyllabus",
                "ApprovedClass",
                "CreatedClass",
                "FeedbackTrainee",
                "FeedbackTrainer",
                "GradeReports",
                "ClassUsers",
                "AuditPlans",
                "AuditDetailsAuditor",
                "AuditDetailsTrainee",
                "TimeMngSystemList",
                "TrainingMaterials"));
        }

        //#region Detail
        ////[Fact]
        //public async void GetUserDetailAsync_UserIdExist_ReturnCorrectUserDetail()
        //{
        //    // arrange
        //    var expectedResult = _fixture.Build<UserDetailModel>().Create();

        //    expectedResult.DateOfBirth = "01/01/2002";
        //    expectedResult.Role = UserRole.SuperAdmin.ToString();
        //    expectedResult.Status = UserStatus.OffClass.ToString();
        //    expectedResult.Level = UserLevel.AA.ToString();

        //    var mockItem = _mapperConfig.Map<User>(expectedResult);

        //    _unitOfWorkMock.Setup(x => x.UserRepo.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(mockItem);

        //    // act
        //    var result = await _userService.GetUserDetailAsync(1);

        //    // assert   
        //    _unitOfWorkMock.Verify(x => x.UserRepo.GetByIdAsync(1), Times.Once());
        //    result.Should().BeEquivalentTo(expectedResult);
        //}

        ////[Fact]
        //public async void GetUserDetailAsync_UserIdNotExist_ReturnNull()
        //{
        //    // arrange
        //    var userDetailModel = _fixture.Build<UserDetailModel>().Create();

        //    userDetailModel.DateOfBirth = "01/01/2002";
        //    userDetailModel.Role = UserRole.SuperAdmin.ToString();
        //    userDetailModel.Status = UserStatus.OffClass.ToString();
        //    userDetailModel.Level = UserLevel.AA.ToString();

        //    var mockItem = _mapperConfig.Map<User>(userDetailModel);

        //    _unitOfWorkMock.Setup(x => x.UserRepo.GetByIdAsync(2)).ReturnsAsync(mockItem);

        //    // act
        //    var result = await _userService.GetUserDetailAsync(1);

        //    // assert   
        //    _unitOfWorkMock.Verify(x => x.UserRepo.GetByIdAsync(1), Times.Once());
        //    result.Should().BeNull();
        //}

        //[Fact]
        //public async void GetUserDetailAsync_InvalidUserId_ThrowArgumentException()
        //{
        //    // arrange
        //    int id = 0;

        //    // act
        //    var action = async () => await _userService.GetUserDetailAsync(id);

        //    // assert   
        //    _unitOfWorkMock.Verify(x => x.UserRepo.GetByIdAsync(1), Times.Never);
        //    await action.Should().ThrowAsync<ArgumentException>().WithMessage("User id cannot be less than 1. Input user id: 0");
        //}
        //#endregion

        //#region GetUserListPaginationAsync
        //[Fact()]
        //public async void GetUserListPaginationAsync_InvalidPageIndex_ThrowArgumentException()
        //{
        //    // arrange
        //    int pageIndex = -1;

        //    // act
        //    var action = async () => await _userService.GetUserListPaginationAsync(pageIndex, 1);

        //    // assert   
        //    _unitOfWorkMock.Verify(x => x.UserRepo.ToPaginationAsync(pageIndex, 1), Times.Never);
        //    await action.Should().ThrowAsync<ArgumentException>().WithMessage($"Page index cannot be less than 0. Input page index: {pageIndex}");
        //}

        //[Fact()]
        //public async void GetUserListPaginationAsync_InvalidPageSize_ThrowArgumentException()
        //{
        //    // arrange
        //    int pageSize = 0;

        //    // act
        //    var action = async () => await _userService.GetUserListPaginationAsync(0, pageSize);

        //    // assert   
        //    _unitOfWorkMock.Verify(x => x.UserRepo.ToPaginationAsync(0, pageSize), Times.Never);
        //    await action.Should().ThrowAsync<ArgumentException>().WithMessage($"Page size cannot be less than 1. Input page size: {pageSize}");
        //}

        ////[Fact()]
        //public async void GetUserListPaginationAsync_ValidParametersAndPageHasContent_ReturnUsersListPageWithData()
        //{
        //    // arrange
        //    int pageSize = 5;
        //    int pageIndex = 0;

        //    var usersModelList = _fixture.Build<UserDetailModel>().CreateMany(10).ToList();
        //    foreach (var item in usersModelList)
        //    {
        //        item.DateOfBirth = "01/01/2002";
        //        item.Role = UserRole.SuperAdmin.ToString();
        //        item.Status = UserStatus.OffClass.ToString();
        //        item.Level = UserLevel.AA.ToString();
        //    }
        //    var usersList = _mapperConfig.Map<List<User>>(usersModelList);
        //    var usersPage = new Pagination<User>
        //    {
        //        Items = usersList,
        //        TotalItemCount = usersList.Count,
        //        PageSize = pageSize,
        //        PageIndex = pageIndex
        //    };
        //    _unitOfWorkMock.Setup(x => x.UserRepo.ToPaginationAsync(pageIndex, pageSize)).ReturnsAsync(usersPage);

        //    // act
        //    var result = await _userService.GetUserListPaginationAsync(pageIndex, pageSize);

        //    // assert   
        //    _unitOfWorkMock.Verify(x => x.UserRepo.ToPaginationAsync(pageIndex, pageSize), Times.Once);
        //    result.Items.Should().NotBeNullOrEmpty();
        //    result.Items.Should().HaveCount(10);
        //    result.TotalItemCount.Should().Be(10);
        //    result.TotalPagesCount.Should().Be(2);
        //    result.Next.Should().BeTrue();
        //    result.Previous.Should().BeFalse();
        //    result.PageSize.Should().Be(pageSize);
        //}

        ////[Fact()]
        //public async void GetUserListPaginationAsync_PageIndexPassedTotalPagesCount_ReturnLastUsersListPage()
        //{
        //    // arrange
        //    int pageSize = 10;
        //    int pageIndex = 2;

        //    var usersModelList = _fixture.Build<UserDetailModel>().CreateMany(11).ToList();
        //    foreach (var item in usersModelList)
        //    {
        //        item.DateOfBirth = "01/01/2002";
        //        item.Role = UserRole.SuperAdmin.ToString();
        //        item.Status = UserStatus.OffClass.ToString();
        //        item.Level = UserLevel.AA.ToString();
        //    }
        //    var usersList = _mapperConfig.Map<List<User>>(usersModelList);

        //    var usersPage = new Pagination<User>
        //    {
        //        Items = usersList,
        //        TotalItemCount = usersList.Count,
        //        PageSize = pageSize,
        //        PageIndex = pageIndex,
        //    };
        //    _unitOfWorkMock.Setup(x => x.UserRepo.ToPaginationAsync(pageIndex, pageSize)).ReturnsAsync(usersPage);

        //    // act
        //    var result = await _userService.GetUserListPaginationAsync(pageIndex, pageSize);

        //    // assert   
        //    _unitOfWorkMock.Verify(x => x.UserRepo.ToPaginationAsync(pageIndex, pageSize), Times.Once);
        //    result.Items.Should().NotBeNullOrEmpty();
        //    result.Items.Should().HaveCount(11);
        //    result.TotalItemCount.Should().Be(11);
        //    result.TotalPagesCount.Should().Be(2);
        //    result.Next.Should().BeFalse();
        //    result.Previous.Should().BeTrue();
        //    result.PageSize.Should().Be(pageSize);
        //    result.PageIndex.Should().Be(1);
        //}

        ////[Fact()]
        //public async void GetUserListPaginationAsync_NoParametersPassed_ReturnDefaultUsersListPage()
        //{
        //    // arrange
        //    var usersModelList = _fixture.Build<UserDetailModel>().CreateMany(50).ToList();
        //    foreach (var item in usersModelList)
        //    {
        //        item.DateOfBirth = "01/01/2002";
        //        item.Role = UserRole.SuperAdmin.ToString();
        //        item.Status = UserStatus.OffClass.ToString();
        //        item.Level = UserLevel.AA.ToString();
        //    }
        //    var usersList = _mapperConfig.Map<List<User>>(usersModelList);
        //    var usersPage = new Pagination<User>
        //    {
        //        Items = usersList,
        //        TotalItemCount = usersList.Count,
        //        PageIndex = 0,
        //        PageSize = 100
        //    };
        //    _unitOfWorkMock.Setup(x => x.UserRepo.ToPaginationAsync(0, 10)).ReturnsAsync(usersPage);

        //    // act
        //    var result = await _userService.GetUserListPaginationAsync();

        //    // assert   
        //    _unitOfWorkMock.Verify(x => x.UserRepo.ToPaginationAsync(0, 10), Times.Once);
        //    result.Items.Should().NotBeNullOrEmpty();
        //    result.Items.Should().HaveCount(50);
        //    result.TotalItemCount.Should().Be(50);
        //    result.TotalPagesCount.Should().Be(1);
        //    result.Next.Should().BeFalse();
        //    result.Previous.Should().BeFalse();
        //    result.PageSize.Should().Be(100);
        //}
        //#endregion

        //#region Login
        ////[Fact]
        //public async Task LoginAsync_ShouldReturnCorrectToken_WhenLoginSucceed()
        //{
        //    //Assign
        //    var mocks = _customFixture.Create<User>();

        //    var userLogin = _mapperConfig.Map<UserLoginModel>(mocks);

        //    _unitOfWorkMock.Setup(x => x.UserRepo.GetUserByUserEmailAndPasswordHash(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(mocks);

        //    string expected = mocks.GenerateJsonWebToken(_configuration["Jwt:Key"]!, _currentTimeMock.Object.GetCurrentTime(), _configuration);

        //    //Act
        //    var result = await _userService.LoginAsync(userLogin);

        //    //Assert
        //    Assert.Equal(expected, result);
        //}

        ////[Fact]
        //public async Task LoginAsync_ShouldReturnEmptyString_WhenLoginFailed()
        //{
        //    //Assign
        //    var mocks = _customFixture.Create<User>();

        //    var userLogin = _mapperConfig.Map<UserLoginModel>(mocks);

        //    _unitOfWorkMock.Setup(x => x.UserRepo.GetUserByUserEmailAndPasswordHash(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(mocks);

        //    string expected = mocks.GenerateJsonWebToken(_configuration["Jwt:Key"]!, _currentTimeMock.Object.GetCurrentTime(), _configuration);

        //    //Act
        //    var result = await _userService.LoginAsync(userLogin);

        //    //Assert
        //    Assert.Equal(expected, result);
        //}
        //#endregion

        //#region Create
        ////[Fact]
        //public async Task CreateUserAsync_ShouldReturnTrue_WhenCreatedSuccessfully()
        //{
        //    //Assign
        //    var mocks = _fixture.Build<UserCreateModel>()
        //        .With(x => x.Email, _fixture.Create<string>() + "@example.com")
        //        .Create();

        //    mocks.Role = UserRole.SuperAdmin.ToString();
        //    mocks.Level = UserLevel.AA.ToString();
        //    mocks.Status = UserStatus.InClass.ToString();

        //    _unitOfWorkMock.Setup(x => x.UserRepo.AddAsync(It.IsAny<User>())).Returns(Task.CompletedTask);
        //    _unitOfWorkMock.Setup(x => x.SaveChangesAsync()).ReturnsAsync(1);
        //    _emailServiceMock.Setup(x => x.GetMailBody(It.IsAny<string>(),
        //        It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(),
        //        It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(string.Empty);
        //    _emailServiceMock.Setup(x => x.SendAsync(It.IsAny<MailDataModel>(), It.IsAny<CancellationToken>())).ReturnsAsync(true);

        //    //Act
        //    var result = await _userService.CreateUserAsync(mocks);

        //    //Assert
        //    _unitOfWorkMock.Verify(x => x.UserRepo.AddAsync(It.IsAny<User>()), Times.Once());
        //    _unitOfWorkMock.Verify(x => x.SaveChangesAsync(), Times.Once());
        //    _emailServiceMock.Verify(x => x.GetMailBody(It.IsAny<string>(),
        //        It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(),
        //        It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        //    _emailServiceMock.Verify(x => x.SendAsync(It.IsAny<MailDataModel>(), It.IsAny<CancellationToken>()), Times.Once);
        //    Assert.True(result);
        //}

        ////[Fact]
        //public async Task CreateUserAsync_ShouldReturnFalse_WhenCreatedUnsuccessfully()
        //{
        //    //Assign
        //    var mocks = _fixture.Build<UserCreateModel>()
        //        .With(x => x.Email, _fixture.Create<string>() + "@example.com")
        //        .Create();

        //    mocks.Role = UserRole.SuperAdmin.ToString();
        //    mocks.Level = UserLevel.AA.ToString();
        //    mocks.Status = UserStatus.InClass.ToString();

        //    _unitOfWorkMock.Setup(x => x.UserRepo.AddAsync(It.IsAny<User>())).Returns(Task.CompletedTask);
        //    _unitOfWorkMock.Setup(x => x.SaveChangesAsync()).ReturnsAsync(0);

        //    //Act
        //    var result = await _userService.CreateUserAsync(mocks);

        //    //Assert
        //    _unitOfWorkMock.Verify(x => x.UserRepo.AddAsync(It.IsAny<User>()), Times.Once());
        //    _unitOfWorkMock.Verify(x => x.SaveChangesAsync(), Times.Once());
        //    Assert.False(result);
        //}

        ////[Fact]
        //public async Task CreateUserAsync_ShouldReturnFalse_WhenSendMailUnsuccessfully()
        //{
        //    //Assign
        //    var mocks = _fixture.Build<UserCreateModel>()
        //        .With(x => x.Email, _fixture.Create<string>() + "@example.com")
        //        .Create();

        //    mocks.Role = UserRole.SuperAdmin.ToString();
        //    mocks.Level = UserLevel.AA.ToString();
        //    mocks.Status = UserStatus.InClass.ToString();

        //    _unitOfWorkMock.Setup(x => x.UserRepo.AddAsync(It.IsAny<User>())).Returns(Task.CompletedTask);
        //    _unitOfWorkMock.Setup(x => x.SaveChangesAsync()).ReturnsAsync(1);
        //    _emailServiceMock.Setup(x => x.GetMailBody(It.IsAny<string>(),
        //        It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(),
        //        It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(string.Empty);
        //    _emailServiceMock.Setup(x => x.SendAsync(It.IsAny<MailDataModel>(), It.IsAny<CancellationToken>())).ReturnsAsync(false);

        //    //Act
        //    var result = await _userService.CreateUserAsync(mocks);

        //    //Assert
        //    _unitOfWorkMock.Verify(x => x.UserRepo.AddAsync(It.IsAny<User>()), Times.Once());
        //    _unitOfWorkMock.Verify(x => x.SaveChangesAsync(), Times.Once());
        //    _emailServiceMock.Verify(x => x.GetMailBody(It.IsAny<string>(),
        //        It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(),
        //        It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        //    _emailServiceMock.Verify(x => x.SendAsync(It.IsAny<MailDataModel>(), It.IsAny<CancellationToken>()), Times.Once);
        //    Assert.False(result);
        //}
        //#endregion

        //#region CreateUserSendMail
        //[Fact]
        //public async Task SendMailCreateUserAsync_ShouldReturnTrue_WhenSendMailSucceed()
        //{
        //    //Assign
        //    string email = _fixture.Create<string>() + "@example.com";
        //    string password = _fixture.Create<string>();

        //    _emailServiceMock.Setup(x => x.GetMailBody(It.IsAny<string>(),
        //        It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(),
        //        It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(string.Empty);
        //    _emailServiceMock.Setup(x => x.SendAsync(It.IsAny<MailDataModel>(), It.IsAny<CancellationToken>())).ReturnsAsync(true);

        //    //Act
        //    var result = await _userService.SendMailCreateUserAsync(email, password);

        //    //Assert
        //    _emailServiceMock.Verify(x => x.GetMailBody(It.IsAny<string>(),
        //        It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(),
        //        It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        //    _emailServiceMock.Verify(x => x.SendAsync(It.IsAny<MailDataModel>(), It.IsAny<CancellationToken>()), Times.Once);
        //    Assert.True(result);
        //}

        //[Fact]
        //public async Task SendMailCreateUserAsync_ShouldReturnFalse_WhenSendMailUnsucceed()
        //{
        //    //Assign
        //    string email = _fixture.Create<string>() + "@example.com";
        //    string password = _fixture.Create<string>();

        //    _emailServiceMock.Setup(x => x.GetMailBody(It.IsAny<string>(),
        //        It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(),
        //        It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(string.Empty);
        //    _emailServiceMock.Setup(x => x.SendAsync(It.IsAny<MailDataModel>(), It.IsAny<CancellationToken>())).ReturnsAsync(false);

        //    //Act
        //    var result = await _userService.SendMailCreateUserAsync(email, password);

        //    //Assert
        //    _emailServiceMock.Verify(x => x.GetMailBody(It.IsAny<string>(),
        //        It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(),
        //        It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        //    _emailServiceMock.Verify(x => x.SendAsync(It.IsAny<MailDataModel>(), It.IsAny<CancellationToken>()), Times.Once);
        //    Assert.False(result);
        //}
        //#endregion

        //#region ValidateCreateUserAsync
        //[Fact]
        //public async Task ValidateCreateUserAsync_ShouldReturnCorrectValud_WhenUserCreateIsValid()
        //{
        //    //Assign
        //    var mock = _customFixture.Create<UserCreateModel>();

        //    _userValidator.Setup(x => x.UserCreateValidator.ValidateAsync(It.IsAny<UserCreateModel>(), It.IsAny<CancellationToken>())).ReturnsAsync(new ValidationResult());

        //    //Act
        //    var result = await _userService.ValidateCreateUserAsync(mock);

        //    //Assert
        //    _userValidator.Verify(x => x.UserCreateValidator.ValidateAsync(It.Is<UserCreateModel>(x => x.Equals(mock)), It.IsAny<CancellationToken>()), Times.Once);
        //    Assert.NotNull(result);
        //    Assert.IsAssignableFrom<ValidationResult>(result);
        //}
        //#endregion

        //#region IsExistsUser
        //[Fact]
        //public async Task IsExistsUser_ShouldReturnTrue_WhenUserExists()
        //{
        //    //Assign
        //    string email = "test@gmail.com";
        //    _unitOfWorkMock.Setup(x => x.UserRepo.IsExistsUser(It.IsAny<string>())).ReturnsAsync(true);

        //    //Act
        //    var result = await _userService.IsExistsUserAsync(email);

        //    //Assert
        //    _unitOfWorkMock.Verify(x => x.UserRepo.IsExistsUser(It.Is<string>(x => x.Equals(email))), Times.Once);
        //    Assert.True(result);
        //}

        //[Fact]
        //public async Task IsExistsUser_ShouldReturnFalse_WhenUserNotExists()
        //{
        //    //Assign
        //    string email = "test@gmail.com";
        //    _unitOfWorkMock.Setup(x => x.UserRepo.IsExistsUser(It.IsAny<string>())).ReturnsAsync(false);

        //    //Act
        //    var result = await _userService.IsExistsUserAsync(email);

        //    //Assert
        //    _unitOfWorkMock.Verify(x => x.UserRepo.IsExistsUser(It.Is<string>(x => x.Equals(email))), Times.Once);
        //    Assert.False(result);
        //}
        //#endregion

        //#region GetCreateOptions
        ////[Fact]
        //public async Task GetCreateOptions_ShouldReturnCorrectData()
        //{
        //    //Assign
        //    var expected = new UserCreateOptions();

        //    var roles = new List<string>
        //    {
        //        " Super Admin", " Class Admin",
        //        " Trainer", " Auditor",
        //        " Trainee"
        //    };

        //    var levels = new List<string>
        //    {
        //        "AA", "AB", "AC", "AD",
        //        "BA", "BB", "BC", "BD",
        //        "CA", "CB", "CD", "PMIPBA"
        //    };

        //    expected.Roles = roles;
        //    expected.Levels = levels;

        //    //Act
        //    var result = await _userService.GetCreateOptionsAsync();

        //    //Assert
        //    Assert.NotNull(expected);
        //    result.Should().BeEquivalentTo(expected);
        //}
        //#endregion

        //#region ImportFile
        ////[Fact]
        //public async Task ImportFileAsync_ShouldReturnCorrectData_WhenExcelFileIsExist()
        //{
        //    //Assign
        //    var fileName = Path.Combine(Directory.GetCurrentDirectory(), @"TestFiles\ValidFileTest.xlsx");
        //    using var fileStream = new FileStream(fileName, FileMode.Open);
        //    IFormFile file = new FormFile(fileStream, 0, fileStream.Length, "test_form", fileName);
        //    var cancellation = new CancellationToken();
        //    var validate = new ValidationResult();

        //    _unitOfWorkMock.Setup(x => x.UserRepo.AddRangeAsync(It.IsAny<List<User>>())).Returns(Task.CompletedTask);
        //    _unitOfWorkMock.Setup(x => x.SaveChangesAsync()).ReturnsAsync(3);
        //    _userValidator.Setup(x => x.UserCreateValidator.Validate(It.IsAny<UserCreateModel>())).Returns(validate);
        //    _emailServiceMock.Setup(x => x.GetMailBody(It.IsAny<string>(),
        //        It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(),
        //        It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(string.Empty);
        //    _emailServiceMock.Setup(x => x.SendAsync(It.IsAny<MailDataModel>(), It.IsAny<CancellationToken>())).ReturnsAsync(true);

        //    //Act
        //    var result = await _userService.ImportFileAsync(file, cancellation);

        //    //Assert
        //    _emailServiceMock.Verify(x => x.GetMailBody(It.IsAny<string>(),
        //        It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(),
        //        It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Exactly(result!.Count));
        //    _emailServiceMock.Verify(x => x.SendAsync(It.IsAny<MailDataModel>(), It.IsAny<CancellationToken>()), Times.Exactly(result.Count));
        //    Assert.NotNull(result);
        //    Assert.NotEmpty(result);
        //}

        ////[Fact]
        //public async Task ImportFileAsync_ShouldReturnNull_WhenSaveFileUnsucceed()
        //{
        //    //Assign
        //    var fileName = Path.Combine(Directory.GetCurrentDirectory(), @"TestFiles\ValidFileTest.xlsx");
        //    using var fileStream = new FileStream(fileName, FileMode.Open);
        //    IFormFile file = new FormFile(fileStream, 0, fileStream.Length, "test_form", fileName);
        //    var cancellation = new CancellationToken();
        //    var validate = new ValidationResult();

        //    _unitOfWorkMock.Setup(x => x.UserRepo.AddRangeAsync(It.IsAny<List<User>>())).Returns(Task.CompletedTask);
        //    _unitOfWorkMock.Setup(x => x.SaveChangesAsync()).ReturnsAsync(0);
        //    _userValidator.Setup(x => x.UserCreateValidator.Validate(It.IsAny<UserCreateModel>())).Returns(validate);

        //    //Act
        //    var result = await _userService.ImportFileAsync(file, cancellation);

        //    //Assert
        //    Assert.Null(result);
        //}

        ///// <summary>
        ///// The test data of import file async function when excel row is invalid
        ///// </summary>
        //public static IEnumerable<object[]> ImportFileData => new List<object[]>
        //{
        //    new object[] { "EmptyCellFileTest.xlsx" },
        //    new object[] { "InvalidRoleFileTest.xlsx" },
        //    new object[] { "InvalidLevelFileTest.xlsx" },
        //    new object[] { "InvalidStatusFileTest.xlsx" },
        //    new object[] { "InvalidValidationFileTest.xlsx" }
        //};

        ////[Theory]
        ////[MemberData(nameof(ImportFileData))]
        //public async Task ImportFileAsync_ShouldReturnException_WhenExcelRowIsInvalid(string excelFile)
        //{
        //    //Assign
        //    var fileName = Path.Combine(Directory.GetCurrentDirectory(), @"TestFiles\", excelFile);
        //    using var fileStream = new FileStream(fileName, FileMode.Open);
        //    IFormFile file = new FormFile(fileStream, 0, fileStream.Length, "test_form", fileName);
        //    var cancellation = new CancellationToken();
        //    IEnumerable<ValidationFailure> errors = new List<ValidationFailure>
        //    {
        //        new ValidationFailure("Email", "Error at line 2: Your file has an empty cell, please check again.")
        //    };
        //    var validate = new ValidationResult(errors);

        //    _userValidator.Setup(x => x.UserCreateValidator.Validate(It.IsAny<UserCreateModel>())).Returns(validate);

        //    //Act
        //    var exceptionResult = async () => await _userService.ImportFileAsync(file, cancellation);

        //    //Assert
        //    var result = await Assert.ThrowsAsync<Exception>(exceptionResult);
        //    Assert.Contains("Error at line", result.Message);
        //}
        //#endregion

        //#region EditUserAsync

        //[Fact()]
        //public async void EditAsync_AllInputCorrect_ReturnTrueAndUserUpdated()
        //{
        //    // Arrange
        //    var userUpdateModel = _fixture.Build<UserUpdateModel>().Create();

        //    userUpdateModel.Id = 6;
        //    userUpdateModel.FullName = "Savoy";
        //    userUpdateModel.Phone = "0386472363";
        //    userUpdateModel.DateOfBirth = "21/09/2002";
        //    userUpdateModel.IsMale = "true";
        //    userUpdateModel.RoleId = 1;
        //    userUpdateModel.Level = "AA";
        //    userUpdateModel.Status = "InClass";
        //    userUpdateModel.AvatarURL = "string";

        //    User userEntity = new User();

        //    bool result = true;

        //    _mapperConfig.Map(userUpdateModel, userEntity, typeof(UserUpdateModel), typeof(User));

        //        _unitOfWorkMock.Setup(x => x.UserRepo.GetByIdAsync(userUpdateModel.Id)).ReturnsAsync(userEntity);
        //        _unitOfWorkMock.Setup(x => x.UserRepo.Update(userEntity)).Verifiable();
        //        _unitOfWorkMock.Setup(x => x.SaveChangesAsync()).ReturnsAsync(userUpdateModel.Id);

        //        // Act
        //        result = await _userService.EditAsync(userUpdateModel, userEntity);

        //        // Assert
        //        _unitOfWorkMock.Verify(x => x.UserRepo.Update(userEntity), Times.Once);
        //        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(), Times.Once);


        //    result.Should().BeTrue();
        //}

        //[Fact()]
        //public async void EditAsync_UserIdNotExist_ReturnFalse()
        //{
        //    // Arrange
        //    var userUpdateModel = _fixture.Build<UserUpdateModel>().Create();

        //    userUpdateModel.Id = 0;
        //    userUpdateModel.FullName = "Savoy";
        //    userUpdateModel.Phone = "0386472363";
        //    userUpdateModel.DateOfBirth = "21/09/2002";
        //    userUpdateModel.IsMale = "true";
        //    userUpdateModel.RoleId = 1;
        //    userUpdateModel.Level = "AA";
        //    userUpdateModel.Status = "InClass";
        //    userUpdateModel.AvatarURL = "";

        //    User userEntity = new User();

        //    bool result = true;

        //    try
        //    {
        //        userEntity = _mapperConfig.Map<User>(userUpdateModel);

        //        _unitOfWorkMock.Setup(x => x.UserRepo.GetByIdAsync(userUpdateModel.Id)).ReturnsAsync(userEntity);
        //        _unitOfWorkMock.Setup(x => x.UserRepo.Update(userEntity)).Verifiable();
        //        _unitOfWorkMock.Setup(x => x.SaveChangesAsync()).ReturnsAsync(userUpdateModel.Id);

        //        // Act
        //        result = await _userService.EditAsync(userUpdateModel, userEntity);

        //        // Assert
        //        _unitOfWorkMock.Verify(x => x.UserRepo.Update(userEntity), Times.Once);
        //        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(), Times.Once);
        //    }
        //    catch (Exception ex) when (ex is NullReferenceException || ex is AutoMapperMappingException || ex is FormatException)
        //    {
        //        result = false;
        //    }

        //    result.Should().BeFalse();
        //}

        //#endregion

        //#region DeleteUserAsync
        //[Fact()]
        //public async void DeleteAsync_UserIDExist_ReturnTrue()
        //{
        //    // Arrange
        //    var user = _fixture.Build<User>().Create();

        //    user.Id = 6;

        //    bool result = true;

        //    //try
        //    //{

        //        _unitOfWorkMock.Setup(x => x.UserRepo.Update(user)).Verifiable();
        //        _unitOfWorkMock.Setup(x => x.SaveChangesAsync());

        //        // Act                                                            
        //        result = await _userService.DeleteAsync(user);

        //        // Assert
        //        _unitOfWorkMock.Verify(x => x.UserRepo.Update(user), Times.Once);
        //        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(), Times.Once);
        //    //}
        //    //catch (Exception ex)
        //    //{
        //    //    result = false;
        //    //}

        //    result.Should().BeTrue();
        //}

        //[Fact()]
        //public async void DeleteAsync_UserIDNotExist_ReturnFalse()
        //{
        //    // Arrange
        //    var user = _fixture.Build<User>().Create();

        //    user.Id = 0;

        //    bool result = true;

        //    _unitOfWorkMock.Setup(x => x.UserRepo.Update(user)).Verifiable();
        //    _unitOfWorkMock.Setup(x => x.SaveChangesAsync()).ReturnsAsync(user.Id);

        //    // Act
        //    result = await _userService.DeleteAsync(user);

        //    // Assert
        //    _unitOfWorkMock.Verify(x => x.UserRepo.Update(user), Times.Once);
        //    _unitOfWorkMock.Verify(x => x.SaveChangesAsync(), Times.Once);

        //    result.Should().BeFalse();
        //}

        //#endregion

        //#region GetUserListAsync
        //[Fact]
        //public async void GetUserListAsync_UsersExist_ReturnCorrectData()
        //{
        //    // arrange
        //    var userList = new List<User>();
        //    var user = _customFixture.Build<User>().Create();
        //    userList.Add(user);

        //    var expectedResult = _mapperConfig.Map<List<UserListModel>>(userList);

        //    _unitOfWorkMock.Setup(x => x.UserRepo.GetAllAsync()).ReturnsAsync(userList);

        //    // act
        //    var result = await _userService.GetUserListAsync();

        //    // assert
        //    result.Should().NotBeNull();
        //    result.Should().BeEquivalentTo(expectedResult);
        //    result.Count.Should().Be(1);
        //    result[0].Id.Should().Be(user.Id);
        //}

        //[Fact]
        //public async void GetUserListAsync_NoUsersExist_ShouldThrowInvalidOperationException()
        //{
        //    // arrange
        //    List<User> userList = new List<User>();
        //    _unitOfWorkMock.Setup(x => x.UserRepo.GetAllAsync()).ReturnsAsync(userList);

        //    // act

        //    // assert
        //    await Assert.ThrowsAsync<InvalidOperationException>(() => _userService.GetUserListAsync());
        //}
        //#endregion

        //#region GeneratePasswordToken

        //[Fact()]
        //public void GeneratePasswordToken_ShouldReturnCorrectToken()
        //{
        //    //Assign
        //    User mockUser = _customFixture.Create<User>();
        //    string expected = mockUser.GenerateJsonWebTokenCustomExpireMinute(mockUser.Password, _currentTimeMock.Object.GetCurrentTime(), 60 * 24, _configuration);

        //    //Act
        //    string actual = _userService.GeneratePasswordToken(mockUser);

        //    //Assert
        //    Assert.Equal(expected, actual);

        //}
        //#endregion

        //#region ForgotPasswordAsync
        //[Fact]
        //public async Task ForgotPasswordAsync_ShouldReturnToken_WhenSaveChangesSuccess()
        //{
        //    //Assign
        //    string mailAddress = "example@example.com";
        //    User mockUser = _customFixture.Create<User>();
        //    _unitOfWorkMock.Setup(uow => uow.UserRepo.GetUserByEmailAsync(It.IsAny<string>())).ReturnsAsync(mockUser);
        //    _unitOfWorkMock.Setup(uow => uow.SaveChangesAsync()).ReturnsAsync(1);

        //    var expected = mockUser.GenerateJsonWebTokenCustomExpireMinute(mockUser.Password, _currentTimeMock.Object.GetCurrentTime(), 60 * 24, _configuration);

        //    //Act
        //    var actual = await _userService.ForgotPasswordAsync(mailAddress);

        //    //Assert
        //    Assert.Equal(expected, actual);
        //}

        //[Fact]
        //public async Task ForgotPasswordAsync_ShouldReturnNull_WhenSaveChangesFail()
        //{
        //    //Assign
        //    string mailAddress = "example@example.com";
        //    User mockUser = _customFixture.Create<User>();
        //    _unitOfWorkMock.Setup(uow => uow.UserRepo.GetUserByEmailAsync(It.IsAny<string>())).ReturnsAsync(mockUser);
        //    _unitOfWorkMock.Setup(uow => uow.SaveChangesAsync()).ReturnsAsync(0);

        //    //Act
        //    var actual = await _userService.ForgotPasswordAsync(mailAddress);

        //    //Assert
        //    Assert.Null(actual);
        //}
        //#endregion

        //#region IsValidTokenAsync
        //[Fact]
        //public async Task IsValidTokenAsync_ShouldReturnTrue_WhenTokensAreSame()
        //{
        //    //Assign
        //    User mockUser = _customFixture.Create<User>();
        //    var inputToken = mockUser.ResetToken;
        //    _unitOfWorkMock.Setup(uow => uow.UserRepo.GetUserByEmailAsync(It.IsAny<string>())).ReturnsAsync(mockUser);

        //    //Act
        //    var actual = await _userService.IsValidPasswordTokenAsync(mockUser.Email, inputToken);

        //    //Assert
        //    Assert.True(actual);
        //}

        //[Fact]
        //public async Task IsValidTokenAsync_ShouldReturnFalse_WhenTokensAreDifferent()
        //{
        //    //Assign
        //    User mockUser = _customFixture.Create<User>();
        //    var inputToken = mockUser.ResetToken + "1";
        //    _unitOfWorkMock.Setup(uow => uow.UserRepo.GetUserByEmailAsync(It.IsAny<string>())).ReturnsAsync(mockUser);

        //    //Act
        //    var actual = await _userService.IsValidPasswordTokenAsync(mockUser.Email, inputToken);

        //    //Assert
        //    Assert.False(actual);
        //}
        //#endregion

        //#region ChangePasswordAsync
        //[Fact]
        //public async Task ChangePasswordAsync_ShouldReturnTrue_WhenSaveChangesSuccess()
        //{
        //    //Assign
        //    User user = _customFixture.Create<User>();
        //    _unitOfWorkMock.Setup(uow => uow.UserRepo.GetUserByEmailAsync(It.IsAny<string>())).ReturnsAsync(user);
        //    _unitOfWorkMock.Setup(uow => uow.SaveChangesAsync()).ReturnsAsync(1);

        //    //Act
        //    bool actual = await _userService.ChangePasswordAsync(user.Email, user.Password + "1");

        //    //Assert
        //    Assert.True(actual);
        //}

        //[Fact]
        //public async Task ChangePasswordAsync_ShouldReturnTrue_WhenSaveChangesFail()
        //{
        //    //Assign
        //    User user = _customFixture.Create<User>();
        //    _unitOfWorkMock.Setup(uow => uow.UserRepo.GetUserByEmailAsync(It.IsAny<string>())).ReturnsAsync(user);
        //    _unitOfWorkMock.Setup(uow => uow.SaveChangesAsync()).ReturnsAsync(0);

        //    //Act
        //    bool actual = await _userService.ChangePasswordAsync(user.Email, user.Password + "1");

        //    //Assert
        //    Assert.False(actual);
        //}
        //#endregion

        //#region IsExpiredPasswordToken
        ////[Fact]
        //public void IsExpiredPasswordToken_ShouldReturnTrue_WhenTokenExpired()
        //{
        //    //Assign
        //    User mockUser= _customFixture.Create<User>();
        //    var token = mockUser.GenerateJsonWebToken(_configuration["Jwt:Key"], _currentTimeMock.Object.GetCurrentTime().AddDays(-1), _configuration);

        //    //Act
        //    var actual = _userService.IsExpiredPasswordToken(token);

        //    //Assert 
        //    Assert.True(actual);
        //}

        ////[Fact]
        //public void IsExpiredPasswordToken_ShouldReturnFalse_WhenTokenIsNotExpired()
        //{
        //    //Assign
        //    User mockUser = _customFixture.Create<User>();
        //    var token = mockUser.GenerateJsonWebToken(_configuration["Jwt:Key"], _currentTimeMock.Object.GetCurrentTime(), _configuration);

        //    //Act
        //    var actual = _userService.IsExpiredPasswordToken(token);

        //    //Assert 
        //    Assert.True(actual);
        //}
        //#endregion

        [Fact]
        public async void Filter_ReturnList()
        {
            //Arrange
            var mockFilter = _fixture.Build<UserFilterModel>().Create();
            var mock = _customFixture.Build<User>().CreateMany(3).ToList();
            _unitOfWorkMock.Setup(x => x.UserRepo.GetAllAsync()).ReturnsAsync(mock);
            //Act
            var result = await _userService.Filter(mockFilter);
            //Assert
            result.Should().NotBeNull().And.BeEmpty();
        }
    }
}