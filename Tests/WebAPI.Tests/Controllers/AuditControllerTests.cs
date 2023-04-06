using Application.ResponseModels;
using Application.ViewModels.AuditViewModels;
using Application.ViewModels.SyllabusViewModels;
using AutoFixture;
using Domain.Tests;
using FluentAssertions;
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
    public class AuditControllerTests : SetupTest
    {
        private readonly AuditController _auditController;

        public AuditControllerTests()
        {
            _auditController = new AuditController(_auditServiceMock.Object);
        }
        #region ControllerTests GetAuditResultDetail
        [Fact]
        public async void GetAuditResulDetail_ExistAuditDetailId_ReturnOkResult()
        {
            // Arrange
            var mockAuditResultDetailModel = _fixture.Build<AuditResultDetailViewModel>().Create();
            _auditServiceMock.Setup(x => x.GetAuditResultDetailAsync(It.IsAny<int>())).ReturnsAsync(mockAuditResultDetailModel);
            var expectedResult = new BaseResponseModel()
            {
                Status = 200,
                Message = "Success",
                Result = mockAuditResultDetailModel
            };
            // Act
            var result = await _auditController.GetAuditResultDetailAsync(1) as OkObjectResult;

            // Assert
            _auditServiceMock.Verify(x => x.GetAuditResultDetailAsync(It.IsAny<int>()), Times.Once);
            result.Should().BeOfType<OkObjectResult>();
            result.Value.Should().NotBeNull();
            result.Value.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public async void GetAuditResulDetail_NotExistAuditDetailId_ReturnNotFoundResult()
        {
            // Arrange
            _auditServiceMock.Setup(x => x.GetAuditResultDetailAsync(It.IsAny<int>())).Throws(new NullReferenceException($"Audit detail not found"));
            var expectedResult = new BaseFailedResponseModel()
            {
                Status = 404,
                Message = "Audit detail not found"
            };
            // Act
            var result = await _auditController.GetAuditResultDetailAsync(1) as NotFoundObjectResult;

            // Assert
            _auditServiceMock.Verify(x => x.GetAuditResultDetailAsync(It.IsAny<int>()), Times.Once);
            result.Should().BeOfType<NotFoundObjectResult>();
            result.Value.Should().NotBeNull();
            result.Value.Should().BeEquivalentTo(expectedResult);
        }

        #endregion

        #region ControllerTests AddAuditDetail
        [Fact]
        public async void AddAuditDetail_Success_ReturnOkResult()
        {
            // arrange
            var mockAuditDetailModel = _fixture.Build<AddAuditDetailViewModel>().Create();
            _auditServiceMock.Setup(x => x.AddAuditDetailAsync(It.IsAny<AddAuditDetailViewModel>())).ReturnsAsync(mockAuditDetailModel);
            var expectedResult = new BaseResponseModel()
            {
                Status = 200,
                Message = "Add audit detail success",
                Result = mockAuditDetailModel
            };

            // act
            var result = await _auditController.AddAuditDetailAsync(mockAuditDetailModel) as OkObjectResult;

            // assert
            _auditServiceMock.Verify(x => x.AddAuditDetailAsync(It.IsAny<AddAuditDetailViewModel>()), Times.Once);
            result.Should().BeOfType<OkObjectResult>();
            result.Value.Should().NotBeNull();
            result.Value.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public async void AddAuditDetail_Fail_ReturnBadRequestResult()
        {
            // Arrange
            var mockAuditDetailModel = _fixture.Build<AddAuditDetailViewModel>().Create();
            _auditServiceMock.Setup(x => x.AddAuditDetailAsync(It.IsAny<AddAuditDetailViewModel>())).Throws(new ArgumentException($"Add audit detail fail"));
            var expectedResult = new BaseFailedResponseModel()
            {
                Status = 400,
                Message = "Add audit detail fail"
            };
            // Act
            var result = await _auditController.AddAuditDetailAsync(mockAuditDetailModel) as BadRequestObjectResult;

            // Assert
            _auditServiceMock.Verify(x => x.AddAuditDetailAsync(It.IsAny<AddAuditDetailViewModel>()), Times.Once);
            result.Should().BeOfType<BadRequestObjectResult>();
            result.Value.Should().NotBeNull();
            result.Value.Should().BeEquivalentTo(expectedResult);
        }
        #endregion

        #region ControllerTests AddAuditPlan
        [Fact]
        public async void AddAuditPlan_Success_ReturnOkResult()
        {
            // arrange
            var mockAuditPlanModel = _fixture.Build<AddAuditPlanViewModel>().Create();
            _auditServiceMock.Setup(x => x.AddAuditPlanAsync(It.IsAny<AddAuditPlanViewModel>())).ReturnsAsync(mockAuditPlanModel);
            var expectedResult = new BaseResponseModel()
            {
                Status = 200,
                Message = "Add audit plan success",
                Result = mockAuditPlanModel
            };

            // act
            var result = await _auditController.AddAuditPlanAsync(mockAuditPlanModel) as OkObjectResult;

            // assert
            _auditServiceMock.Verify(x => x.AddAuditPlanAsync(It.IsAny<AddAuditPlanViewModel>()), Times.Once);
            result.Should().BeOfType<OkObjectResult>();
            result.Value.Should().NotBeNull();
            result.Value.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public async void AddAuditPlan_Fail_ReturnBadRequestResult()
        {
            // Arrange
            var mockAuditPlanModel = _fixture.Build<AddAuditPlanViewModel>().Create();
            _auditServiceMock.Setup(x => x.AddAuditPlanAsync(It.IsAny<AddAuditPlanViewModel>())).Throws(new ArgumentException($"Add audit plan fail"));
            var expectedResult = new BaseFailedResponseModel()
            {
                Status = 400,
                Message = "Add audit plan fail"
            };
            // Act
            var result = await _auditController.AddAuditPlanAsync(mockAuditPlanModel) as BadRequestObjectResult;

            // Assert
            _auditServiceMock.Verify(x => x.AddAuditPlanAsync(It.IsAny<AddAuditPlanViewModel>()), Times.Once);
            result.Should().BeOfType<BadRequestObjectResult>();
            result.Value.Should().NotBeNull();
            result.Value.Should().BeEquivalentTo(expectedResult);
        }
        #endregion

        #region ControllerTests AddAuditResult
        [Fact]
        public async void AddAuditResult_Success_ReturnOkResult()
        {
            // arrange
            var mockAuditResultModel = _fixture.Build<AddAuditResultViewModel>().Create();
            _auditServiceMock.Setup(x => x.AddAuditResultAsync(It.IsAny<AddAuditResultViewModel>())).ReturnsAsync(mockAuditResultModel);
            var expectedResult = new BaseResponseModel()
            {
                Status = 200,
                Message = "Add audit result success",
                Result = mockAuditResultModel
            };

            // act
            var result = await _auditController.AddAuditResultAsync(mockAuditResultModel) as OkObjectResult;

            // assert
            _auditServiceMock.Verify(x => x.AddAuditResultAsync(It.IsAny<AddAuditResultViewModel>()), Times.Once);
            result.Should().BeOfType<OkObjectResult>();
            result.Value.Should().NotBeNull();
            result.Value.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public async void AddAuditResult_Fail_ReturnBadRequestResult()
        {
            // Arrange
            var mockAuditResultModel = _fixture.Build<AddAuditResultViewModel>().Create();
            _auditServiceMock.Setup(x => x.AddAuditResultAsync(It.IsAny<AddAuditResultViewModel>())).Throws(new ArgumentException($"Add audit result fail"));
            var expectedResult = new BaseFailedResponseModel()
            {
                Status = 400,
                Message = "Add audit result fail"
            };
            // Act
            var result = await _auditController.AddAuditResultAsync(mockAuditResultModel) as BadRequestObjectResult;

            // Assert
            _auditServiceMock.Verify(x => x.AddAuditResultAsync(It.IsAny<AddAuditResultViewModel>()), Times.Once);
            result.Should().BeOfType<BadRequestObjectResult>();
            result.Value.Should().NotBeNull();
            result.Value.Should().BeEquivalentTo(expectedResult);
        }
        #endregion

        #region Update Feedback Audit Deatil
        [Fact]
        public async void UpdateFeedbackAuditDeatail_ReturnOk()
        {
            //Arrange
            var mock = _fixture.Build<AuditResultDetailViewModel>().Create();
            _auditServiceMock.Setup(x => x.UpdateFeedbackAuditDeatilsAsync(It.IsAny<int>(), It.IsAny<string>())).ReturnsAsync(mock);
            //Act
            var result = await _auditController.UpdateFeedbackAuditDeatilsAsync(1, "ew") as OkObjectResult;
            //Assert
            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async void UpdateFeedbackAuditDeatail_ReturnBadRequest()
        {
            //Arrange
            //var mock = _fixture.Build<AuditResultDetailViewModel>().Create();
            _auditServiceMock.Setup(x => x.UpdateFeedbackAuditDeatilsAsync(It.IsAny<int>(), It.IsAny<string>())).ThrowsAsync( new NullReferenceException());
            //Act
            var result = await _auditController.UpdateFeedbackAuditDeatilsAsync(1, "ew") as NotFoundObjectResult;
            //Assert
            result.Should().BeOfType<NotFoundObjectResult>();
        }
        #endregion
    }
}
