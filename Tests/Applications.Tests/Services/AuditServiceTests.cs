using Application.Interfaces;
using Application.Services;
using Application.ViewModels.AuditViewModels;
using AutoFixture;
using Domain.Models;
using Domain.Models.Syllabuses;
using Domain.Tests;
using FluentAssertions;
using Infracstructures.Mappers;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Tests.Services
{
    public class AuditServiceTests : SetupTest
    {
        private readonly IAuditService _auditService;
        private Fixture _customFixture;

        public AuditServiceTests()
        {
            _auditService = new AuditService(_unitOfWorkMock.Object, _mapperConfig, _claimsServiceMock.Object, _currentTimeMock.Object, _configuration);
            _customFixture = new Fixture();
            _customFixture.Customizations.Add(new TypePropertyOmitter(
                "AuditPlan", "Auditor", "Trainee", "Results", "AuditPlan"
                ));
        }

        #region ServiceTests  GetAuditResultDetail
        [Fact]
        public async void GetAuditResultDetailAsync_AuditDetailIdExist_ReturnCorrectAuditResultDetail()
        {
            // arrange
            var expectedResult = _fixture.Build<AuditResultDetailViewModel>().Create();
            expectedResult.Date = "24/01/2015";
            expectedResult.Status = "Pass";
            var mockAuditDetail = _mapperConfig.Map<AuditDetail>(expectedResult);
            mockAuditDetail.AuditPlan.Class.Code = expectedResult.ClassCode;
            mockAuditDetail.AuditPlan.Syllabus.Code = expectedResult.SyllabusCode;
            mockAuditDetail.Auditor.FullName = expectedResult.AuditorName;
            mockAuditDetail.Trainee.FullName = expectedResult.TraineeName;
            mockAuditDetail.AuditPlan.Location = expectedResult.Location;
            mockAuditDetail.AuditPlan.AuditDate = DateTime.ParseExact(expectedResult.Date, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            _unitOfWorkMock.Setup(x => x.AuditDetailRepo.GetByIdAsync(mockAuditDetail.Id))
                .ReturnsAsync(mockAuditDetail);
            // act
            var result = await _auditService.GetAuditResultDetailAsync(mockAuditDetail.Id);
            expectedResult.NumOfQuestion = result.NumOfQuestion;
            expectedResult.Results = result.Results;
            // assert
            _unitOfWorkMock.Verify(x => x.AuditDetailRepo.GetByIdAsync(mockAuditDetail.Id), Times.Once());
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public void GetAuditResultDetailAsync_AuditDetailIdNotExist_ThrowNullReferenceException()
        {
            // arrange
            AuditDetail? mockNullAuditDetail = null;
            _unitOfWorkMock.Setup(x => x.AuditDetailRepo.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(mockNullAuditDetail);

            // act and assert
            var result =
                Assert.ThrowsAsync<NullReferenceException>(async () =>
                    await _auditService.GetAuditResultDetailAsync(1));
            _unitOfWorkMock.Verify(x => x.AuditDetailRepo.GetByIdAsync(1), Times.Once());
            Assert.Equal("Audit detail not found", result.Result.Message);
        }

        #endregion

        #region ServiceTests AddAuditDetail
        [Fact]
        public async void AddAuditDetailAsync_Success_ReturnCorrectAddAuditDetailModel()
        {
            // arrange
            var expectedResult = _fixture.Build<AddAuditDetailViewModel>().Create();
            _unitOfWorkMock.Setup(x => x.AuditDetailRepo.AddAsync(It.IsAny<AuditDetail>())).Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(x => x.SaveChangesAsync()).ReturnsAsync(1);
            //Act
            var result = await _auditService.AddAuditDetailAsync(expectedResult);

            //Assert
            _unitOfWorkMock.Verify(x => x.AuditDetailRepo.AddAsync(It.IsAny<AuditDetail>()), Times.Once());
            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(), Times.Once());
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public async void AddAuditDetailAsync_Fail_ThrowArgumentException()
        {
            // arrange
            var mockAuditDetailAddModel = _fixture.Build<AddAuditDetailViewModel>().Create();
            _unitOfWorkMock.Setup(x => x.AuditDetailRepo.AddAsync(It.IsAny<AuditDetail>())).Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(x => x.SaveChangesAsync()).Throws<DbUpdateException>();

            //Act an assert
            var result = Assert.ThrowsAsync<ArgumentException>(async () => await _auditService.AddAuditDetailAsync(mockAuditDetailAddModel));
            _unitOfWorkMock.Verify(x => x.AuditDetailRepo.AddAsync(It.IsAny<AuditDetail>()), Times.Once());
            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(), Times.Once());
            Assert.Equal("Add audit detail fail", result.Result.Message);
        }
        #endregion

        #region ServiceTests AddAuditPlan
        [Fact]
        public async void AddAuditPlanAsync_Success_ReturnCorrectAddAuditPlanModel()
        {
            // arrange
            var expectedResult = _fixture.Build<AddAuditPlanViewModel>().Create();
            _unitOfWorkMock.Setup(x => x.AuditPlanRepo.AddAsync(It.IsAny<AuditPlan>())).Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(x => x.SaveChangesAsync()).ReturnsAsync(1);
            //Act
            var result = await _auditService.AddAuditPlanAsync(expectedResult);

            //Assert
            _unitOfWorkMock.Verify(x => x.AuditPlanRepo.AddAsync(It.IsAny<AuditPlan>()), Times.Once());
            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(), Times.Once());
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public async void AddAuditPlanAsync_Fail_ThrowArgumentException()
        {
            // arrange
            var mockAuditPlanAddModel = _fixture.Build<AddAuditPlanViewModel>().Create();
            _unitOfWorkMock.Setup(x => x.AuditPlanRepo.AddAsync(It.IsAny<AuditPlan>())).Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(x => x.SaveChangesAsync()).Throws<DbUpdateException>();

            //Act an assert
            var result = Assert.ThrowsAsync<ArgumentException>(async () => await _auditService.AddAuditPlanAsync(mockAuditPlanAddModel));
            _unitOfWorkMock.Verify(x => x.AuditPlanRepo.AddAsync(It.IsAny<AuditPlan>()), Times.Once());
            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(), Times.Once());
            Assert.Equal("Add audit plan fail", result.Result.Message);
        }
        #endregion

        #region ServiceTests AddAuditResult
        [Fact]
        public async void AddAuditResultAsync_Success_ReturnCorrectAddAuditResultModel()
        {
            // arrange
            var expectedResult = _fixture.Build<AddAuditResultViewModel>().Create();
            _unitOfWorkMock.Setup(x => x.AuditResultRepo.AddAsync(It.IsAny<AuditResult>())).Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(x => x.SaveChangesAsync()).ReturnsAsync(1);
            //Act
            var result = await _auditService.AddAuditResultAsync(expectedResult);

            //Assert
            _unitOfWorkMock.Verify(x => x.AuditResultRepo.AddAsync(It.IsAny<AuditResult>()), Times.Once());
            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(), Times.Once());
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public async void AddAuditResultAsync_Fail_ThrowArgumentException()
        {
            // arrange
            var mockAuditResultAddModel = _fixture.Build<AddAuditResultViewModel>().Create();
            _unitOfWorkMock.Setup(x => x.AuditResultRepo.AddAsync(It.IsAny<AuditResult>())).Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(x => x.SaveChangesAsync()).Throws<DbUpdateException>();

            //Act an assert
            var result = Assert.ThrowsAsync<ArgumentException>(async () => await _auditService.AddAuditResultAsync(mockAuditResultAddModel));
            _unitOfWorkMock.Verify(x => x.AuditResultRepo.AddAsync(It.IsAny<AuditResult>()), Times.Once());
            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(), Times.Once());
            Assert.Equal("Add audit result fail", result.Result.Message);
        }
        #endregion

        #region ServiceTest UpdateFeedbackAuditDeatil
        [Fact]
        public async void UpdateFeedbackAuditDeatil_ReturnAudtiViewModel()
        {
            //Arrange
            var mock = _customFixture.Build<AuditDetail>().Create();
            _unitOfWorkMock.Setup(x => x.AuditDetailRepo.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(mock);
            _unitOfWorkMock.Setup(x => x.AuditDetailRepo.Update(It.IsAny<AuditDetail>()));
            _unitOfWorkMock.Setup(x => x.SaveChangesAsync()).ReturnsAsync(1);
            //Act
            var result = await _auditService.UpdateFeedbackAuditDeatilsAsync(1, "sd");
            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<AuditResultDetailViewModel>();
        }

        [Fact]
        public async void UpdateFeedbackAuditDeatil_ReturnNullReferenceException_WhenNotFoundAudit()
        {
            //Arrange
            var mock = _customFixture.Build<AuditDetail>().Create();
            _unitOfWorkMock.Setup(x => x.AuditDetailRepo.GetByIdAsync(It.IsAny<int>()));
            //Act
            var result = async () => await _auditService.UpdateFeedbackAuditDeatilsAsync(1, "22");
            //Assert
            await result.Should().ThrowAsync<NullReferenceException>().WithMessage("AuditDetail not found");
        }
        #endregion
    }
}
