using Application.Repositories;
using Application;
using AutoMapper;
using Domain.Tests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Interfaces;
using AutoFixture;
using Application.Services;
using Infracstructures.Repositories;
using Application.ViewModels.ClassViewModels;
using Domain.Models;
using Moq;
using FluentAssertions;
using Castle.Components.DictionaryAdapter.Xml;
using Domain.Models.Users;
using Application.ViewModels.TrainingProgramViewModels;
using Xunit.Sdk;
using Microsoft.AspNetCore.Http;
using Domain.Enums.ClassEnums;

namespace Application.Tests.Services
{
    public class ClassServiceTests : SetupTest
    {
        private readonly IClassService _classService;
        private Fixture _customFixture;

        public ClassServiceTests()
        {
            _classService = new ClassService(
                _unitOfWorkMock.Object,
                _currentTimeMock.Object,
                _configuration, _mapperConfig,
                _claimsServiceMock.Object, _trainingProgramServiceMock.Object);
            _customFixture = new Fixture();
            _customFixture.Customizations.Add(new TypePropertyOmitter(
               "TimeMngSystem", "Role", "CreatedTrainingProgram", "ModifyTrainingProgram", "CreatedSyllabus", "ModifiedSyllabus", "ApprovedClass", "CreatedClass", "FeedbackTrainee"
               , "FeedbackTrainer", "GradeReports", "ClassUsers", "CreatedAuditPlans", "AuditedAuditDetails", "TakenAuditDetails", "TimeMngSystemList",
               "TrainingMaterials", "QuizBanks", "QuizRecords", "Level", "FullName", "Email", "Password"
               , /*Class*/ "ClassUsers", "ClassUnitDetails", "Syllabuses", "AuditPlans", "ApprovedAdmin", "CreatedAdmin", "TrainingProgram"
               /*ClassUser*/ , "Attendances", "User", "Class"
               ));
        }

