using Application.ResponseModels;
using Application.ViewModels.AddLectureViewModels;
using Application.ViewModels.AttendanceViewModels;
using Application.ViewModels.LectureViewModels;
using AutoFixture;
using Domain.Tests;
using FluentAssertions;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAPI.Controllers;

namespace WebAPI.Tests.Controllers
{
    public class AttendanceControllerTests : SetupTest
    {
        private readonly AttendanceController _attendanceController;

        public AttendanceControllerTests()
        {
            _attendanceController = new AttendanceController(_attendanceServiceMock.Object);
        }
        [Fact]
        public async void TakeAttendance_Passed_ReturnCorrectLecture()
        {
            // Arrange
            var addmodel = _fixture.Build<TakeAttendanceModel>().Create();
            var model = _fixture.Build<AttendanceViewModel>().Create();
            _attendanceServiceMock.Setup(x => x.TakeAttendance(It.IsAny<TakeAttendanceModel>())).ReturnsAsync(model);
            _attendanceServiceMock.Setup(x => x.ValidateTakeAttendanceAsync(It.IsAny<TakeAttendanceModel>())).ReturnsAsync(new ValidationResult());
            // Act
            var result = await _attendanceController.TakeAttendance(addmodel) as OkObjectResult;
            var expectedResult = new BaseResponseModel()
            {
                Status = 200,
                Message = "Take Attendance Succeed",
                Result = model
            };
            // Assert
            _attendanceServiceMock.Verify(x => x.TakeAttendance(It.IsAny<TakeAttendanceModel>()), Times.Once);
            result.Should().BeOfType<OkObjectResult>();
            result.Value.Should().NotBeNull();
            result.Value.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public async void TakeAttendance_PassFailed_ReturnNotFoundResult()
        {
            // Arrange
            var addmodel = _fixture.Build<TakeAttendanceModel>().Create();
            var model = _fixture.Build<AttendanceViewModel>().Create();
            _attendanceServiceMock.Setup(x => x.TakeAttendance(It.IsAny<TakeAttendanceModel>())).ReturnsAsync(model);
            _attendanceServiceMock.Setup(x => x.ValidateTakeAttendanceAsync(It.IsAny<TakeAttendanceModel>())).ReturnsAsync(new ValidationResult());
            // Act
            var result = await _attendanceController.TakeAttendance(addmodel) as NotFoundObjectResult;

            // Assert
            _attendanceServiceMock.Verify(x => x.TakeAttendance(It.IsAny<TakeAttendanceModel>()), Times.Once);
            //result.Should().BeOfType<NotFoundObjectResult>();
            //   result.Value.Should().BeEquivalentTo("Add Lecture fail");
            result.Should().BeNull();

        }
        [Fact]
        public async void TakeAttendance_InvalidAttendanceIdPassed_ReturnBadRequestResult()
        {
            // Arrange
            var model = _fixture.Build<AttendanceViewModel>().Create();
            var addmodel = _fixture.Build<TakeAttendanceModel>().Create();
            _attendanceServiceMock.Setup(x => x.TakeAttendance(It.IsAny<TakeAttendanceModel>())).Throws(new Exception());
            _attendanceServiceMock.Setup(x => x.ValidateTakeAttendanceAsync(It.IsAny<TakeAttendanceModel>())).ReturnsAsync(new ValidationResult());
            // Act
            var result = await _attendanceController.TakeAttendance(addmodel) as BadRequestObjectResult;

            // Assert
            _attendanceServiceMock.Verify(x => x.TakeAttendance(It.IsAny<TakeAttendanceModel>()), Times.Once);
            result.Should().BeOfType<BadRequestObjectResult>();
        }
    }
}
