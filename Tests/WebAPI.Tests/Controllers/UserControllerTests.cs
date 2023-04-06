using Application.ViewModels.TrainingProgramViewModels;
using Application.ViewModels.UserViewModels;
using AutoFixture;
using Domain.Models.Users;
using Domain.Tests;
using FluentAssertions;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Moq;
using WebAPI.Controllers;

namespace WebAPI.Tests.Controllers
{
    public class UserControllerTests : SetupTest
    {
        private readonly UserController _userController;
        private Fixture _customFixture;

        public UserControllerTests()
        {
            _userController = new UserController(_userServiceMock.Object);
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
        //[Fact]
        //public async void Detail_ExistUserIdPassed_ReturnCorrectUserDetail()
        //{
        //    // Arrange
        //    var userModel = _fixture.Build<UserDetailModel>().Create();
        //    _userServiceMock.Setup(x => x.GetUserDetailAsync(It.IsAny<int>())).ReturnsAsync(userModel);

        //    // Act
        //    var result = await _userController.Detail(1) as OkObjectResult;

        //    // Assert
        //    _userServiceMock.Verify(x => x.GetUserDetailAsync(It.IsAny<int>()), Times.Once);
        //    result.Should().BeOfType<OkObjectResult>();
        //    result.Value.Should().NotBeNull();
        //    result.Value.Should().BeOfType<UserDetailModel>();

        //}

        //[Fact]
        //public async void Detail_NoExistUserIdPassed_ReturnNotFoundResult()
        //{
        //    // Arrange
        //    UserDetailModel userModel = null;
        //    _userServiceMock.Setup(x => x.GetUserDetailAsync(It.IsAny<int>())).ReturnsAsync(userModel);

        //    // Act
        //    var result = await _userController.Detail(1) as NotFoundObjectResult;

        //    // Assert
        //    _userServiceMock.Verify(x => x.GetUserDetailAsync(It.IsAny<int>()), Times.Once);
        //    result.Should().BeOfType<NotFoundObjectResult>();
        //    result.Value.Should().BeEquivalentTo("User not found");

        //}

        //[Fact]
        //public async void Detail_InvalidUserIdPassed_ReturnBadRequestResult()
        //{
        //    // Arrange
        //    int id = 0;
        //    _userServiceMock.Setup(x => x.GetUserDetailAsync(id)).Throws(new ArgumentException($"User id cannot be less than 1. Input user id: {id}"));

        //    // Act
        //    var result = await _userController.Detail(id) as BadRequestObjectResult;

        //    // Assert
        //    _userServiceMock.Verify(x => x.GetUserDetailAsync(0), Times.Once);
        //    result.Should().BeOfType<BadRequestObjectResult>();
        //    result.Value.Should().BeEquivalentTo($"Invalid parameters: User id cannot be less than 1. Input user id: {id}");

        //}
        //#endregion

        //#region ListPagination
        //[Fact]
        //public async void List_ValidParameters_ReturnCorrectData()
        //{
        //    // Arrange
        //    var mocks = _fixture.Build<Pagination<UserListModel>>().Create();
        //    _userServiceMock.Setup(x => x.GetUserListPaginationAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(mocks);

        //    // Act
        //    var result = await _userController.List(0, 30) as OkObjectResult;

        //    // Assert
        //    _userServiceMock.Verify(x => x.GetUserListPaginationAsync(It.Is<int>(x => x.Equals(0)), It.Is<int>(x => x.Equals(30))), Times.Once);
        //    result.Should().BeOfType<OkObjectResult>();
        //    result.Value.Should().BeEquivalentTo(mocks);
        //}

        //[Fact]
        //public async void List_OutOfRangePageIndex_ReturnOkResultWithLastPage()
        //{
        //    // Arrange
        //    var users = _fixture.Build<UserListModel>().CreateMany(10).ToList();
        //    var userPage = new Pagination<UserListModel>
        //    {
        //        PageSize = 10,
        //        TotalItemCount = users.Count,
        //        PageIndex = 1,
        //        Items = users
        //    };
        //    _userServiceMock.Setup(x => x.GetUserListPaginationAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(userPage);

        //    // Act
        //    var result = await _userController.List(1, 10) as OkObjectResult;
        //    var pageResult = (Pagination<UserListModel>)result.Value;

        //    // Assert
        //    _userServiceMock.Verify(x => x.GetUserListPaginationAsync(It.Is<int>(x => x.Equals(1)), It.Is<int>(x => x.Equals(10))), Times.Once);
        //    result.Should().BeOfType<OkObjectResult>();
        //    result.Value.Should().BeEquivalentTo(userPage);
        //    pageResult.PageIndex.Should().Be(0);

        //}

        //[Fact]
        //public async void List_UserServiceReturnNull_ReturnNotFound()
        //{
        //    // Arrange
        //    _userServiceMock.Setup(x => x.GetUserListPaginationAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(value: null);

        //    // Act
        //    var result = await _userController.List(100, 100) as NotFoundResult;

        //    // Assert
        //    _userServiceMock.Verify(x => x.GetUserListPaginationAsync(It.Is<int>(x => x.Equals(100)), It.Is<int>(x => x.Equals(100))), Times.Once);
        //    result.Should().BeOfType<NotFoundResult>();
        //}

        //[Fact]
        //public async void List_DefaultParameters_ReturnCorrectData()
        //{
        //    // Arrange
        //    var mocks = _fixture.Build<Pagination<UserListModel>>().Create();
        //    _userServiceMock.Setup(x => x.GetUserListPaginationAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(mocks);

        //    // Act
        //    var result = await _userController.List() as OkObjectResult;

        //    // Assert
        //    _userServiceMock.Verify(x => x.GetUserListPaginationAsync(It.Is<int>(x => x.Equals(0)), It.Is<int>(x => x.Equals(10))), Times.Once);
        //    result.Should().BeOfType<OkObjectResult>();
        //    result.Value.Should().BeEquivalentTo(mocks);
        //}

        //[Fact]
        //public async void List_InvalidPageIndex_ReturnBadRequest()
        //{
        //    // Arrange
        //    int pageIndex = -1;
        //    _userServiceMock.Setup(x => x.GetUserListPaginationAsync(pageIndex, It.IsAny<int>())).Throws(new ArgumentException($"Page index cannot be less than 0. Input page index: {pageIndex}"));

        //    // Act
        //    var result = await _userController.List(-1, 10) as BadRequestObjectResult;

        //    // Assert
        //    _userServiceMock.Verify(x => x.GetUserListPaginationAsync(It.Is<int>(x => x.Equals(pageIndex)), It.Is<int>(x => x.Equals(10))), Times.Once);
        //    result.Should().BeOfType<BadRequestObjectResult>();
        //    result.Value.Should().BeEquivalentTo($"Invalid parameters: Page index cannot be less than 0. Input page index: {pageIndex}");

        //}

        //[Fact]
        //public async void List_InvalidPageSize_ReturnBadRequest()
        //{
        //    // Arrange
        //    int pageSize = 0;
        //    _userServiceMock.Setup(x => x.GetUserListPaginationAsync(It.IsAny<int>(), pageSize)).Throws(new ArgumentException($"Page size cannot be less than 1. Input page index: {pageSize}"));

        //    // Act
        //    var result = await _userController.List(0, pageSize) as BadRequestObjectResult;

        //    // Assert
        //    _userServiceMock.Verify(x => x.GetUserListPaginationAsync(It.Is<int>(x => x.Equals(0)), It.Is<int>(x => x.Equals(pageSize))), Times.Once);
        //    result.Should().BeOfType<BadRequestObjectResult>();
        //    result.Value.Should().BeEquivalentTo($"Invalid parameters: Page size cannot be less than 1. Input page index: {pageSize}");

        //}
        //#endregion

        //#region Login
        ////[Fact]
        //public async Task LoginAsync_ShouldReturnOkResult_WhenLoginSucceed()
        //{
        //    //Assign
        //    var mock = _customFixture.Create<User>();
        //    var userDetailMock = _fixture.Create<UserDetailModel>();

        //    var userLogin = _mapperConfig.Map<UserLoginModel>(mock);
        //    string expected = mock.GenerateJsonWebToken(_configuration["Jwt:Key"]!, _currentTimeMock.Object.GetCurrentTime(), _configuration);

        //    _userServiceMock.Setup(x => x.LoginAsync(It.IsAny<UserLoginModel>())).ReturnsAsync(expected);
        //    _userServiceMock.Setup(x => x.GetUserByEmailAsync(It.IsAny<string>())).ReturnsAsync(userDetailMock);

        //    //Act
        //    var actionResult = await _userController.LoginAsync(userLogin);
        //    var result = actionResult as ObjectResult;

        //    //Assert
        //    _userServiceMock.Verify(x => x.LoginAsync(It.Is<UserLoginModel>(x => x.Equals(userLogin))), Times.Once());
        //    _userServiceMock.Verify(x => x.GetUserByEmailAsync(It.IsAny<string>()), Times.Once());
        //    Assert.NotNull(result);
        //    Assert.IsAssignableFrom<OkObjectResult>(result);
        //    Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
        //    Assert.Equivalent(new { Token = expected, User = userDetailMock }, result.Value);
        //}

        ////[Fact]
        //public async Task LoginAsync_ShouldReturnNotFoundResult_WhenLoginFailed()
        //{
        //    //Assign
        //    var mock = _customFixture.Create<User>();

        //    var userLogin = _mapperConfig.Map<UserLoginModel>(mock);
        //    string expected = string.Empty;

        //    _userServiceMock.Setup(x => x.LoginAsync(It.IsAny<UserLoginModel>()))!.ReturnsAsync(expected);

        //    //Act
        //    var actionResult = await _userController.LoginAsync(userLogin);
        //    var result = actionResult as ObjectResult;

        //    //Assert
        //    _userServiceMock.Verify(x => x.LoginAsync(It.Is<UserLoginModel>(x => x.Equals(userLogin))), Times.Once());
        //    Assert.NotNull(result);
        //    Assert.IsAssignableFrom<NotFoundObjectResult>(result);
        //    Assert.Equal(StatusCodes.Status404NotFound, result.StatusCode);
        //    Assert.Equivalent(new { Message = "Incorrect Email or Password." }, result.Value);
        //}
        //#endregion

        //#region Create
        //[Fact]
        //public async Task CreateUserAsync_ShouldReturnCreatedResult_WhenCreateSucceed()
        //{
        //    //Assign
        //    var expectedUser = _customFixture.Create<UserCreateModel>();
        //    expectedUser.Role = UserRole.Trainee.ToString();
        //    expectedUser.Level = UserLevel.AA.ToString();
        //    expectedUser.Status = UserStatus.InClass.ToString();

        //    var validate = new ValidationResult();

        //    _userServiceMock.Setup(x => x.IsExistsUserAsync(It.IsAny<string>())).ReturnsAsync(false);
        //    _userServiceMock.Setup(x => x.CreateUserAsync(It.IsAny<UserCreateModel>())).ReturnsAsync(true);
        //    _userServiceMock.Setup(x => x.ValidateCreateUserAsync(It.IsAny<UserCreateModel>())).ReturnsAsync(validate);

        //    //Act
        //    var result = await _userController.CreateUserAsync(expectedUser);

        //    //Assert
        //    _userServiceMock.Verify(x => x.CreateUserAsync(It.Is<UserCreateModel>(x => x.Equals(expectedUser))), Times.Once());
        //    _userServiceMock.Verify(x => x.IsExistsUserAsync(It.IsAny<string>()), Times.Once);
        //    _userServiceMock.Verify(x => x.ValidateCreateUserAsync(It.Is<UserCreateModel>(x => x.Equals(expectedUser))), Times.Once);
        //    Assert.NotNull(result);
        //    Assert.IsAssignableFrom<CreatedResult>(result);
        //    Assert.IsAssignableFrom<UserCreateModel>((result as ObjectResult)!.Value);
        //    Assert.Equal(StatusCodes.Status201Created, (result as CreatedResult)!.StatusCode);
        //    (result as ObjectResult)!.Value.Should().BeEquivalentTo(expectedUser);
        //}

        ////[Fact]
        //public async Task CreateUserAsync_ShouldReturnNotFoundResult_WhenCreateUnsucceed()
        //{
        //    //Assign
        //    var expectedUser = _customFixture.Create<UserCreateModel>();
        //    var validate = new ValidationResult();

        //    _userServiceMock.Setup(x => x.IsExistsUserAsync(It.IsAny<string>())).ReturnsAsync(false);
        //    _userServiceMock.Setup(x => x.CreateUserAsync(It.IsAny<UserCreateModel>())).ReturnsAsync(false);
        //    _userServiceMock.Setup(x => x.ValidateCreateUserAsync(It.IsAny<UserCreateModel>())).ReturnsAsync(validate);

        //    //Act
        //    var result = await _userController.CreateUserAsync(expectedUser);

        //    //Assert
        //    _userServiceMock.Verify(x => x.CreateUserAsync(It.Is<UserCreateModel>(x => x.Equals(expectedUser))), Times.Once());
        //    _userServiceMock.Verify(x => x.IsExistsUserAsync(It.IsAny<string>()), Times.Once);
        //    _userServiceMock.Verify(x => x.ValidateCreateUserAsync(It.Is<UserCreateModel>(x => x.Equals(expectedUser))), Times.Once);
        //    Assert.NotNull(result);
        //    Assert.IsAssignableFrom<NotFoundObjectResult>(result);
        //    Assert.Equal(StatusCodes.Status404NotFound, (result as ObjectResult)!.StatusCode);
        //    Assert.Equal("Created Unsuccessfully.", (result as ObjectResult)!.Value);
        //}

        ////[Fact]
        //public async Task CreateUserAsync_ShouldReturnBadRequestResult_WhenUserCreatedEmailIsExists()
        //{
        //    //Assign
        //    var expectedUser = _customFixture.Create<UserCreateModel>();
        //    var validate = new ValidationResult();

        //    _userServiceMock.Setup(x => x.IsExistsUserAsync(It.IsAny<string>())).ReturnsAsync(true);
        //    _userServiceMock.Setup(x => x.ValidateCreateUserAsync(It.IsAny<UserCreateModel>())).ReturnsAsync(validate);

        //    //Act
        //    var result = await _userController.CreateUserAsync(expectedUser);

        //    //Assert
        //    _userServiceMock.Verify(x => x.IsExistsUserAsync(It.IsAny<string>()), Times.Once);
        //    _userServiceMock.Verify(x => x.ValidateCreateUserAsync(It.Is<UserCreateModel>(x => x.Equals(expectedUser))), Times.Once);
        //    Assert.NotNull(result);
        //    Assert.IsAssignableFrom<BadRequestObjectResult>(result);
        //    Assert.Equal(StatusCodes.Status400BadRequest, (result as ObjectResult)!.StatusCode);
        //    Assert.Equal("This email is already exists.", (result as ObjectResult)!.Value);
        //}

        ////[Fact]
        //public async Task CreateUserAsync_ShouldReturnBadRequestResult_WhenUserCreatedValidationIsInvalid()
        //{
        //    //Assign
        //    var expectedUser = _customFixture.Create<UserCreateModel>();
        //    IEnumerable<ValidationFailure> errors = new List<ValidationFailure>
        //    {
        //        new ValidationFailure("Email", "User email cannot be empty."),
        //        new ValidationFailure("UserRole", "User role is invalid.")
        //    };
        //    var validate = new ValidationResult(errors);

        //    _userServiceMock.Setup(x => x.ValidateCreateUserAsync(It.IsAny<UserCreateModel>())).ReturnsAsync(validate);

        //    //Act
        //    var result = await _userController.CreateUserAsync(expectedUser);

        //    //Assert
        //    _userServiceMock.Verify(x => x.ValidateCreateUserAsync(It.Is<UserCreateModel>(x => x.Equals(expectedUser))), Times.Once);
        //    Assert.NotNull(result);
        //    Assert.IsAssignableFrom<BadRequestObjectResult>(result);
        //    Assert.Equal(StatusCodes.Status400BadRequest, (result as ObjectResult)!.StatusCode);
        //}
        //#endregion

        //#region GetCreateOptions
        ////[Fact]
        //public async void GetCreateOptions_ShouldReturnCorrectData_WhenDataExists()
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

        //    _userServiceMock.Setup(x => x.GetCreateOptionsAsync()).ReturnsAsync(expected);

        //    //Act
        //    var result = await _userController.GetCreateOptionsAsync();

        //    //Assert
        //    _userServiceMock.Verify(x => x.GetCreateOptionsAsync(), Times.Once());
        //    Assert.NotNull(expected);
        //    result.Should().BeEquivalentTo(expected);
        //}
        //#endregion

        //#region ImportUser
        //[Fact]
        //public async Task ImportUserAsync_ShouldReturnCreatedResult_WhenAddSucceed()
        //{
        //    //Assign
        //    var fileName = "test.xlsx";
        //    var stream = new MemoryStream();

        //    IFormFile file = new FormFile(stream, 0, 1, "test_form", fileName);
        //    var cancellation = new CancellationToken();

        //    var mock = _fixture.CreateMany<UserCreateModel>(100).ToList();

        //    _userServiceMock.Setup(x => x.ImportFileAsync(It.IsAny<IFormFile>(), It.IsAny<CancellationToken>())).ReturnsAsync(mock);

        //    //Act
        //    var actionResult = await _userController.ImportUserAsync(file, cancellation);
        //    var result = actionResult.Result as CreatedResult;

        //    //Assert
        //    _userServiceMock.Verify(x => x.ImportFileAsync(It.Is<IFormFile>(x => x.Equals(file)), It.Is<CancellationToken>(x => x.Equals(cancellation))), Times.Once());
        //    Assert.NotNull(result);
        //    Assert.NotNull(result!.Value);
        //    Assert.NotEmpty(result.Value.As<IEnumerable<UserCreateModel>>());
        //    Assert.Equal(StatusCodes.Status201Created, result.StatusCode);
        //    result.Value.Should().BeEquivalentTo(mock);
        //}

        ///// <summary>
        ///// Test data of import user async fuction when excel file is invalid
        ///// </summary>
        //public static IEnumerable<object[]> ImportUserAsyncTestData => new List<object[]>
        //{
        //    new object[] { null },
        //    new object[] { new FormFile(new MemoryStream(), 0, 0, "test_form", "test.xlsx") },
        //    new object[] { new FormFile(new MemoryStream(), 0, -1, "test_form", "test.xlsx") }
        //};

        //[Theory]
        //[MemberData(nameof(ImportUserAsyncTestData))]
        //public async Task ImportUserAsync_ShouldReturnNotFoundResult_WhenFileIsInvalid(IFormFile formFile)
        //{
        //    //Assign
        //    IFormFile file = formFile;
        //    var cancellation = new CancellationToken();

        //    //Act
        //    var actionResult = await _userController.ImportUserAsync(file, cancellation);
        //    var result = actionResult.Result as ObjectResult;

        //    //Assert
        //    Assert.NotNull(result);
        //    Assert.IsAssignableFrom<NotFoundObjectResult>(result);
        //    Assert.Equal(StatusCodes.Status404NotFound, result.StatusCode);
        //    Assert.Equal("Your file doesn't exists.", result.Value);
        //}

        //[Fact]
        //public async Task ImportUserAsync_ShouldReturnBadRequestResult_WhenExtensionFileIsInvalid()
        //{
        //    //Assign
        //    var fileName = "test.pdf";
        //    var stream = new MemoryStream();

        //    IFormFile file = new FormFile(stream, 0, 1, "test_form", fileName);
        //    var cancellation = new CancellationToken();

        //    //Act
        //    var actionResult = await _userController.ImportUserAsync(file, cancellation);
        //    var result = actionResult.Result as ObjectResult;

        //    //Assert
        //    Assert.NotNull(result);
        //    Assert.IsAssignableFrom<BadRequestObjectResult>(result);
        //    Assert.Equal(StatusCodes.Status400BadRequest, result.StatusCode);
        //    Assert.Equal("This file doesn't support.", result.Value);
        //}

        //[Fact]
        //public async Task ImportUserAsync_ShouldReturnBadRequestResult_WhenImportUnsucceed()
        //{
        //    //Assign
        //    var fileName = "test.xlsx";
        //    var stream = new MemoryStream();

        //    IFormFile file = new FormFile(stream, 0, 1, "test_form", fileName);
        //    var cancellation = new CancellationToken();

        //    List<UserCreateModel>? users = null;

        //    _userServiceMock.Setup(x => x.ImportFileAsync(It.IsAny<IFormFile>(), It.IsAny<CancellationToken>())).ReturnsAsync(users);

        //    //Act
        //    var actionResult = await _userController.ImportUserAsync(file, cancellation);
        //    var result = actionResult.Result as ObjectResult;

        //    //Assert
        //    _userServiceMock.Verify(x => x.ImportFileAsync(It.Is<IFormFile>(x => x.Equals(file)), It.Is<CancellationToken>(x => x.Equals(cancellation))), Times.Once());
        //    Assert.NotNull(result);
        //    Assert.IsAssignableFrom<BadRequestObjectResult>(result);
        //    Assert.Equal(StatusCodes.Status400BadRequest, result.StatusCode);
        //    Assert.Equal("Import file unsucceed.", result.Value);
        //}
        //#endregion

        //#region List
        //[Fact]
        //public async void ListAsync_UserListExist_ReturnCorrectData()
        //{
        //    // Arrange
        //    var userList = _fixture.Create<List<UserListModel>>();
        //    userList[0].Id = 1;
        //    _userServiceMock.Setup(x => x.GetUserListAsync()).ReturnsAsync(userList);

        //    // Act
        //    var result = await _userController.ListAsync() as OkObjectResult;

        //    // Assert
        //    result.StatusCode.Should().Be(200);
        //    result.Value.Should().BeEquivalentTo(userList);
        //}

        //[Fact]
        //public async void ListAsync_UserListIsEmpty_ReturnNoContent()
        //{
        //    // Arrange       
        //    _userServiceMock.Setup(x => x.GetUserListAsync()).Throws(new InvalidOperationException());

        //    // Act
        //    var result = await _userController.ListAsync() as NoContentResult;

        //    // Assert
        //    result.StatusCode.Should().Be(204);
        //}

        //[Fact]
        //public async void ListAsync_UserServiceThrowException_ReturnInternalServerError()
        //{
        //    // Arrange       
        //    _userServiceMock.Setup(x => x.GetUserListAsync()).ThrowsAsync(new Exception("Internal server error with message"));

        //    // Act
        //    var result = await _userController.ListAsync() as ObjectResult;
        //    // Assert
        //    result.StatusCode.Should().Be(500);
        //    result.Value.Should().Be("Internal server error with message");
        //}
        //#endregion

        //#region EditUserAsync

        //[Fact]
        //public async void EditAsync_AllInputCorrect_ReturnOkAndTrue()
        //{

        //    // Arrange 
        //    var userUpdateModel = _customFixture.Build<UserUpdateModel>().Create();

        //    userUpdateModel.Id = 6;
        //    userUpdateModel.FullName = "Savoy";
        //    userUpdateModel.Phone = "0386472363";
        //    userUpdateModel.DateOfBirth = "21/09/2002";
        //    userUpdateModel.IsMale = "true";
        //    userUpdateModel.RoleId = 1;
        //    userUpdateModel.Level = "AA";
        //    userUpdateModel.Status = "InClass";
        //    userUpdateModel.AvatarURL = "bruh";

        //    var userEntity = _mapperConfig.Map<User>(userUpdateModel);

        //    _userServiceMock.Setup(x => x.GetUserByIdAsync(userUpdateModel.Id)).ReturnsAsync(userEntity);
        //    _userServiceMock.Setup(x => x.ValidateUpdateUserAsync(userUpdateModel)).ReturnsAsync(new ValidationResult());
        //    _userServiceMock.Setup(x => x.EditAsync(userUpdateModel, userEntity)).ReturnsAsync(true);

        //    // Act
        //    var result = await _userController.EditAsync(userUpdateModel.Id, userUpdateModel) as OkObjectResult; 

        //    // Assert
        //    _userServiceMock.Verify(x => x.EditAsync(userUpdateModel, userEntity), Times.Once);
        //    result.Should().BeOfType<OkObjectResult>();
        //    result.Value.Should().NotBeNull();
        //}

        //[Fact]
        //public async void EditAsync_UserIdNotExist_ReturnNotFound()
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

        //    var userEntity = _mapperConfig.Map<User>(userUpdateModel);

        //    _userServiceMock.Setup(x => x.GetUserByIdAsync(userUpdateModel.Id)).ReturnsAsync(userEntity);
        //    _userServiceMock.Setup(x => x.ValidateUpdateUserAsync(userUpdateModel)).ReturnsAsync(new ValidationResult());
        //    _userServiceMock.Setup(x => x.EditAsync(userUpdateModel, userEntity)).ReturnsAsync(false);


        //    // Act
        //    var result = await _userController.EditAsync(userUpdateModel.Id, userUpdateModel) as OkObjectResult;

        //    // Assert
        //    _userServiceMock.Verify(x => x.EditAsync(userUpdateModel, userEntity), Times.Once);
        //    result.Should().BeNull();
        //}

        //#endregion

        //#region DeleteUserAsync
        ////[Fact()]
        //public async void DeleteAsync_UserIDExist_ReturnOk()
        //{

        //    // Arrange
        //    var user = _fixture.Build<User>().Create();

        //    user.Id = 1;

        //    _userServiceMock.Setup(x => x.GetUserByIdAsync(user.Id)).ReturnsAsync(user);
        //    _userServiceMock.Setup(x => x.DeleteAsync(user)).ReturnsAsync(true);

        //    // Act

        //    var result = await _userController.DeleteAsync(user.Id) as OkObjectResult;

        //    // Assert

        //    _userServiceMock.Verify(x => x.DeleteAsync(user), Times.Once);
        //    result.Should().BeOfType<OkObjectResult>();
        //    result.Value.Should().NotBeNull();
        //    result.Value.Should().Be("User with ID " + user.Id + " has been deleted!");
        //}

        //[Fact()]
        //public async void DeleteAsync_UserIDNotExist_ReturnNotFound()
        //{

        //    // Arrange
        //    var user = _fixture.Build<User>().Create();

        //    user.Id = 0;

        //    _userServiceMock.Setup(x => x.GetUserByIdAsync(user.Id)).ReturnsAsync(user);
        //    _userServiceMock.Setup(x => x.DeleteAsync(user)).ReturnsAsync(false);

        //    // Act

        //    var result = await _userController.DeleteAsync(user.Id) as NotFoundObjectResult;

        //    // Assert

        //    _userServiceMock.Verify(x => x.DeleteAsync(user), Times.Once);
        //    result.Should().BeNull();
        //}
        //#endregion
        [Fact]
        public async void FilterUser_ReturnOk()
        {
            //Arrange
            var mock = _fixture.Build<UserFilterModel>().Create();
            var mockUserModel = _fixture.Build<UserListModel>().CreateMany().ToList();

            _userServiceMock.Setup(x => x.Filter(It.IsAny<UserFilterModel>())).ReturnsAsync(mockUserModel);
            //Act
            var result = await _userController.Filter(mock) as OkObjectResult;
            //Assert
            result.Should().BeOfType<OkObjectResult>();
        }
    }
}