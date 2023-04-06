using Application.Interfaces;
using Application.Repositories;
using Application.Services;
using Application.ViewModels.AddLectureViewModels;
using Application.ViewModels.LectureViewModels;
using Application.ViewModels.SyllabusViewModels;
using Application.ViewModels.UnitViewModels;
using AutoFixture;
using AutoMapper;
using Domain.Enums.LectureEnums;
using Domain.Models;
using Domain.Models.Syllabuses;
using Domain.Models.Users;
using Domain.Tests;
using FluentAssertions;
using Infracstructures.Repositories;
using Moq;
using System;
using System.Diagnostics;

namespace Applications.Tests.Services
{
    public class SyllabusServiceTests : SetupTest
    {
        private readonly ISyllabusService _syllabusService;
        private readonly IUnitRepository _unitRepository;
        private Fixture _customFixture;

        public SyllabusServiceTests()
        {

            _syllabusService = new SyllabusService(_unitOfWorkMock.Object, _mapperConfig, _claimsServiceMock.Object,
            _currentTimeMock.Object, _configuration, _lectureValidator.Object);
            _unitRepository = new UnitRepository(_dbContext, _currentTimeMock.Object, _claimsServiceMock.Object);
            _customFixture = new Fixture();
            _customFixture.Customizations.Add(new TypePropertyOmitter(
                "CreatedAdmin",
                "ModifiedAdmin",
                "TrainingPrograms",
                "AuditPlans",
                "Classes",
                "Syllabuses",
                "ClassUnitDetails",
                "Unit",
                "OutputStandard",
                "GradeReports",
                "TrainingMaterials",
                "LessonType",
                "DeliveryType",
                "Name",
                "UnitId",
                "OutputStandardId",
                "isDeleted",
                "Id",
                "LastModifiedBy",
                "LastModifiedOn",
                "CreatedOn",
                "CreatedBy",
                "AttendeeNumber",
                "CourseObjectives",
                "TechnicalRequirements",
                "TrainingDeliveryPrinciple",
                "QuizCriteria",
                "AssignmentCriteria",
                "FinalCriteria",
                "FinalTheoryCriteria",
                "FinalPracticalCriteria",
                "PassingGPA",
                "isActive",
                "Version",
                "Code",
                "TimeDuration",
                "QuizBanks"));
        }

        #region ServiceTests GetSyllabusDetail

