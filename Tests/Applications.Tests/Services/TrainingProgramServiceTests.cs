using Application.Interfaces;
using Application.Services;
using Application.ViewModels.TrainingProgramViewModels;
using AutoFixture;
using Domain.Models;
using FluentAssertions;
using Domain.Tests;
using Moq;
using Application.ViewModels.UserViewModels;
using Domain.Enums.UserEnums;
using Domain.Models.Users;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient.DataClassification;
using Xunit.Sdk;
using MimeKit.Cryptography;
using Domain.Models.Syllabuses;
using Application.ViewModels.SyllabusViewModels;
using AutoFixture.Kernel;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace Applications.Tests.Services
{
    public class TrainingProgramServiceTests : SetupTest
    {
        private readonly ITrainingProgramService _trainingProgramService;
        private Fixture _customFixture;
        private IConfiguration GetConfiguration()
        {
            var config = new Dictionary<string, string>
            {
                { "Jwt:Key", "TqofsXAj1s5jHz83cSh9" },
                { "Jwt:Issuer", "FATMSAuthenticator" },
                { "Jwt:Audience", "FATMSPostmanClient" },
                { "Jwt:Subject", "FATMSServiceAccessToken" }
            };

            return new ConfigurationBuilder().AddInMemoryCollection(config!).Build();
        }
        public TrainingProgramServiceTests()
        {
            _trainingProgramService = new TrainingProgramService(_unitOfWorkMock.Object,
                _currentTimeMock.Object,
                GetConfiguration(),
                _mapperConfig,
                _claimsServiceMock.Object, _syllabusServiceMock.Object);

            _customFixture = new Fixture();
            _customFixture.Customizations.Add(new TypePropertyOmitter(
                "TrainingMaterials",
                "AuditPlans",
                "Units",
                "TrainingDeliveryPrinciple",
                "ModifiedAdmin", "Level", "TrainingPrograms", "LastModifiedOn", "LastModifiedBy",
                "CourseObjectives", "TrainingDeliveryPrincipleId", "AttendeeNumber", "Classes",
                "LastModify", "ClassUsers", "ClassUnitDetails",
               "AuditPlans", "TimeMngSystem", "Role", "ModifyTrainingProgram", "CreatedTrainingProgram",
               "CreatedSyllabus", "ModifiedSyllabus", "ApprovedClass", "CreatedClass", "FeedbackTrainee", "FeedbackTrainer",
               "GradeReports", "CreatedAuditPlans", "AuditedAuditDetails", "TakenAuditDetails", "TimeMngSystemList",
               "TrainingMaterials", "ClassUnitDetails", "Level", "ResetToken", "AvatarURL", "DateOfBirth",
               "Attendances", "CreatedAdmin"
                ));
        }


        #region Test GetTrainingProgramById

        [Fact]
        public async Task GetTrainingProgramById_GetTrainingProgramByIdSuccess_ReturnCorretTrainingProgramDetail()
        {
            //arrange
            var mocks = _fixture.Build<TrainingProgramViewModels>().Create();
            mocks.Name = "Duy";
            mocks.CreatedOn = "01/01/2002";
            mocks.CreatedBy = "23";
            mocks.LastModifyBy = "23";
            mocks.LastModify = "01/01/2002";
            mocks.syllabusDetailIds = new List<int>() { 1};

            var mocksSyllabus = _fixture.Build<SyllabusViewModel>().CreateMany(1);
            foreach (var item in mocksSyllabus)
            {
                item.Id = 1;
                item.LastModifiedOn = "01/01/2002";
                item.LastModifiedBy = "23";
                item.CreatedOn = "01/01/2002";
                item.CreatedBy = "23";
            }

            var expectdResult = _mapperConfig.Map<TrainingProgram>(mocks);
            var mapSyllabus = _mapperConfig.Map<List<Syllabus>>(mocksSyllabus);
            expectdResult.Syllabuses = mapSyllabus;

            _unitOfWorkMock.Setup(x => x.TrainingProgramRepo.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(expectdResult);

            //act
            var result = await _trainingProgramService.GetByIdAsync(1);

            //assert
            _unitOfWorkMock.Verify(x => x.TrainingProgramRepo.GetByIdAsync(It.IsAny<int>()), Times.Once());
            result.Should().BeEquivalentTo(mocks);
        }

        [Fact]
        public async Task GetTrainingProgramById_InvalidTrainingProgramId_ThrowArgumentException()
        {
            int id = 0;

            var action = async () => await _trainingProgramService.GetByIdAsync(id);

            _unitOfWorkMock.Verify(x => x.TrainingProgramRepo.GetByIdAsync(1), Times.Never);

            await action.Should().ThrowAsync<ArgumentException>().WithMessage("Training Program id cannot be less than 1.");
        }
        #endregion

        #region Test GetListTrainingProgramAsync

        [Fact]
        public async void GetTrainingProgramListAsync_ReturnList()
        {
            // arrange
            var mocksList = _fixture.Build<TrainingProgramViewModels>().CreateMany(5).ToList();
            foreach (var item in mocksList)
            {
                item.CreatedOn = "01/01/2002";
                item.CreatedBy = "23";
                item.LastModifyBy = "23";
                item.LastModify = "01/01/2002";
                item.syllabusDetailIds = new List<int>() { 1, 1 };
            }

            var mocksSyllabus = _fixture.Build<SyllabusViewModel>().CreateMany(1);
            foreach (var item in mocksSyllabus)
            {
                item.Id = 1;
                item.LastModifiedOn = "01/01/2002";
                item.LastModifiedBy = "23";
                item.CreatedOn = "01/01/2002";
                item.CreatedBy = "23";
            }

            var list = _mapperConfig.Map<List<TrainingProgram>>(mocksList);
            var mapSyllabus = _mapperConfig.Map<List<Syllabus>>(mocksSyllabus);
            foreach (var item in list)
            {
                item.Syllabuses = mapSyllabus;
            }

            _unitOfWorkMock.Setup(x => x.TrainingProgramRepo.GetAllAsync()).ReturnsAsync(list);

            // act
            var result = await _trainingProgramService.GetAllAsync();

            // assert   
            _unitOfWorkMock.Verify(x => x.TrainingProgramRepo.GetAllAsync(), Times.Once);
            result.Should().NotBeEmpty();
        }

        [Fact]
        public async void GetTrainingProgramListnAsync_ReturnNotFound_WhenListEmpty()
        {
            // arrange
            var mocksList = _fixture.Build<TrainingProgramViewModels>().CreateMany(0).ToList();
            foreach (var item in mocksList)
            {
                item.CreatedOn = "01/01/2002";
                item.CreatedBy = "23";
                item.LastModifyBy = "23";
                item.LastModify = "01/01/2002";
            }

            var list = _mapperConfig.Map<List<TrainingProgram>>(mocksList);

            _unitOfWorkMock.Setup(x => x.TrainingProgramRepo.GetAllAsync()).ReturnsAsync(list);

            // act
            var result = Assert.ThrowsAsync<Exception>(async () => await _trainingProgramService.GetAllAsync());

            // assert   
            _unitOfWorkMock.Verify(x => x.TrainingProgramRepo.GetAllAsync(), Times.Once);
            Assert.Equal("No Item", result.Result.Message);
        }
        #endregion

        #region Test GetAllTrainingProgramIsActive

        [Fact]
        public async void GetAllTrainingProgramIsActiveAsync_ReturnList()
        {
            // arrange
            var mocksList = _fixture.Build<TrainingProgramViewModels>().CreateMany(3).ToList();
            foreach (var item in mocksList)
            {
                item.CreatedOn = "01/01/2002";
                item.CreatedBy = "23";
                item.LastModifyBy = "23";
                item.LastModify = "01/01/2002";
            }

            var list = _mapperConfig.Map<List<TrainingProgram>>(mocksList);
            _unitOfWorkMock.Setup(x => x.TrainingProgramRepo.GetAllAsync()).ReturnsAsync(list);

            // act
            var result = await _trainingProgramService.GetAllTrainingProgramIsActiveAsync();

            // assert   
            _unitOfWorkMock.Verify(x => x.TrainingProgramRepo.GetAllAsync(), Times.Once);
            result.Should().NotBeEmpty();          
        }

        [Fact]
        public async void GetAllTrainingProgramIsActiveAsync_ReturnNotFound_WhenListEmpty()
        {
            // arrange
            var mocksList = _fixture.Build<TrainingProgramViewModels>().CreateMany(0).ToList();
            foreach (var item in mocksList)
            {
                item.CreatedOn = "01/01/2002";
                item.CreatedBy = "23";
                item.LastModifyBy = "23";
                item.LastModify = "01/01/2002";
            }

            var list = _mapperConfig.Map<List<TrainingProgram>>(mocksList);
            foreach (var item in list)
            {
                item.isDeleted = true;
            }

            _unitOfWorkMock.Setup(x => x.TrainingProgramRepo.GetAllAsync()).ReturnsAsync(list);

            // act
            var result = Assert.ThrowsAsync<Exception>(async() => await _trainingProgramService.GetAllTrainingProgramIsActiveAsync());

            // assert   
            _unitOfWorkMock.Verify(x => x.TrainingProgramRepo.GetAllAsync(), Times.Once);
            Assert.Equal("No Item", result.Result.Message);
        }
        #endregion

        #region UpdateTrainingProgram Tests

        [Fact]
        public async Task UpdateTrainingProgram_ExistID_CorrectInput_ReturnTrue()
        {
            //arrange
            var tpModel = _customFixture.Build<UpdateTrainingProgramViewModel>().Create();

            var tpEntity = _mapperConfig.Map<TrainingProgram>(tpModel);
            tpEntity.LastModify = _currentTimeMock.Object.GetCurrentTime();
            tpEntity.LastModifyBy = _claimsServiceMock.Object.GetCurrentUserId;

            _unitOfWorkMock.Setup(x => x.SyllabusRepo.GetSyllabusByID(It.IsAny<int>()));
            _unitOfWorkMock.Setup(x => x.TrainingProgramRepo.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(tpEntity);
            _unitOfWorkMock.Setup(x => x.TrainingProgramRepo.Update(tpEntity));
            _unitOfWorkMock.Setup(x => x.SaveChangesAsync()).ReturnsAsync(1);

            //act
            var result = await _trainingProgramService.UpdateTrainingProgramsAsync(1, tpModel);

            //assert
            _unitOfWorkMock.Verify(x => x.TrainingProgramRepo.Update(tpEntity), Times.Once);
            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(), Times.Once);
            result.Should().BeTrue();
        }

        [Fact]
        public async Task UpdateTrainingProgram_NotExistID_ReturnNull2()
        {
            var tpModel = _customFixture.Build<UpdateTrainingProgramViewModel>().Create();

            //tpModel.Id = 1;
            tpModel.Name = "Test";
            //tpModel.Duration = 2;
            tpModel.IsActive = true;

            TrainingProgram tpEntity = new TrainingProgram();
            tpEntity.LastModify = _currentTimeMock.Object.GetCurrentTime();
            tpEntity.LastModifyBy = _claimsServiceMock.Object.GetCurrentUserId;


            tpEntity = _mapperConfig.Map<TrainingProgram>(tpModel);

            _unitOfWorkMock.Setup(x => x.TrainingProgramRepo.GetByIdAsync(2)).ReturnsAsync(tpEntity);

            var result = async () => await _trainingProgramService.UpdateTrainingProgramsAsync(1, tpModel);

            await result.Should().ThrowAsync<Exception>().WithMessage("Training program is null");
        }

        #endregion

        #region Test Create Training Program
        [Fact]
        public async Task CreateTrainingProgram_ShouldReturnCurrentData_WhenSavedSuccess()
        {
            //arrange
            var mock = _fixture.Build<CreateTrainingProgramViewModels>().With(x => x.SyllabusIds, new List<int>() { 1, 2, 3 }).Create();
            //var mockTrainingProgram = _fixture.Build<CreateTrainingProgramViewModels>().With(x => x.SyllabusIds, new List<int>() { 1, 2, 3}).CreateMany(2).ToList();
            //var list = _mapperConfig.Map<List<TrainingProgram>>(mockTrainingProgram);           
            var mockSyllabus = _customFixture.Build<Syllabus>().Create();
            var mockSyllabusDetail = _customFixture.Build<SyllabusDetailModel>().Create();
            

            _unitOfWorkMock.Setup(x => x.TrainingProgramRepo.GetAllAsync()).ReturnsAsync(It.IsAny<List<TrainingProgram>>());         
            _unitOfWorkMock.Setup(x => x.SyllabusRepo.GetSyllabusDetailByIdAsync(It.IsAny<int>())).ReturnsAsync(mockSyllabus);
            _unitOfWorkMock.Setup(X => X.SyllabusRepo.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(mockSyllabus);
            _unitOfWorkMock.Setup(x => x.TrainingProgramRepo.AddAsync(It.IsAny<TrainingProgram>())).Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(x => x.SaveChangesAsync()).ReturnsAsync(1);

            //act
            var result = await _trainingProgramService.CreateTrainingProgramAsync(mock);

            //assert
            _unitOfWorkMock.Verify(x => x.TrainingProgramRepo.AddAsync(It.IsAny<TrainingProgram>()), Times.Once());
            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(), Times.Once());
        }
        [Fact]
        public async Task CreateTrainingProgram_ShouldReturnFalse_WhenSavedFalse()
        {
           // arrange
            var mock = _fixture.Build<CreateTrainingProgramViewModels>().Create();

            var mockTrainigPriogram = _fixture.Build<CreateTrainingProgramViewModels>().CreateMany(2).ToList();
            var list = _mapperConfig.Map<List<TrainingProgram>>(mockTrainigPriogram);

            var mockSyllabus = _customFixture.Build<Syllabus>().Create();

            _unitOfWorkMock.Setup(x => x.TrainingProgramRepo.GetAllAsync()).ReturnsAsync(list);
            _unitOfWorkMock.Setup(x => x.SyllabusRepo.GetSyllabusDetailByIdAsync(It.IsAny<int>())).ReturnsAsync(mockSyllabus);

            _unitOfWorkMock.Setup(x => x.TrainingProgramRepo.AddAsync(It.IsAny<TrainingProgram>())).Returns(Task.CompletedTask);

            _unitOfWorkMock.Setup(x => x.SaveChangesAsync()).ReturnsAsync(0);

            //act
            var result = Assert.ThrowsAsync<Exception>(async () => await _trainingProgramService.CreateTrainingProgramAsync(mock));

            //assert
            _unitOfWorkMock.Verify(x => x.TrainingProgramRepo.AddAsync(It.IsAny<TrainingProgram>()), Times.Once);
            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(), Times.Once);
            Assert.Equal("Add Syllabus into TrainingProgam faild.", result.Result.Message);

        }

        [Fact]
        public async Task CreateTrainingProgram_ShouldReturnError_WhenDuplicationName()
        {

            //arr
            var mocks = _fixture.Build<CreateTrainingProgramViewModels>().With(x => x.Name, "DDD").Create();

            var mockTrainigPriogram = _fixture.Build<CreateTrainingProgramViewModels>().With(x => x.Name, "DDD").CreateMany(3).ToList();
            var list = _mapperConfig.Map<List<TrainingProgram>>(mockTrainigPriogram);
            _unitOfWorkMock.Setup(x => x.TrainingProgramRepo.GetAllAsync()).ReturnsAsync(list);

            //act
            var result = async () => await _trainingProgramService.CreateTrainingProgramAsync(mocks);

            //assert
            _unitOfWorkMock.Verify(x => x.TrainingProgramRepo.AddAsync(It.IsAny<TrainingProgram>()), Times.Never());
            await result.Should().ThrowAsync<ArgumentNullException>();
        }

        #endregion

        #region Test Delete TrainingProgram

        [Fact]
        public async Task DeleteTrainingProgram_CorrectInput_ReturnCorrect()
        {
            //arrange
            var mocks = _fixture.Build<TrainingProgram>().Without(x => x.CreatedAdmin)
                .Without(x => x.ModifiedAdmin)
                .Without(x => x.Classes)
                .Without(x => x.Syllabuses).Create();
            mocks.Id = 1;
            var result = true;
            try
            {
                _unitOfWorkMock.Setup(x => x.TrainingProgramRepo.GetByIdAsync(mocks.Id)).ReturnsAsync(mocks);

                //act
                result = await _trainingProgramService.DeleteTrainingProgramsAsync(mocks.Id);
                _unitOfWorkMock.Verify(x => x.SaveChangesAsync(), Times.Once);
            }
            catch (Exception ex)
            {
                result = false;
            }

            //assert
            result.Should().BeTrue();

        }
        [Fact]
        public async Task DeleteTrainingProgram_IncorrectId_ReturnException()
        {
            var mock = _fixture.Build<TrainingProgramViewModels>().With(x => x.Id, 1).Create();

            _unitOfWorkMock.Setup(x => x.TrainingProgramRepo.GetByIdAsync(1));
            var mocks = _trainingProgramService.GetByIdAsync(1);

            var result = async () => await _trainingProgramService.DeleteTrainingProgramsAsync(mocks.Id);


            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(), Times.Never);
            await result.Should().ThrowAsync<Exception>().WithMessage("Don't found this Training Program");
        }
        #endregion

        #region Test Get TrainingProgram IsActive By Name
        [Fact]
        public async void GetTrainingProgramIsActiveByNameAsync_ReturnList()
        {
            // arrange
            var mocksList = _customFixture.Build<TrainingProgramViewModels>().CreateMany(3).ToList();
            foreach (var item in mocksList)
            {
                item.Name = "C#";
                item.CreatedOn = "01/01/2002";
                item.CreatedBy = "23";
                item.LastModifyBy = "23";
                item.LastModify = "01/01/2002";
            }

            var list = _mapperConfig.Map<List<TrainingProgram>>(mocksList);
            _unitOfWorkMock.Setup(x => x.TrainingProgramRepo.GetAllAsync()).ReturnsAsync(list);

            // act
            var result = await _trainingProgramService.GetTrainingProgramIsActiveByNameAsync("C#");

            // assert   
            _unitOfWorkMock.Verify(x => x.TrainingProgramRepo.GetAllAsync(), Times.Once);
            result.Should().NotBeEmpty();
        }
        #endregion

        #region Test Get TrainingProgram By Name
        [Fact]
        public async void GetTrainingProgramByNameAsync_ReturnList()
        {
            // arrange
            var mocksList = _customFixture.Build<TrainingProgramViewModels>().CreateMany(3).ToList();
            foreach (var item in mocksList)
            {
                item.Name = "C#";
                item.CreatedOn = "01/01/2002";
                item.CreatedBy = "23";
                item.LastModifyBy = "23";
                item.LastModify = "01/01/2002";
            }

            var list = _mapperConfig.Map<List<TrainingProgram>>(mocksList);
            _unitOfWorkMock.Setup(x => x.TrainingProgramRepo.GetAllAsync()).ReturnsAsync(list);

            // act
            var result = await _trainingProgramService.GetTrainingProgramByNameAsync("C#");

            // assert   
            _unitOfWorkMock.Verify(x => x.TrainingProgramRepo.GetAllAsync(), Times.Once);
            result.Should().NotBeEmpty();
        }
        #endregion

        #region Test Get TrainingProgram By Date Range CreateOn
        [Fact]
        public async void GetTrainingProgramByDateRangeCreateOn_ReturnListTrainigProgram()
        { 
            //arrange
            var mocksList = _customFixture.Build<TrainingProgramViewModels>().CreateMany(3).ToList();
            foreach (var item in mocksList)
            {
                item.CreatedOn = "01/01/2002";
                item.CreatedBy = "23";
                item.LastModifyBy = "23";
                item.LastModify = "01/01/2002";
            }
            var list = _mapperConfig.Map<List<TrainingProgram>>(mocksList);
            _unitOfWorkMock.Setup(x => x.TrainingProgramRepo.GetAllAsync()).ReturnsAsync(list);

            //act
            var result = await _trainingProgramService.GetTrainingProgramByDateRangeCreateOnAsync(new DateTime[] {new DateTime(2002, 01, 01), new DateTime(2010, 10, 2)});

            //assert
            result.Should().NotBeNull();
        }

        [Fact]
        public async void GetTrainingProgramByDateRangeCreateOn_ReturnListTrainigProgram_WhenDateTimeIsEmpty()
        {
            //arrange
            var mocksList = _customFixture.Build<TrainingProgramViewModels>().CreateMany(3).ToList();
            foreach (var item in mocksList)
            {
                item.CreatedOn = "01/01/2002";
                item.CreatedBy = "23";
                item.LastModifyBy = "23";
                item.LastModify = "01/01/2002";
            }
            var list = _mapperConfig.Map<List<TrainingProgram>>(mocksList);
            _unitOfWorkMock.Setup(x => x.TrainingProgramRepo.GetAllAsync()).ReturnsAsync(list);

            //act
            var result = await _trainingProgramService.GetTrainingProgramByDateRangeCreateOnAsync(new DateTime[] {});

            //assert
            result.Should().NotBeNull();
        }
        #endregion

        #region Test Get TrainingProgram By Date Range ListModify
        [Fact]
        public async void GetTrainingProgramByDateRangeListModify_ReturnListTrainigProgram()
        {
            //arrange
            var mocksList = _customFixture.Build<TrainingProgramViewModels>().CreateMany(3).ToList();
            foreach (var item in mocksList)
            {
                item.CreatedOn = "01/01/2002";
                item.CreatedBy = "23";
                item.LastModifyBy = "23";
                item.LastModify = "01/01/2002";
            }
            var list = _mapperConfig.Map<List<TrainingProgram>>(mocksList);
            _unitOfWorkMock.Setup(x => x.TrainingProgramRepo.GetAllAsync()).ReturnsAsync(list);

            //act
            var result = await _trainingProgramService.GetTrainingProgramByDateRangeLastModifyAsync(new DateTime[] { new DateTime(2002, 01, 01), new DateTime(2010, 10, 2) });

            //assert
            result.Should().NotBeNull();        }

        [Fact]
        public async void GetTrainingProgramByDateRangeListModify_ReturnListTrainigProgram_WhenDateTimeIsEmpty()
        {
            //arrange
            var mocksList = _customFixture.Build<TrainingProgramViewModels>().CreateMany(3).ToList();
            foreach (var item in mocksList)
            {
                item.CreatedOn = "01/01/2002";
                item.CreatedBy = "23";
                item.LastModifyBy = "23";
                item.LastModify = "01/01/2002";
            }
            var list = _mapperConfig.Map<List<TrainingProgram>>(mocksList);
            _unitOfWorkMock.Setup(x => x.TrainingProgramRepo.GetAllAsync()).ReturnsAsync(list);

            //act
            var result = await _trainingProgramService.GetTrainingProgramByDateRangeLastModifyAsync(new DateTime[] { });

            //assert
            result.Should().NotBeNull();
        }
        #endregion

        #region Clone TrainingProgram
        [Fact]
        public async void CloneTrainingProgram_ReturnException_WhenClassIdLessThan1()
        {
            //Arrange
            int id = 0;
            //Act
            var result = Assert.ThrowsAsync<Exception>(async () => await _trainingProgramService.CloneTrainingProgramAsync(id));

            //Assert
            Assert.Equal("TrainingProgram id must be an integer with value equal or greater than 1", result.Result.Message);
        }

        [Fact]
        public async void CloneTrainingProgram_ReturnException_WhenClassNotFound()
        {
            //Arrange
            TrainingProgram newTrainingProgram = null;
            _unitOfWorkMock.Setup(x => x.TrainingProgramRepo.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(newTrainingProgram);

            //Act
            var result = Assert.ThrowsAsync<Exception>(async () => await _trainingProgramService.CloneTrainingProgramAsync(1));

            //Assert
            Assert.Equal("TrainingProgram not found", result.Result.Message);
        }
        [Fact]
        public async void CloneTrainingProgram_Succeed()
        {
            //Arrange
            var mockTrainingProgram = _customFixture.Build<TrainingProgram>().Create();
            _unitOfWorkMock.Setup(x => x.TrainingProgramRepo.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(mockTrainingProgram);
            _unitOfWorkMock.Setup(x => x.TrainingProgramRepo.CloneAsync(mockTrainingProgram)).ReturnsAsync(mockTrainingProgram);
            _unitOfWorkMock.Setup(x => x.TrainingProgramRepo.AddAsync(It.IsAny<TrainingProgram>())).Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(x => x.SaveChangesAsync()).ReturnsAsync(1);
            _trainingProgramServiceMock.Setup(x => x.CloneTrainingProgramAsync(It.IsAny<int>()));
            //Act
            var result = _trainingProgramService.CloneTrainingProgramAsync(1);
            //Assert
            _unitOfWorkMock.Verify(x => x.TrainingProgramRepo.AddAsync(It.IsAny<TrainingProgram>()), Times.Once);
            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(), Times.Once);
        }
        #endregion

        #region Filter
        [Fact]
        public async void Filter_ReturnList()
        {
            //Arrange
            var mockFilter = _customFixture.Build<TrainingProgramFilterModel>().Create();
            var mock = _customFixture.Build<TrainingProgram>().CreateMany(3).ToList();
            _unitOfWorkMock.Setup(x => x.TrainingProgramRepo.GetAllAsync()).ReturnsAsync(mock);
            //Act
            var result = await _trainingProgramService.Filter(mockFilter);
            //Assert
            result.Should().NotBeNull().And.BeEmpty();
        }
        #endregion
    }
}