        #region ListClassAsync
        [Fact]
        public async void GetClassListAsync_ReturnList()
        {
            //arange
            var mocksClass = _customFixture.Build<Class>().CreateMany(3).ToList();
            _unitOfWorkMock.Setup(x => x.ClassRepo.GetAllAsync()).ReturnsAsync(mocksClass);

            var mocksUser = _customFixture.Build<User>().Create();
            _unitOfWorkMock.Setup(x => x.UserRepo.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(mocksUser);

            var mocksClassUsers = _customFixture.Build<ClassUsers>().CreateMany(3).ToList();
            _unitOfWorkMock.Setup(x => x.ClassUserRepo.GetClassUsers(It.IsAny<int>())).ReturnsAsync(mocksClassUsers);

            //act
            var result = await _classService.GetClassListAsync();

            //assert
            _unitOfWorkMock.Verify(x => x.ClassRepo.GetAllAsync(), Times.Once);
            result.Should().NotBeNull();
        }

        [Fact]
        public async void GetClassListAsync_ReturnException_WhenListClassIsNull()
        {
            //arange
            var mocksClass = _customFixture.Build<Class>().CreateMany(0).ToList();
            _unitOfWorkMock.Setup(x => x.ClassRepo.GetAllAsync()).ReturnsAsync(mocksClass);

            //act
            var result = Assert.ThrowsAsync<Exception>(async () => await _classService.GetClassListAsync());

            //assert
            _unitOfWorkMock.Verify(x => x.ClassRepo.GetAllAsync(), Times.Once);
            Assert.Equal("Class is null.", result.Result.Message);
        }

        #endregion

        #region Update Class
        [Fact]
        public async void UpdateClass_ReturnTrue_ExistID()
        {
            //Arrange
            var mockClass = _customFixture.Build<Class>().Create();

            var mockUpdateClass = _fixture.Build<UpdateClassViewModel>().Create();
            mockUpdateClass.StatusClass = "Closed";
            mockUpdateClass.Location = "HaNoi";
            mockUpdateClass.Attendee = "Intern";

            _unitOfWorkMock.Setup(x => x.ClassRepo.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(mockClass);
            _unitOfWorkMock.Setup(x => x.ClassRepo.Update(mockClass));
            _unitOfWorkMock.Setup(x => x.SaveChangesAsync()).ReturnsAsync(1);

            //Act
            var result = await _classService.UpdateClassAsync(1, mockUpdateClass);

            //Assert
            //_unitOfWorkMock.Verify(x => x.ClassRepo.Update(mockClass), Times.Once);
            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(), Times.Once);
            result.Should().BeTrue();
        }

        [Fact]
        public async void UpdateClass_ThrowException_NoExistID()
        {
            //Arrange
            int id = 0;
            _unitOfWorkMock.Setup(x => x.ClassRepo.GetByIdAsync(It.IsAny<int>())).ThrowsAsync(new Exception("Not found class"));
            //Act
            var result = async () => await _classService.UpdateClassAsync(id, It.IsAny<UpdateClassViewModel>());
            //Assert
            _unitOfWorkMock.Verify(x => x.ClassRepo.Update(It.IsAny<Class>()), Times.Never);
            result.Should().ThrowAsync<Exception>().WithMessage("Not found class");
        }
        #endregion

        #region CreateClass
        [Fact]
        public async void CreateClass_ReturnSucceed()
        {
            //Arrange
            var mockClass = _customFixture.Build<CreateClassViewModel>().Create();
            var mocksListInt = new List<int>() { 1, 2, 3 };

            var mapperClass = _mapperConfig.Map<Class>(mockClass);
            _unitOfWorkMock.Setup(x => x.TrainingProgramRepo.getStatusTrainingProgram(It.IsAny<int>())).ReturnsAsync(true);
            _classServiceMock.Setup(x => x.AddClassUnit(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(true);
            _classServiceMock.Setup(x => x.GetUnitIdList(It.IsAny<int>())).ReturnsAsync(mocksListInt);
            _unitOfWorkMock.Setup(x => x.SyllabusRepo.GetUnitId(It.IsAny<List<int>>())).ReturnsAsync(mocksListInt);
            _unitOfWorkMock.Setup(x => x.ClassUnitRepo.AddAsync(It.IsAny<ClassUnitDetail>()));
            _unitOfWorkMock.Setup(x => x.ClassRepo.AddAttach(It.IsAny<Class>()));
            _unitOfWorkMock.Setup(x => x.SaveChangesAsync()).ReturnsAsync(1);

            //Act
            var result = await _classService.CreateClassAsync(mockClass);

            //Assert
            _unitOfWorkMock.Verify(x => x.ClassRepo.AddAttach(It.IsAny<Class>()), Times.Once);
            result.Should().BeSameAs("Create Class Succeed");
        }

        [Fact]
        public async void CreateClass_ReturnException_WhenStatusFalse()
        {
            //Arrange
            var mockClass = _fixture.Build<CreateClassViewModel>().Create();
            _unitOfWorkMock.Setup(x => x.TrainingProgramRepo.getStatusTrainingProgram(It.IsAny<int>())).ReturnsAsync(false);

            //Act
            var result = async () => await _classService.CreateClassAsync(mockClass);

            //Assert
            _unitOfWorkMock.Verify(x => x.ClassRepo.AddAttach(It.IsAny<Class>()), Times.Never);
            result.Should().ThrowAsync<Exception>().WithMessage("Training Program is not avaiable");
        }

        [Fact]
        public async void CreateClass_ReturnException_WhenSaveFailed()
        {
            var mockClass = _fixture.Build<CreateClassViewModel>().Create();

            var mapperClass = _mapperConfig.Map<Class>(mockClass);
            _unitOfWorkMock.Setup(x => x.TrainingProgramRepo.getStatusTrainingProgram(It.IsAny<int>())).ReturnsAsync(true);
            _unitOfWorkMock.Setup(x => x.ClassRepo.AddAttach(It.IsAny<Class>()));
            _unitOfWorkMock.Setup(x => x.SaveChangesAsync()).ReturnsAsync(0);
            //Act
            var result = async () => await _classService.CreateClassAsync(mockClass);
            //Assert
            result.Should().ThrowAsync<Exception>().WithMessage("Save Failed");
        }

        [Fact]
        public async void CreateClass_ReturnException_WhenAddClassUnitSuccessFailed()
        {
            //Arrange
            var mocksListInt = new List<int>() { 1, 2, 3 };

            _classServiceMock.Setup(x => x.GetUnitIdList(It.IsAny<int>())).ReturnsAsync(mocksListInt);
            _unitOfWorkMock.Setup(x => x.SyllabusRepo.GetUnitId(It.IsAny<List<int>>())).ReturnsAsync(mocksListInt);
            _unitOfWorkMock.Setup(x => x.ClassUnitRepo.AddAsync(It.IsAny<ClassUnitDetail>()));
            _unitOfWorkMock.Setup(x => x.ClassRepo.GetSyllabusId(It.IsAny<int>())).ReturnsAsync(mocksListInt);
            _unitOfWorkMock.Setup(x => x.SaveChangesAsync()).ReturnsAsync(0);
            //Act
            var result = await _classService.AddClassUnit(1, 1);

            //Assert
            _unitOfWorkMock.Verify(x => x.ClassUnitRepo.AddAsync(It.IsAny<ClassUnitDetail>()), Times.Once);
            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(), Times.Once);
            result.Should().BeFalse();
        }
        #endregion

        #region  GetOpeningClassListAsync
        [Fact]
        public async void GetOpeningClassListAsync_ReturnList()
        {
            //Arrange
            var mocks = _fixture.Build<ClassViewModel>().With(x => x.Status, "Openning").CreateMany(3).ToList();
            _classServiceMock.Setup(x => x.GetClassListAsync()).ReturnsAsync(mocks);

            var mocksClass = _customFixture.Build<Class>().CreateMany(3).ToList();
            _unitOfWorkMock.Setup(x => x.ClassRepo.GetAllAsync()).ReturnsAsync(mocksClass);

            var mocksUser = _customFixture.Build<User>().Create();
            _unitOfWorkMock.Setup(x => x.UserRepo.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(mocksUser);

            var mocksClassUsers = _customFixture.Build<ClassUsers>().CreateMany(3).ToList();
            _unitOfWorkMock.Setup(x => x.ClassUserRepo.GetClassUsers(It.IsAny<int>())).ReturnsAsync(mocksClassUsers);

            //Act
            var result = await _classService.GetOpeningClassListAsync();

            //Assert
            result.Should().NotBeNull();
        }

        [Fact]
        public async void GetOpeningClassListAsync_ReturnException_WhenStatusNotOpenning()
        {
            //Arrange
            var mocks = _fixture.Build<ClassViewModel>().With(x => x.Status, "Closed").CreateMany(1).ToList();
            var mocksClass = _customFixture.Build<Class>().CreateMany(3).ToList();
            foreach (var item in mocksClass)
            {
                item.Status = ClassStatus.Closed;
            }
            var mockUser = _customFixture.Build<User>().Create();
            var mockClassUser = _customFixture.Build<ClassUsers>().CreateMany(3).ToList();


            _unitOfWorkMock.Setup(x => x.ClassRepo.GetAllAsync()).ReturnsAsync(mocksClass);
            _unitOfWorkMock.Setup(x => x.UserRepo.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(mockUser);
            _unitOfWorkMock.Setup(x => x.ClassUserRepo.GetClassUsers(It.IsAny<int>())).ReturnsAsync(mockClassUser);
            _classServiceMock.Setup(x => x.GetClassListAsync()).ReturnsAsync(mocks);
            //Act
            var result = Assert.ThrowsAsync<Exception>(async () => await _classService.GetOpeningClassListAsync());

            //Assert
            Assert.Equal("NO OPENNING CLASS", result.Result.Message);
        }
        #endregion

        #region AddUserToClassAsync
        [Fact]
        public async void AddUserToClassAsync_ReturnException_WhenUserAlreadyInClass()
        {
            //Arrange
            var mockUser = _customFixture.Build<User>().Create();
            var mockClass = _customFixture.Build<Class>().Create();
            var mockClassView = _customFixture.Build<AddUserToClassViewModel>().Create();

            _unitOfWorkMock.Setup(x => x.ClassRepo.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(mockClass);
            _unitOfWorkMock.Setup(x => x.UserRepo.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(mockUser);
            _unitOfWorkMock.Setup(x => x.ClassUserRepo.IsExist(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(true);

            //Act
            var result = Assert.ThrowsAsync<Exception>(async () => await _classService.AddUserToClassAsync(mockClassView));
            //Assert
            Assert.Equal("This user is already in this class", result.Result.Message);
        }

        [Fact]
        public async void AddUserToClassAsync_ReturnSucceed()
        {
            //Arrange
            var mockUser = _customFixture.Build<User>().Create();
            var mockClass = _customFixture.Build<Class>().Create();
            var mockClassView = _customFixture.Build<AddUserToClassViewModel>().Create();

            _unitOfWorkMock.Setup(x => x.ClassRepo.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(mockClass);
            _unitOfWorkMock.Setup(x => x.UserRepo.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(mockUser);
            _unitOfWorkMock.Setup(x => x.ClassUserRepo.IsExist(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(false);
            _unitOfWorkMock.Setup(x => x.ClassUserRepo.AddAsync(It.IsAny<ClassUsers>())).Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(x => x.SaveChangesAsync()).ReturnsAsync(1);
            //Act
            var result = await _classService.AddUserToClassAsync(mockClassView);
            //Assert
            _unitOfWorkMock.Verify(x => x.ClassUserRepo.AddAsync(It.IsAny<ClassUsers>()), Times.Once);
            result.Should().BeSameAs("Add User To Class Succeed");
        }

        [Fact]
        public async void AddUserToClassAsync_ReturnException_WhenClassNotFound()
        {
            //Arrange
            var mockUser = _customFixture.Build<User>().Create();
            int idClass = 0;
            var mockClassView = _customFixture.Build<AddUserToClassViewModel>().Create();

            _unitOfWorkMock.Setup(x => x.ClassRepo.GetByIdAsync(idClass));
            _unitOfWorkMock.Setup(x => x.UserRepo.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(mockUser);
            _unitOfWorkMock.Setup(x => x.ClassUserRepo.IsExist(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(false);
            //Act
            var result = Assert.ThrowsAsync<Exception>(async () => await _classService.AddUserToClassAsync(mockClassView));
            //Assert
            _unitOfWorkMock.Verify(x => x.ClassUserRepo.AddAsync(It.IsAny<ClassUsers>()), Times.Never);
            Assert.Equal("Class Not Found", result.Result.Message);
        }

        [Fact]
        public async void AddUserToClassAsync_ReturnException_WhenUserNotFound()
        {
            //Arrange
            int idUser = 0;
            var mockClass = _customFixture.Build<Class>().Create();
            var mockClassView = _customFixture.Build<AddUserToClassViewModel>().Create();

            _unitOfWorkMock.Setup(x => x.ClassRepo.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(mockClass);
            _unitOfWorkMock.Setup(x => x.UserRepo.GetByIdAsync(idUser));
            _unitOfWorkMock.Setup(x => x.ClassUserRepo.IsExist(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(false);
            //Act
            var result = Assert.ThrowsAsync<Exception>(async () => await _classService.AddUserToClassAsync(mockClassView));
            //Assert
            _unitOfWorkMock.Verify(x => x.ClassUserRepo.AddAsync(It.IsAny<ClassUsers>()), Times.Never);
            Assert.Equal("User Not Found", result.Result.Message);
        }
        #endregion

        #region Clone Class
        [Fact]
        public async void CloneClass_ReturnException_WhenClassIdLessThan1()
        {
            //Arrange
            int id = 0;
            //Act
            var result = Assert.ThrowsAsync<Exception>(async () => await _classService.CloneClassAsync(id));

            //Assert
            Assert.Equal("Class id must be an integer with value equal or greater than 1", result.Result.Message);
        }

        [Fact]
        public async void CloneClass_ReturnException_WhenClassNotFound()
        {
            //Arrange
            Class newClass = null;
            _unitOfWorkMock.Setup(x => x.ClassRepo.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(newClass);

            //Act
            var result = Assert.ThrowsAsync<Exception>(async () => await _classService.CloneClassAsync(1));

            //Assert
            Assert.Equal("Class not found", result.Result.Message);
        }
        [Fact]
        public async void CloneClass_Succeed()
        {
            //Arrange
            var mockClass = _customFixture.Build<Class>().Create();
            _unitOfWorkMock.Setup(x => x.ClassRepo.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(mockClass);
            _unitOfWorkMock.Setup(x => x.ClassRepo.CloneAsync(mockClass)).ReturnsAsync(mockClass);
            _unitOfWorkMock.Setup(x => x.ClassRepo.AddAsync(It.IsAny<Class>())).Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(x => x.SaveChangesAsync()).ReturnsAsync(1);
            _trainingProgramServiceMock.Setup(x => x.CloneTrainingProgramAsync(It.IsAny<int>()));
            //Act
            var result = _classService.CloneClassAsync(1);
            //Assert
            _unitOfWorkMock.Verify(x => x.ClassRepo.AddAsync(It.IsAny<Class>()), Times.Once);
            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(), Times.Once);
        }
        #endregion

        #region Get Class Detail
        //[Fact]
        public async void GetClassDetail_Return()
        {
            //Arrange
            Fixture _customLocalFixture = new Fixture();
            _customLocalFixture.Customizations.Add(new TypePropertyOmitter(
                   "TimeMngSystem", "CreatedTrainingProgram", "ModifyTrainingProgram", "CreatedSyllabus", "ModifiedSyllabus", "ApprovedClass", "FeedbackTrainee"
                   , "FeedbackTrainer", "GradeReports", "CreatedAuditPlans", "AuditedAuditDetails", "TakenAuditDetails", "TimeMngSystemList",
                   "TrainingMaterials", "QuizBanks", "QuizRecords", "Level", "Email", "Password"

                   ));

            var mockClass = _fixture.Build<Class>().Create();
            _unitOfWorkMock.Setup(x => x.ClassRepo.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(mockClass);
            //Act
            var result = await _classService.GetClassDetail(1);
            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<ClassDetailViewModels>();
        }
        #endregion
    }
}