        [Fact]
        public async void GetSyllabusDetailAsync_SyllabusIdExist_ReturnCorrectSyllabusDetailWithLectures()
        {
            // arrange
            var mockSyllabus = _customFixture.Build<Syllabus>().Create();
            var expectedResult = _mapperConfig.Map<SyllabusDetailModel>(mockSyllabus);
            for (int i = 1; i <= expectedResult.Duration.Days; i++)
            {
                expectedResult.Days.Add(new Content
                {
                    Day = i,
                    Units = new List<UnitDetailModel>()
                });
            }
            foreach (var unit in expectedResult.Units)
            {
                foreach (var item in expectedResult.Days)
                {
                    if (unit.Session == item.Day)
                    {
                        item.Units.Add(unit);
                    }
                }
            }
            _unitOfWorkMock.Setup(x => x.SyllabusRepo.GetSyllabusDetailByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(mockSyllabus);

            // act
            var result = await _syllabusService.GetSyllabusDetailByIdAsync(1);
            result.TimeAllocation = expectedResult.TimeAllocation;
            // assert
            _unitOfWorkMock.Verify(x => x.SyllabusRepo.GetSyllabusDetailByIdAsync(1), Times.Once());
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public async void GetSyllabusDetailAsync_SyllabusIdExist_ReturnCorrectSyllabusDetailWithoutLectures()
        {
            // arrange
            _customFixture.Customize<Unit>(c => c.With(x => x.Lectures, () => _customFixture.CreateMany<Lecture>(0).ToList()));
            var mockSyllabus = _customFixture.Build<Syllabus>().Create();
            var expectedResult = _mapperConfig.Map<SyllabusDetailModel>(mockSyllabus);
            for (int i = 1; i <= expectedResult.Duration.Days; i++)
            {
                expectedResult.Days.Add(new Content
                {
                    Day = i,
                    Units = new List<UnitDetailModel>()
                });
            }
            foreach (var unit in expectedResult.Units)
            {
                foreach (var item in expectedResult.Days)
                {
                    if (unit.Session == item.Day)
                    {
                        item.Units.Add(unit);
                    }
                }
            }
            _unitOfWorkMock.Setup(x => x.SyllabusRepo.GetSyllabusDetailByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(mockSyllabus);

            // act
            var result = await _syllabusService.GetSyllabusDetailByIdAsync(1);
            result.TimeAllocation = expectedResult.TimeAllocation;
            // assert
            _unitOfWorkMock.Verify(x => x.SyllabusRepo.GetSyllabusDetailByIdAsync(1), Times.Once());
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public void GetSyllabusDetailAsync_SyllabusIdNotExist_ThrowNullReferenceException()
        {
            // arrange
            Syllabus? mockNullSyllabus = null;
            _unitOfWorkMock.Setup(x => x.SyllabusRepo.GetSyllabusDetailByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(mockNullSyllabus);

            // act and assert
            var result = Assert.ThrowsAsync<NullReferenceException>(async () => await _syllabusService.GetSyllabusDetailByIdAsync(-1));
            _unitOfWorkMock.Verify(x => x.SyllabusRepo.GetSyllabusDetailByIdAsync(-1), Times.Once());
            _unitOfWorkMock.Verify(x => x.UserRepo.GetByIdAsync(It.IsAny<int>()), Times.Never());
            Assert.Equal("Syllabus not found", result.Result.Message);
        }
        #endregion

        #region ServiceTests CloneSyllabus

        [Fact]
        public async void CloneSyllabusAsync_SyllabusIdExist_CloneSuccess()
        {
            // arrange
            var cloneSyllabusModel = _fixture.Build<CloneSyllabusViewModel>().Create();
            var cloneSyllabus = _mapperConfig.Map<Syllabus>(cloneSyllabusModel);
            _unitOfWorkMock.Setup(x => x.SyllabusRepo.GetSyllabusDetailByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(cloneSyllabus);
            _claimsServiceMock.Setup(x => x.GetCurrentUserId).Returns(1);

            // act
            await _syllabusService.CloneSyllabusAsync(1);
            // assert
            _unitOfWorkMock.Verify(x => x.SyllabusRepo.GetSyllabusDetailByIdAsync(1), Times.Once());
            _unitOfWorkMock.Verify(x => x.SyllabusRepo.AddAsync(It.IsAny<Syllabus?>()), Times.Once());
            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(), Times.Once());
        }

        [Fact]
        public void CloneSyllabusAsync_SyllabusIdNotExist_ThrowNullReferenceException()
        {
            // arrange
            Syllabus? cloneSyllabus = null;
            _unitOfWorkMock.Setup(x => x.SyllabusRepo.GetSyllabusDetailByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(cloneSyllabus);

            // act and assert
            var result = Assert.ThrowsAsync<NullReferenceException>(async () => await _syllabusService.CloneSyllabusAsync(-1));
            _unitOfWorkMock.Verify(x => x.SyllabusRepo.GetSyllabusDetailByIdAsync(-1), Times.Once());
            _unitOfWorkMock.Verify(x => x.SyllabusRepo.AddAsync(It.IsAny<Syllabus?>()), Times.Never());
            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(), Times.Never());
            Assert.Equal("Syllabus not found", result.Result.Message);
        }
        #endregion

        [Fact]
        public async void DeleteSyallabusAsync_ShouldReturnCorrentData_WhenSuccessSaved()
        {
            // Arrange
            var SyllabusDetailModel = _customFixture.Build<Syllabus>()
                .Create();
            var mockItem = _mapperConfig.Map<SyllabusViewModel>(SyllabusDetailModel);
            _unitOfWorkMock.Setup(x => x.SyllabusRepo.GetByIdAsync(1)).ReturnsAsync(SyllabusDetailModel);
            _unitOfWorkMock.Setup(x => x.SyllabusRepo.Update(SyllabusDetailModel));
            _unitOfWorkMock.Setup(x => x.SaveChangesAsync()).ReturnsAsync(1);
            //Act 
            var result = await _syllabusService.DeleteSyllabusAsync(1);
            //Assert
            _unitOfWorkMock.Verify(x => x.SyllabusRepo.GetByIdAsync(1), Times.Once());
            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(), Times.Once());
            result.Should().NotBeNull();
            result.Should().BeOfType(typeof(SyllabusViewModel));
        }

        [Fact]
        public async void DeleteSyallabusAsync_InvalidSyllabusId_ThrowArgumentException()
        {
            // arrange
            int id = 0;
            // act
            var action = async () => await _syllabusService.DeleteSyllabusAsync(id);
            // assert   
            _unitOfWorkMock.Verify(x => x.SyllabusRepo.GetByIdAsync(1), Times.Never);
            await action.Should().ThrowAsync<ArgumentException>().WithMessage("Syllabus id cannot be less than 1.");
        }

        [Fact]
        public async void DeleteSyallabusAsync_SyllabusIdNotExist_ReturnNull()
        {
            // Arrange
            var SyllabusDetailModel = _customFixture.Build<Syllabus>()
                .Create();
            var mockItem = _mapperConfig.Map<Syllabus>(SyllabusDetailModel);
            _unitOfWorkMock.Setup(x => x.SyllabusRepo.GetByIdAsync(2)).ReturnsAsync(mockItem);
            _unitOfWorkMock.Setup(x => x.SyllabusRepo.Update(mockItem));
            _unitOfWorkMock.Setup(x => x.SaveChangesAsync()).ReturnsAsync(2);
            //Act 
            var result = async () => await _syllabusService.DeleteSyllabusAsync(1);
            //Assert
            _unitOfWorkMock.Verify(x => x.SyllabusRepo.GetByIdAsync(1), Times.Never());
            await result.Should().ThrowAsync<ArgumentException>().WithMessage("Syllabus id not found!!");
        }


        [Fact]
        public async Task AddLectureAsync_ShouldReturnCorrentData_WhenSuccessSaved()
        {
            //Arrange
            var mocks = _fixture.Build<AddLectureViewModel>()
                .Create();


            _unitOfWorkMock.Setup(x => x.LectureRepo.AddAsync(It.IsAny<Lecture>()))
                .Returns(Task.CompletedTask);

            _unitOfWorkMock.Setup(x => x.SaveChangesAsync()).ReturnsAsync(1);
            //Act
            var result = await _syllabusService.AddLectureAsync(mocks);
            var view = _mapperConfig.Map<LectureViewModel>(mocks);
            //Assert
            _unitOfWorkMock.Verify(x => x.LectureRepo.AddAsync(It.IsAny<Lecture>()), Times.Once());
            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(), Times.Once());
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(view);
            result.Should().BeOfType(typeof(LectureViewModel));
        }

        [Fact]
        public async Task AddLectureAsync_ShouldReturnNull_WhenFailedSave()
        {
            //Arrange
            var mocks = _fixture.Build<AddLectureViewModel>()
              .Create();


            _unitOfWorkMock.Setup(
                x => x.LectureRepo.AddAsync(It.IsAny<Lecture>())).Returns(Task.CompletedTask);

            _unitOfWorkMock.Setup(x => x.SaveChangesAsync()).ReturnsAsync(0);

            //Act
            var result = await _syllabusService.AddLectureAsync(mocks);

            //Assert
            _unitOfWorkMock.Verify(
                x => x.LectureRepo.AddAsync(It.IsAny<Lecture>()), Times.Once());

            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(), Times.Once());

            result.Should().BeNull();
        }

        [Fact]
        public async Task AddLectureToUnitAsync_ShouldReturnException_WhenFailedInputID()
        {
            // Arrange
            int id = 1;
            int unit_id = 0;
            // act
            var action = async () => await _syllabusService.AddLectureToUnitAsync(id, unit_id);
            // assert   
            _unitOfWorkMock.Verify(x => x.SyllabusRepo.GetByIdAsync(1), Times.Never);
            await action.Should().ThrowAsync<Exception>().WithMessage("Unit id or Lecture id cannot be less than 1.");
        }

        [Fact]
        public async Task AddLectureToUnitAsync_ShouldReturnException_WhenLectureIDNotFound()
        {
            // Arrange
            var mocks = _fixture.Build<AddLectureViewModel>().Create();

            var expectdResult = _mapperConfig.Map<Lecture>(mocks);
            var mocks1 = _fixture.Build<Unit>().Without(e => e.Syllabuses).Without(e => e.Lectures).Without(e => e.ClassUnitDetails).Without(e => e.QuizBanks).Create();
            _unitOfWorkMock.Setup(x => x.LectureRepo.GetByIdAsync(2)).ReturnsAsync(expectdResult);
            _unitOfWorkMock.Setup(x => x.UnitRepo.GetByIdAsync(1)).ReturnsAsync(mocks1);
            // Act
            var action = async () => await _syllabusService.AddLectureToUnitAsync(1, 1);
            // Assert   
            _unitOfWorkMock.Verify(x => x.LectureRepo.GetByIdAsync(50), Times.Never());
            await action.Should().ThrowAsync<Exception>().WithMessage("Lecture id not found");
        }

        [Fact]
        public async Task AddLectureToUnitAsync_ShouldReturnException_WhenUnitIDNotFound()
        {
            // Arrange
            var mocks = _fixture.Build<AddLectureViewModel>().Create();
            var expectdResult = _mapperConfig.Map<Lecture>(mocks);
            var mocks1 = _fixture.Build<Unit>().Without(e => e.Syllabuses).Without(e => e.Lectures).Without(e => e.ClassUnitDetails).Without(e => e.QuizBanks).Create();
            _unitOfWorkMock.Setup(x => x.LectureRepo.GetByIdAsync(2)).ReturnsAsync(expectdResult);
            _unitOfWorkMock.Setup(x => x.UnitRepo.GetByIdAsync(2)).ReturnsAsync(mocks1);
            // Act
            var action = async () => await _syllabusService.AddLectureToUnitAsync(1, 1);
            // Assert   
            _unitOfWorkMock.Verify(x => x.UnitRepo.GetByIdAsync(40), Times.Never());
            await action.Should().ThrowAsync<Exception>().WithMessage("Unit id not found");
        }

        [Fact]
        public async Task AddLectureToUnit_AddLectureToUnitSuccess_ReturnCorretLectureDetail()
        {
            // Arrange
            var mocks = _fixture.Build<AddLectureViewModel>().Create();
            var expectdResult = _mapperConfig.Map<Lecture>(mocks);
            var mocks1 = _fixture.Build<Unit>().Without(e => e.Syllabuses).Without(e => e.Lectures).Without(e => e.ClassUnitDetails).Without(e => e.QuizBanks).Create();
            _unitOfWorkMock.Setup(x => x.LectureRepo.GetByIdAsync(1)).ReturnsAsync(expectdResult);
            _unitOfWorkMock.Setup(x => x.UnitRepo.GetByIdAsync(1)).ReturnsAsync(mocks1);
            _unitOfWorkMock.Setup(x => x.SaveChangesAsync()).ReturnsAsync(1);
            // Act
            var view = _mapperConfig.Map<LectureViewModel>(mocks);
            view.UnitId = 1;
            var action = await _syllabusService.AddLectureToUnitAsync(1, 1);
            // Assert
            action.Should().NotBeNull();
            action.Should().BeOfType(typeof(LectureViewModel));
            action.Should().BeEquivalentTo(view);
        }

        [Fact]
        public async Task AddLectureToUnit_AddLectureToUnitFail_ReturnException()

        {// Arrange
            var mocks = _fixture.Build<AddLectureViewModel>().Create();
            var expectdResult = _mapperConfig.Map<Lecture>(mocks);
            var mocks1 = _fixture.Build<Unit>().Without(e => e.Syllabuses).Without(e => e.Lectures).Without(e => e.ClassUnitDetails).Without(e => e.QuizBanks).Create();
            _unitOfWorkMock.Setup(x => x.LectureRepo.GetByIdAsync(1)).ReturnsAsync(expectdResult);
            _unitOfWorkMock.Setup(x => x.UnitRepo.GetByIdAsync(1)).ReturnsAsync(mocks1);
            _unitOfWorkMock.Setup(x => x.SaveChangesAsync()).ReturnsAsync(0);
            // Act
            var action = async () => await _syllabusService.AddLectureToUnitAsync(1, 1);
            // Assert
            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(), Times.Never());
            await action.Should().ThrowAsync<Exception>().WithMessage("Add Error");
        }



        #region ServiceTests UpdateSyllabus
        // #region Update
        //
        // [Fact]
        // public async Task UpdateSyllabusAsync_ValidInput_ReturnUpdatedSyllabus()
        // {
        //     // arrange
        //     var currentUserId = _fixture.Create<int>();
        //     _claimsServiceMock.Setup(x => x.GetCurrentUserId).Returns(currentUserId);
        //
        //     var mockItem = _fixture.Build<Syllabus>()
        //         .With(x => x.TrainingDeliveryPrincipleId, () => _fixture.Create<int>())
        //         .With(x => x.LastModifiedBy, () => currentUserId)
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
        //     _unitOfWorkMock.Setup(x => x.SyllabusRepo.GetByIdAsync(mockItem.Id)).ReturnsAsync(mockItem);
        //     _unitOfWorkMock.Setup(x => x.SyllabusRepo.Update(mockItem));
        //     _unitOfWorkMock.Setup(x => x.SaveChangesAsync()).ReturnsAsync(1);
        //
        //     // act
        //     var result =
        //         await _syllabusService.UpdateSyllabusAsync(_mapperConfig.Map<UpdateSyllabusViewModel>(mockItem));
        //
        //     // assert
        //     result.Should().NotBeNull();
        //     result.Should().BeEquivalentTo(expectedResult);
        // }
        //
        // [Fact]
        // public async Task UpdateSyllabusAsync_InvalidInput_ReturnFailure()
        // {
        //     // arrange
        //     var currentUserId = _fixture.Create<int>();
        //     _claimsServiceMock.Setup(x => x.GetCurrentUserId).Returns(currentUserId);
        //
        //     var mockItem = _fixture.Build<Syllabus>()
        //         .With(x => x.TrainingDeliveryPrincipleId, () => _fixture.Create<int>())
        //         .With(x => x.LastModifiedBy, () => currentUserId)
        //         .With(x => x.LastModifiedOn, () => _currentTimeMock.Object.GetCurrentTime())
        //         .Without(e => e.CreatedAdmin)
        //         .Without(e => e.ModifiedAdmin)
        //         .Without(e => e.TrainingDeliveryPrinciple)
        //         .Without(e => e.Units)
        //         .Without(e => e.TrainingPrograms)
        //         .Without(e => e.AuditPlans)
        //         .Create();
        //
        //     _unitOfWorkMock.Setup(x => x.SyllabusRepo.GetByIdAsync(mockItem.Id)).ReturnsAsync(mockItem);
        //     _unitOfWorkMock.Setup(x => x.SyllabusRepo.Update(mockItem));
        //     _unitOfWorkMock.Setup(x => x.SaveChangesAsync()).ReturnsAsync(0);
        //
        //     // act
        //     var result =
        //         await _syllabusService.UpdateSyllabusAsync(_mapperConfig.Map<UpdateSyllabusViewModel>(mockItem));
        //
        //     // assert
        //     result.Should().BeNull();
        // }
        //
        // [Fact]
        // public async Task UpdateSyllabusAsync_AuthorizationFailed_ReturnFailure()
        // {
        //     // arrange
        //     var currentUserId = _fixture.Create<int>();
        //     _claimsServiceMock.Setup(x => x.GetCurrentUserId).Returns(currentUserId);
        //
        //     var mockItem = _fixture.Build<Syllabus>()
        //         .With(x => x.TrainingDeliveryPrincipleId, () => _fixture.Create<int>())
        //         .With(x => x.LastModifiedBy, () => currentUserId)
        //         .With(x => x.LastModifiedOn, () => _currentTimeMock.Object.GetCurrentTime())
        //         .Without(e => e.CreatedAdmin)
        //         .Without(e => e.ModifiedAdmin)
        //         .Without(e => e.TrainingDeliveryPrinciple)
        //         .Without(e => e.Units)
        //         .Without(e => e.TrainingPrograms)
        //         .Without(e => e.AuditPlans)
        //         .Create();
        //
        //     _unitOfWorkMock.Setup(x => x.SyllabusRepo.GetByIdAsync(mockItem.Id)).ReturnsAsync(mockItem);
        //     _unitOfWorkMock.Setup(x => x.SyllabusRepo.Update(mockItem));
        //     _unitOfWorkMock.Setup(x => x.SaveChangesAsync()).ReturnsAsync(0);
        //
        //     // act
        //     var result =
        //         await _syllabusService.UpdateSyllabusAsync(_mapperConfig.Map<UpdateSyllabusViewModel>(mockItem));
        //
        //     // assert
        //     result.Should().BeNull();
        // }
        //
        //
        // [Fact]
        // public async Task UpdateSyllabusAsync_MissingSyllabus_ReturnsNull()
        // {
        //     // Arrange
        //     var missingSyllabusId = 999;
        //
        //     var currentUserId = _fixture.Create<int>();
        //     _claimsServiceMock.Setup(x => x.GetCurrentUserId).Returns(currentUserId);
        //
        //     var mockItem = _fixture.Build<Syllabus>()
        //         .With(x => x.TrainingDeliveryPrincipleId, () => _fixture.Create<int>())
        //         .With(x => x.LastModifiedBy, () => currentUserId)
        //         .With(x => x.LastModifiedOn, () => _currentTimeMock.Object.GetCurrentTime())
        //         .With(x => x.Id, () => missingSyllabusId)
        //         .Without(e => e.CreatedAdmin)
        //         .Without(e => e.ModifiedAdmin)
        //         .Without(e => e.TrainingDeliveryPrinciple)
        //         .Without(e => e.Units)
        //         .Without(e => e.TrainingPrograms)
        //         .Without(e => e.AuditPlans)
        //         .Create();
        //
        //     _unitOfWorkMock.Setup(x => x.SyllabusRepo.GetByIdAsync(missingSyllabusId)).ReturnsAsync(mockItem);
        //
        //     // Act
        //     var result =
        //         await _syllabusService.UpdateSyllabusAsync(new UpdateSyllabusViewModel { Id = missingSyllabusId });
        //
        //     // Assert
        //     result.Should().BeNull();
        // }
        //
        // [Fact]
        // public async Task UpdateSyllabusAsync_UnauthorizedAccess_ReturnFailure()
        // {
        //     // Arrange
        //     var currentUserId = _fixture.Create<int>();
        //     var unauthorizedUserId = _fixture.Create<int>();
        //
        //     var mockItem = _fixture.Build<Syllabus>()
        //         .With(x => x.TrainingDeliveryPrincipleId, () => _fixture.Create<int>())
        //         .With(x => x.LastModifiedBy, () => currentUserId)
        //         .With(x => x.LastModifiedOn, () => _currentTimeMock.Object.GetCurrentTime())
        //         .Without(e => e.CreatedAdmin)
        //         .Without(e => e.ModifiedAdmin)
        //         .Without(e => e.TrainingDeliveryPrinciple)
        //         .Without(e => e.Units)
        //         .Without(e => e.TrainingPrograms)
        //         .Without(e => e.AuditPlans)
        //         .Create();
        //
        //     _unitOfWorkMock.Setup(x => x.SyllabusRepo.GetByIdAsync(mockItem.Id)).ReturnsAsync(mockItem);
        //     _claimsServiceMock.Setup(x => x.GetCurrentUserId).Returns(unauthorizedUserId);
        //
        //     // Act
        //     var result =
        //         await _syllabusService.UpdateSyllabusAsync(_mapperConfig.Map<UpdateSyllabusViewModel>(mockItem));
        //
        //     // Assert
        //     result.Should().BeNull();
        // }
        //
        // [Fact]
        // public async Task UpdateSyllabusAsync_LargeNumberOfSyllabus_PerformanceTest()
        // {
        //     // Arrange
        //     var currentUserId = _fixture.Create<int>();
        //     _claimsServiceMock.Setup(x => x.GetCurrentUserId).Returns(currentUserId);
        //
        //     var mockItems = _fixture.Build<Syllabus>()
        //         .With(x => x.TrainingDeliveryPrincipleId, () => _fixture.Create<int>())
        //         .With(x => x.LastModifiedBy, () => currentUserId)
        //         .With(x => x.LastModifiedOn, () => _currentTimeMock.Object.GetCurrentTime())
        //         .Without(e => e.CreatedAdmin)
        //         .Without(e => e.ModifiedAdmin)
        //         .Without(e => e.TrainingDeliveryPrinciple)
        //         .Without(e => e.Units)
        //         .Without(e => e.TrainingPrograms)
        //         .Without(e => e.AuditPlans)
        //         .CreateMany(1000)
        //         .ToList();
        //
        //     _unitOfWorkMock.Setup(x => x.SyllabusRepo.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(mockItems.First);
        //     _unitOfWorkMock.Setup(x => x.SyllabusRepo.Update(It.IsAny<Syllabus>()));
        //     _unitOfWorkMock.Setup(x => x.SaveChangesAsync()).ReturnsAsync(1);
        //
        //     // Act
        //     var stopwatch = Stopwatch.StartNew();
        //     foreach (var mockItem in mockItems)
        //     {
        //         await _syllabusService.UpdateSyllabusAsync(_mapperConfig.Map<UpdateSyllabusViewModel>(mockItem));
        //     }
        //
        //     stopwatch.Stop();
        //
        //     // Assert
        //     stopwatch.ElapsedMilliseconds.Should().BeLessThan(1000);
        // }
        //
        // [Fact]
        // public async Task UpdateLectureInSyllabusAsync_ValidInput_ReturnUpdatedSyllabus()
        // {
        //     // arrange
        //     var currentUserId = _fixture.Create<int>();
        //     _claimsServiceMock.Setup(x => x.GetCurrentUserId).Returns(currentUserId);
        //
        //     var mockSyllabus = _fixture.Build<Syllabus>()
        //         .With(x => x.TrainingDeliveryPrincipleId, () => _fixture.Create<int>())
        //         .With(x => x.LastModifiedBy, () => currentUserId)
        //         .With(x => x.LastModifiedOn, () => _currentTimeMock.Object.GetCurrentTime())
        //         .Without(e => e.CreatedAdmin)
        //         .Without(e => e.ModifiedAdmin)
        //         .Without(e => e.TrainingDeliveryPrinciple)
        //         .Without(e => e.Units)
        //         .Without(e => e.TrainingPrograms)
        //         .Without(e => e.AuditPlans)
        //         .Create();
        //
        //     var mockLecture = _fixture.Build<Lecture>()
        //         .Create();
        //
        //     var mockViewModel = _fixture.Build<UpdateLectureInSyllabusViewModel>()
        //         .With(x => x.SyllabusId, mockSyllabus.Id)
        //         .With(x => x.LectureId, mockLecture.Id)
        //         .Create();
        //
        //     _unitOfWorkMock.Setup(x => x.SyllabusRepo.GetByIdAsync(mockSyllabus.Id)).ReturnsAsync(mockSyllabus);
        //     _unitOfWorkMock.Setup(x => x.LectureRepo.GetByIdAsync(mockLecture.Id)).ReturnsAsync(mockLecture);
        //     _unitOfWorkMock.Setup(x => x.SyllabusRepo.Update(mockSyllabus));
        //     _unitOfWorkMock.Setup(x => x.SaveChangesAsync()).ReturnsAsync(1);
        //
        //     var expectedResult = _mapperConfig.Map<SyllabusViewModel>(mockSyllabus);
        //
        //     // act
        //     var result = await _syllabusService.UpdateLectureInSyllabusAsync(mockViewModel);
        //
        //     // assert
        //     result.Should().NotBeNull();
        //     result.Should().BeEquivalentTo(expectedResult);
        // }
        //
        // #endregion
        #endregion

        #region GetSyllabus
        // [Fact]
        // public async Task GetSyllabusAsync_ReturnAllSyllabus()
        // {
        //     //arrange
        //     var mockMapper = new Mock<IMapper>();
        //     _unitOfWorkMock.Setup(uow => uow.SyllabusRepo).Returns(_syllabusRepositoryMock.Object);
        //     _unitOfWorkMock.Setup(uow => uow.UnitRepo).Returns(_unitRepositoryMock.Object);
        //     _unitOfWorkMock.Setup(uow => uow.SaveChangesAsync()).ReturnsAsync(1);
        //
        //     var syllabuses = new List<Syllabus>();
        //     var units = new List<Unit>();
        //     var lectures = new List<Lecture>();
        //     var outputStandards = new List<OutputStandard>();
        //
        //     var syllabusId = 1;
        //     var unitId = 2;
        //     var lectureId = 3;
        //     var outputStandardCode = "ABC";
        //     var duration = 60;
        //
        //     outputStandards.Add(new OutputStandard { Code = outputStandardCode });
        //     lectures.Add(new Lecture { Id = lectureId, OutputStandard = outputStandards.First(), Duration = duration });
        //     units.Add(new Unit { Id = unitId, Lectures = lectures });
        //     syllabuses.Add(new Syllabus { Id = syllabusId });
        //
        //     _syllabusRepositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(syllabuses);
        //     //_unitRepositoryMock.Setup(r => r.GetUnitsBySyllabusId(syllabusId)).ReturnsAsync(units);
        //     _unitOfWorkMock.Setup(uow => uow.UnitRepo.GetUnitsBySyllabusId(syllabusId)).ReturnsAsync(units);
        //
        //     var syllabusViewModels = new List<SyllabusViewModel>();
        //     syllabusViewModels.Add(new SyllabusViewModel { Id = syllabusId });
        //     mockMapper.Setup(m => m.Map<List<SyllabusViewModel>>(syllabuses)).Returns(syllabusViewModels);
        //
        //     // Act
        //     var result = await _syllabusService.GetSyllabusAsync();
        //
        //     // Assert
        //     Assert.NotNull(result);
        //     Assert.NotEmpty(result);
        //
        //     var syllabusViewModel = result.First();
        //     Assert.Equal(outputStandardCode, syllabusViewModel.OutPutStandardCode.First());
        //     Assert.Equal(duration, syllabusViewModel.Duration);
        // }






        #region ServiceTests AddSyllabus
        //[Fact]
        public async Task AddSyllabusAsync_ReturnCorrectData_WhenSaveSuccess()
        {
            //arrange
            var mocks = _fixture.Build<AddSyllabusViewModel>().Create();
            _unitOfWorkMock.Setup(x => x.SyllabusRepo.AddAttach(It.IsAny<Syllabus>()));
            _unitOfWorkMock.Setup(x => x.SaveChangesAsync()).ReturnsAsync(1);

            //act
            var result = await _syllabusService.AddSyllabusAsync(mocks);

            //assert
            _unitOfWorkMock.Verify(x => x.SyllabusRepo.AddAttach(It.IsAny<Syllabus>()), Times.Once());
            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(), Times.Once());
        }

        //[Fact]
        public async Task AddSyllabusAsync_ReturnNull_WhenSaveFail()
        {
            //arrange
            var mocks = _fixture.Build<AddSyllabusViewModel>().Create();
            _unitOfWorkMock.Setup(x => x.SyllabusRepo.AddAttach(It.IsAny<Syllabus>()));
            _unitOfWorkMock.Setup(x => x.SaveChangesAsync()).ReturnsAsync(0);

            //act
            var result = await _syllabusService.AddSyllabusAsync(mocks);

            //assert
            _unitOfWorkMock.Verify(x => x.SyllabusRepo.AddAttach(It.IsAny<Syllabus>()), Times.Once());
            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(), Times.Once());
            result.Should().BeNull();
        }
        #endregion
    }
}
    #endregion