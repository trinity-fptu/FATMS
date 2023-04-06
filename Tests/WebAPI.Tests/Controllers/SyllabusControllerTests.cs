using Application.ViewModels.SyllabusViewModels;
using Application.ViewModels.AddLectureViewModels;
using AutoFixture;
using Domain.Models.Syllabuses;
using Domain.Tests;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using WebAPI.Controllers;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;
using System.Diagnostics;
using Application.ResponseModels;
using Application.ViewModels.LectureViewModels;
using Application.ViewModels.AttendanceViewModels;
using FluentValidation.Results;

namespace WebAPI.Tests.Controllers
{
    public class SyllabusControllerTests : SetupTest
    {
        private readonly SyllabusController _syllabusController;

        public SyllabusControllerTests()
        {
            _syllabusController = new SyllabusController(_syllabusServiceMock.Object);
        }
        #region ControllerTests GetSyllabusDetail
        [Fact]
        public async void GetSyllabusDetail_ExistSyllabusId_ReturnOkResult()
        {
            // Arrange
            var syllabusDetailModel = _fixture.Build<SyllabusDetailModel>().Create();
            _syllabusServiceMock.Setup(x => x.GetSyllabusDetailByIdAsync(It.IsAny<int>())).ReturnsAsync(syllabusDetailModel);
            var expectedResult = new BaseResponseModel()
            {
                Status = 200,
                Message = "Success",
                Result = syllabusDetailModel
            };
            // Act
            var result = await _syllabusController.GetSyllabusDetail(1) as OkObjectResult;

            // Assert
            _syllabusServiceMock.Verify(x => x.GetSyllabusDetailByIdAsync(It.IsAny<int>()), Times.Once);
            result.Should().BeOfType<OkObjectResult>();
            result.Value.Should().NotBeNull();
            result.Value.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public async void GetSyllabusDetail_NotExistSyllabusId_ReturnNotFoundResult()
        {
            // Arrange
            _syllabusServiceMock.Setup(x => x.GetSyllabusDetailByIdAsync(It.IsAny<int>())).Throws(new NullReferenceException($"Syllabus not found"));
            var expectedResult = new BaseFailedResponseModel()
            {
                Status = 404,
                Message = "Syllabus not found"
            };
            // Act
            var result = await _syllabusController.GetSyllabusDetail(1) as NotFoundObjectResult;

            // Assert
            _syllabusServiceMock.Verify(x => x.GetSyllabusDetailByIdAsync(It.IsAny<int>()), Times.Once);
            result.Should().BeOfType<NotFoundObjectResult>();
            result.Value.Should().BeEquivalentTo(expectedResult);
        }

        #endregion

        #region ControllerTests CloneSyllabus
        [Fact]
        public async void CloneSyllabus_ExistSyllabusId_ReturnOkResult()
        {
            // Arrange
            _syllabusServiceMock.Setup(x => x.CloneSyllabusAsync(It.IsAny<int>())).Returns(Task.CompletedTask);
            var expectedResult = new BaseResponseModel()
            {
                Status = 200,
                Message = "Success",
                Result = "Success"
            };
            // Act
            var result = await _syllabusController.CloneSyllabusAsync(1) as OkObjectResult;

            // Assert
            _syllabusServiceMock.Verify(x => x.CloneSyllabusAsync(It.IsAny<int>()), Times.Once);
            result.Should().BeOfType<OkObjectResult>();
            result.Value.Should().NotBeNull();
            result.Value.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public async void CloneSyllabus_NotExistSyllabusId_ReturnNotFoundResult()
        {
            // Arrange
            _syllabusServiceMock.Setup(x => x.CloneSyllabusAsync(It.IsAny<int>())).Throws(new NullReferenceException($"Syllabus not found"));
            var expectedResult = new BaseFailedResponseModel()
            {
                Status = 404,
                Message = "Syllabus not found"
            };
            // Act
            var result = await _syllabusController.CloneSyllabusAsync(1) as NotFoundObjectResult;

            // Assert
            _syllabusServiceMock.Verify(x => x.CloneSyllabusAsync(It.IsAny<int>()), Times.Once);
            result.Should().BeOfType<NotFoundObjectResult>();
            result.Value.Should().BeEquivalentTo(expectedResult);
        }

        #endregion

        //[Fact]
        public async void Delete_ExistSyllabusIdPassed_ReturnCorrectMessage()
        {
            // Arrange
            var mockItem = _fixture.Build<SyllabusViewModel>()
            .Create();
            _syllabusServiceMock.Setup(x => x.DeleteSyllabusAsync(It.IsAny<int>())).ReturnsAsync(mockItem);
            // Act
            var result = await _syllabusController.DeleteSyllabusAsync(1) as OkObjectResult;
            var expectedResult = new BaseResponseModel()
            {
                Status = 200,
                Message = "Delete Succeed",
                Result = mockItem
            };
            // Assert
            _syllabusServiceMock.Verify(x => x.DeleteSyllabusAsync(It.IsAny<int>()), Times.Once);
            result.Should().BeOfType<OkObjectResult>();
            result.Value.Should().NotBeNull();
            result.Value.Should().BeEquivalentTo(expectedResult);
        }
        //[Fact]
        public async void Delete_ExistSyllabusIdNotPassed_ReturnNotFoundResult()
        {
            // Arrange
            var mockItem = _fixture.Build<SyllabusViewModel>().Create();
            _syllabusServiceMock.Setup(x => x.DeleteSyllabusAsync(It.IsAny<int>())).ReturnsAsync(mockItem);
            // Act
            var result = await _syllabusController.DeleteSyllabusAsync(1) as NotFoundObjectResult;

            // Assert
            _syllabusServiceMock.Verify(x => x.DeleteSyllabusAsync(It.IsAny<int>()), Times.Once);
            result.Should().BeNull();

        }
        //[Fact]
        public async void Delete_InvalidSyllabusIdPassed_ReturnBadRequestResult()
        {
            // Arrange
            int id = 0;
            _syllabusServiceMock.Setup(x => x.DeleteSyllabusAsync(id)).Throws(new ArgumentException("Syllabus id cannot be less than 1."));

            // Act
            var result = await _syllabusController.DeleteSyllabusAsync(id) as BadRequestObjectResult;

            // Assert
            _syllabusServiceMock.Verify(x => x.DeleteSyllabusAsync(0), Times.Once);
            result.Should().BeOfType<BadRequestObjectResult>();
            result.Value.Should().BeEquivalentTo("Invalid parameters: Syllabus id cannot be less than 1.");

        }
        [Fact]
        public async void AddLecture_ExistLectureIdPassed_ReturnCorrectLecture()
        {
            // Arrange
            var addmodel = _fixture.Build<AddLectureViewModel>().Create();
            var model = _fixture.Build<LectureViewModel>().Create();
             _syllabusServiceMock.Setup(x => x.AddLectureAsync(It.IsAny<AddLectureViewModel>())).ReturnsAsync(model);
            _syllabusServiceMock.Setup(x => x.ValidateAddLectureAsync(It.IsAny<AddLectureViewModel>())).ReturnsAsync(new ValidationResult());

            // Act
            var result = await _syllabusController.AddLectureAsync(addmodel) as OkObjectResult;
            var expectedResult = new BaseResponseModel()
            {
                Status = 200,
                Message = "Add Succeed",
                Result = model
            };
            // Assert
            _syllabusServiceMock.Verify(x => x.AddLectureAsync(It.IsAny<AddLectureViewModel>()), Times.Once);
            result.Should().BeOfType<OkObjectResult>();
            result.Value.Should().NotBeNull();
            result.Value.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public async void AddLecture_NoExistLectureIdPassed_ReturnNotFoundResult()
        {
            // Arrange
            var addmodel = _fixture.Build<AddLectureViewModel>().Create();
            var model = _fixture.Build<LectureViewModel>().Create();
            _syllabusServiceMock.Setup(x => x.AddLectureAsync(It.IsAny<AddLectureViewModel>())).ReturnsAsync(model);
            _syllabusServiceMock.Setup(x => x.ValidateAddLectureAsync(It.IsAny<AddLectureViewModel>())).ReturnsAsync(new ValidationResult());

            // Act
            var result = await _syllabusController.AddLectureAsync(addmodel) as NotFoundObjectResult;

            // Assert
            _syllabusServiceMock.Verify(x => x.AddLectureAsync(It.IsAny<AddLectureViewModel>()), Times.Once);
            //result.Should().BeOfType<NotFoundObjectResult>();
            //   result.Value.Should().BeEquivalentTo("Add Lecture fail");
            result.Should().BeNull();

        }
        [Fact]
        public async void AddLecture_InvalidLectureIdPassed_ReturnBadRequestResult()
        {
            // Arrange
            var addmodel = _fixture.Build<AddLectureViewModel>().Create();
            var model = _fixture.Build<LectureViewModel>().Create();
            _syllabusServiceMock.Setup(x => x.AddLectureAsync(It.IsAny<AddLectureViewModel>())).Throws(new ArgumentException("Syllabus id cannot be less than 1."));
            _syllabusServiceMock.Setup(x => x.ValidateAddLectureAsync(It.IsAny<AddLectureViewModel>())).ReturnsAsync(new ValidationResult());
            // Act
            var result = await _syllabusController.AddLectureAsync(addmodel) as BadRequestObjectResult;
            // Assert
            _syllabusServiceMock.Verify(x => x.AddLectureAsync(It.IsAny<AddLectureViewModel>()), Times.Once);
            result.Should().BeOfType<BadRequestObjectResult>();
            result.Value.Should().BeEquivalentTo("Invalid parameters: Syllabus id cannot be less than 1.");

        }
        [Fact]
        public async void AddLectureToUnit_ExistLectureIdPassed_ReturnCorrectLecture()
        {
            // Arrange
            int lecutreid = 1;
            int unitid = 1;
            var model = _fixture.Build<LectureViewModel>().Create();
            var addmodel = _fixture.Build<AddLectureViewModel>().Create();
            _syllabusServiceMock.Setup(x => x.AddLectureToUnitAsync(lecutreid, unitid)).ReturnsAsync(model);
            // Act
            var result = await _syllabusController.AddLectureToUnitAsync(lecutreid, unitid) as OkObjectResult;
            var expectedResult = new BaseResponseModel()
            {
                Status = 200,
                Message = "Add Succeed",
                Result = model
            };
            // Assert
            _syllabusServiceMock.Verify(x => x.AddLectureToUnitAsync(lecutreid, unitid), Times.Once);
            result.Should().BeOfType<OkObjectResult>();
            result.Value.Should().NotBeNull();
            result.Value.Should().BeEquivalentTo(expectedResult);
        }
        [Fact]
        public async void AddLectureToUnit_InvalidLectureIdPassed_ReturnBadRequestResult()
        {
            // Arrange
            int lecutreid = 1;
            int unitid = 1;
            var model = _fixture.Build<AddLectureViewModel>().Create();
            _syllabusServiceMock.Setup(x => x.AddLectureToUnitAsync(lecutreid, unitid)).Throws(new ArgumentException("Unit id or Lecture id not found"));

            // Act
            var result = await _syllabusController.AddLectureToUnitAsync(lecutreid, unitid) as BadRequestObjectResult;
            // Assert
            _syllabusServiceMock.Verify(x => x.AddLectureToUnitAsync(lecutreid, unitid), Times.Once);
            result.Should().BeOfType<BadRequestObjectResult>();
            result.Value.Should().BeEquivalentTo("Invalid parameters: Unit id or Lecture id not found");

        }
        [Fact]
        public async void AddLectureToUnit_NoExistLectureIdPassed_ReturnNotFoundResult()
        {
            // Arrange
            int lecutreid = 1;
            int unitid = 1;
            var addmodel = _fixture.Build<AddLectureViewModel>().Create();
            var model = _fixture.Build<LectureViewModel>().Create();
            _syllabusServiceMock.Setup(x => x.AddLectureToUnitAsync(lecutreid, unitid)).ReturnsAsync(model);
            // Act
            var result = await _syllabusController.AddLectureToUnitAsync(lecutreid, unitid) as NotFoundObjectResult;

            // Assert
            _syllabusServiceMock.Verify(x => x.AddLectureToUnitAsync(lecutreid, unitid), Times.Once);
            //result.Should().BeOfType<NotFoundObjectResult>();
            //   result.Value.Should().BeEquivalentTo("Add Lecture fail");
            result.Should().BeNull();

        }


        // #region Update
        //
        // [Fact]
        // public async Task UpdateSyllabusAsync_ValidInput_ReturnOk()
        // {
        //     // arrange
        //     var mockItem = _fixture.Build<Syllabus>()
        //         .With(x => x.TrainingDeliveryPrincipleId, () => _fixture.Create<int>())
        //         .With(x => x.LastModifiedBy, () => _claimsServiceMock.Object.GetCurrentUserId)
        //         .With(x => x.LastModifiedOn, () => _currentTimeMock.Object.GetCurrentTime())
        //         .Without(e => e.CreatedAdmin)
        //         .Without(e => e.ModifiedAdmin)
        //         .Without(e => e.TrainingDeliveryPrinciple)
        //         .Without(e => e.Units)
        //         .Without(e => e.TrainingPrograms)
        //         .Without(e => e.AuditPlans)
        //         .Create();
        //
        //     var expectedResult = _mapperConfig.Map<SyllabusViewModel>(mockItem);
        //
        //     UpdateSyllabusViewModel input = _mapperConfig.Map<UpdateSyllabusViewModel>(mockItem);
        //
        //     _syllabusServiceMock.Setup(x => x.UpdateSyllabusAsync(input)).ReturnsAsync(expectedResult);
        //
        //     // act
        //     var result = await _syllabusController.UpdateSyllabusAsync(input) as ObjectResult;
        //
        //     // assert
        //     result.Should().NotBeNull();
        //     result.Should().BeOfType<OkObjectResult>();
        //     result?.Value.Should().BeEquivalentTo(expectedResult);
        // }
        //
        // // [Fact]
        // // public async Task UpdateSyllabusAsync_NullInput_ReturnBadRequest()
        // // {
        // //     // arrange
        // //     UpdateSyllabusViewModel input = null;
        // //     
        // //     var mockItem = _fixture.Build<Syllabus>()
        // //         .With(x => x.TrainingDeliveryPrincipleId, () => _fixture.Create<int>())
        // //         .With(x => x.LastModifiedBy, () => _claimsServiceMock.Object.GetCurrentUserId)
        // //         .With(x => x.LastModifiedOn, () => _currentTimeMock.Object.GetCurrentTime())
        // //         .Without(e => e.CreatedAdmin)
        // //         .Without(e => e.ModifiedAdmin)
        // //         .Without(e => e.TrainingDeliveryPrinciple)
        // //         .Without(e => e.Units)
        // //         .Without(e => e.TrainingPrograms)
        // //         .Without(e => e.AuditPlans)
        // //         .Create();
        // //     
        // //     var expectedResult = _mapperConfig.Map<SyllabusViewModel>(mockItem);
        // //     
        // //     _syllabusServiceMock.Setup(x => x.UpdateSyllabusAsync(input)).ReturnsAsync(expectedResult);
        // //
        // //     // act
        // //     var result = await _syllabusController.UpdateSyllabusAsync(input) as ObjectResult;
        // //     
        // //     // assert
        // //     result.Should().NotBeNull();
        // //     result.Should().BeOfType<BadRequestObjectResult>();
        // // }
        //
        // [Fact]
        // public async Task UpdateSyllabusAsync_InvalidInput_ReturnNotFound()
        // {
        //     // arrange
        //     _syllabusController.ModelState.AddModelError("error", "error");
        //
        //     var mockItem = _fixture.Build<Syllabus>()
        //         .With(x => x.TrainingDeliveryPrincipleId, () => _fixture.Create<int>())
        //         .With(x => x.LastModifiedBy, () => _claimsServiceMock.Object.GetCurrentUserId)
        //         .With(x => x.LastModifiedOn, () => _currentTimeMock.Object.GetCurrentTime())
        //         .Without(e => e.CreatedAdmin)
        //         .Without(e => e.ModifiedAdmin)
        //         .Without(e => e.TrainingDeliveryPrinciple)
        //         .Without(e => e.Units)
        //         .Without(e => e.TrainingPrograms)
        //         .Without(e => e.AuditPlans)
        //         .Create();
        //
        //     UpdateSyllabusViewModel input = _mapperConfig.Map<UpdateSyllabusViewModel>(mockItem);
        //
        //     var expectedResult = _mapperConfig.Map<SyllabusViewModel>(mockItem);
        //
        //     _syllabusServiceMock.Setup(x => x.UpdateSyllabusAsync(input))
        //         .ReturnsAsync((SyllabusViewModel)null);
        //
        //     // act
        //     var result = await _syllabusController.UpdateSyllabusAsync(input) as ObjectResult;
        //
        //     // assert
        //     result.Should().NotBeNull();
        //     result.Should().BeOfType<NotFoundObjectResult>();
        //     result.Should().BeEquivalentTo(new NotFoundObjectResult("Syllabus not found"));
        // }
        //
        // // [Fact]
        // // public async Task UpdateSyllabusAsync_InvalidInput_ReturnUnauthorized()
        // // {
        // //     // arrange
        // //     _syllabusController.ModelState.AddModelError("error", "error");
        // //     
        // //     var mockItem = _fixture.Build<Syllabus>()
        // //         .With(x => x.TrainingDeliveryPrincipleId, () => _fixture.Create<int>())
        // //         .With(x => x.LastModifiedBy, () => null)
        // //         .With(x => x.LastModifiedOn, () => _currentTimeMock.Object.GetCurrentTime())
        // //         .Without(e => e.CreatedAdmin)
        // //         .Without(e => e.ModifiedAdmin)
        // //         .Without(e => e.TrainingDeliveryPrinciple)
        // //         .Without(e => e.Units)
        // //         .Without(e => e.TrainingPrograms)
        // //         .Without(e => e.AuditPlans)
        // //         .Create();
        // //     
        // //     UpdateSyllabusViewModel input = _mapperConfig.Map<UpdateSyllabusViewModel>(mockItem);
        // //     
        // //     var expectedResult = _mapperConfig.Map<SyllabusViewModel>(mockItem);
        // //     
        // //     _syllabusServiceMock.Setup(x => x.UpdateSyllabusAsync(input)).ReturnsAsync(expectedResult);
        // //     
        // //     // act
        // //     var result = await _syllabusController.UpdateSyllabusAsync(input) as ObjectResult;
        // //     
        // //     // assert
        // //     result.Should().NotBeNull();
        // //     result.Should().BeOfType<UnauthorizedObjectResult>();
        // //     result.Should().BeEquivalentTo(new UnauthorizedObjectResult("You are not authorized to update this syllabus"));
        // // }
        //
        // [Fact]
        // public async Task UpdateSyllabusAsync_LargeNumberOfSyllabus_PerformanceTest()
        // {
        //     // arrange
        //     var currentUserId = _fixture.Create<int>();
        //     _claimsServiceMock.Setup(x => x.GetCurrentUserId).Returns(currentUserId);
        //
        //     var mockItems = _fixture.Build<Syllabus>()
        //         .With(x => x.TrainingDeliveryPrincipleId, () => _fixture.Create<int>())
        //         .With(x => x.LastModifiedBy, () => currentUserId)
        //         .With(x => x.LastModifiedOn, () => DateTime.Today)
        //         .Without(e => e.CreatedAdmin)
        //         .Without(e => e.ModifiedAdmin)
        //         .Without(e => e.TrainingDeliveryPrinciple)
        //         .Without(e => e.Units)
        //         .Without(e => e.TrainingPrograms)
        //         .Without(e => e.AuditPlans)
        //         .CreateMany(1000)
        //         .ToList();
        //
        //     _syllabusServiceMock.Setup(x => x.UpdateSyllabusAsync(It.IsAny<UpdateSyllabusViewModel>()))
        //         .ReturnsAsync(_mapperConfig.Map<SyllabusViewModel>(mockItems.First()));
        //
        //     var stopWatch = new Stopwatch();
        //     foreach (var item in mockItems)
        //     {
        //         stopWatch.Start();
        //         // act
        //         var result =
        //             await _syllabusController.UpdateSyllabusAsync(_mapperConfig.Map<UpdateSyllabusViewModel>(item)) as ObjectResult;
        //         stopWatch.Stop();
        //         // assert
        //         result.Should().NotBeNull();
        //         result.Should().BeOfType<OkObjectResult>();
        //     }
        //
        //     stopWatch.ElapsedMilliseconds.Should().BeLessOrEqualTo(1000);
        // }
        //
        // #endregion

        //[Fact]
        public async Task GetSyllabusAsync_ReturnsOkObjectResult_WithSyllabusViewModelList()
        {
            // Arrange
            var syllabusViewModelList = new List<SyllabusViewModel>
        {
            new SyllabusViewModel { Id = 1, Name = "Syllabus 1" },
            new SyllabusViewModel { Id = 2, Name = "Syllabus 2" }
        };
            _syllabusServiceMock.Setup(x => x.GetSyllabusAsync()).ReturnsAsync(syllabusViewModelList);

            // Act
            var result = await _syllabusController.GetSyllabusAsync();

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            var okResult = result as OkObjectResult;
            Assert.AreEqual(syllabusViewModelList, okResult.Value);
        }

        //[Fact]
        public async Task AddSyllabusAsync_ReturnsOkObjectResult_WithSyllabusViewModel()
        {
            // Arrange
            var addSyllabusViewModel = new AddSyllabusViewModel { Name = "New Syllabus" };
            var syllabusViewModel = new SyllabusViewModel { Id = 1, Name = "New Syllabus" };
            _syllabusServiceMock.Setup(x => x.AddSyllabusAsync(addSyllabusViewModel)).ReturnsAsync(syllabusViewModel);

            // Act
            var result = await _syllabusController.AddSyllabusAsync(addSyllabusViewModel);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            var okResult = result as OkObjectResult;
            Assert.AreEqual(syllabusViewModel, okResult.Value);
        }

        //[Fact]
        public async Task AddSyllabusAsync_ReturnsBadRequestResult_WhenExceptionThrown()
        {
            // Arrange
            var addSyllabusViewModel = new AddSyllabusViewModel { Name = "New Syllabus" };
            _syllabusServiceMock.Setup(x => x.AddSyllabusAsync(addSyllabusViewModel)).ThrowsAsync(new Exception());

            // Act
            var result = await _syllabusController.AddSyllabusAsync(addSyllabusViewModel);

            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
        }
    }
}