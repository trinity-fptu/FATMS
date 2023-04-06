using Application.Commons;
using Application.ResponseModels;
using Application.ViewModels.SyllabusViewModels;
using Application.ViewModels.TrainingProgramViewModels;
using AutoFixture;
using Castle.Components.DictionaryAdapter.Xml;
using Domain.Models;
using Domain.Models.Syllabuses;
using Domain.Tests;
using Fare;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Data;
using WebAPI.Controllers;
using Xunit.Sdk;

namespace WebAPI.Tests.Controllers
{
    public class TrainingProgramControllerTests : SetupTest
    {
        private readonly TrainingProgramController _trainingProgramController;

        public TrainingProgramControllerTests()
        {
            _trainingProgramController = new TrainingProgramController(_trainingProgramServiceMock.Object);
        }

        #region Test GetTrainingProgramById
        [Fact]
        public async void GetTrainingProgramById_ExistTrainingProgramIdPassed_ReturnCorrectTrainingProgramDetail()
        {
            // Arrange
            var mock = _fixture.Build<TrainingProgramViewModels>().Create();
            _trainingProgramServiceMock.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(mock);

            // Act
            var result = await _trainingProgramController.GetByIdAsync(1) as OkObjectResult;

            // Assert
            _trainingProgramServiceMock.Verify(x => x.GetByIdAsync(It.IsAny<int>()), Times.Once);
            result.Should().BeOfType<OkObjectResult>();
            result.Value.Should().NotBeNull();
        }

        [Fact]
        public async void GetTrainingProgramById_NoExistTrainingProgramIdPassed_ReturnNotFoundResult()
        {
            // Arrange
            _trainingProgramServiceMock.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ThrowsAsync(new InvalidOperationException());

            // Act
            var result = await _trainingProgramController.GetByIdAsync(1);

            // Assert
            _trainingProgramServiceMock.Verify(x => x.GetByIdAsync(It.IsAny<int>()), Times.Once);
            result.Should().BeOfType<NotFoundObjectResult>();
        }

        [Fact]
        public async void GetTrainingProgramById_InvalidUserIdPassed_ReturnBadRequestResult()
        {
            // Arrange
            int id = 0;
            _trainingProgramServiceMock.Setup(x => x.GetByIdAsync(id)).Throws(new ArgumentException("Training Program id cannot be less than 1."));

            // Act
            var result = await _trainingProgramController.GetByIdAsync(id) as BadRequestObjectResult;

            // Assert
            _trainingProgramServiceMock.Verify(x => x.GetByIdAsync(0), Times.Once);
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        #endregion

        #region GetAllTrainingProgram
        [Fact]
        public async void GetAllTrainingProgram_ReturnList()
        {
            //Arrange
            var mocks = _fixture.Build<TrainingProgramViewModels>().CreateMany(3).ToList();
            _trainingProgramServiceMock.Setup(x => x.GetAllAsync()).ReturnsAsync(mocks);

            //Act
            var result = await _trainingProgramController.GetAllAsync() as OkObjectResult;

            //Assert
            _trainingProgramServiceMock.Verify(x => x.GetAllAsync(), Times.Once);
            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async void GetAllTrainingProgram_ReturnNotFound_WhenListEmpty()
        {
            //Arrange
            _trainingProgramServiceMock.Setup(x => x.GetAllAsync()).Throws(new Exception("No Item"));

            //Act
            var result = await _trainingProgramController.GetAllAsync() as NotFoundObjectResult;

            //Assert
            _trainingProgramServiceMock.Verify(x => x.GetAllAsync(), Times.Once);
            result.Should().BeOfType<NotFoundObjectResult>();
        }
        #endregion

        #region GetAllTrainingProgramIsActive
        [Fact]
        public async void GetAllTrainingProgramIsActive_ReturnOk()
        {
            //Arrange
            var mocks = _fixture.Build<TrainingProgramViewModels>().CreateMany(3).ToList();
            foreach (var item in mocks)
            {
                item.IsActive = true;
            }
            _trainingProgramServiceMock.Setup(x => x.GetAllTrainingProgramIsActiveAsync()).ReturnsAsync(mocks);
            
            //Act
            var result = await _trainingProgramController.GetAllTrainingProgramIsActiveAsync() as OkObjectResult;

            //Assert
            _trainingProgramServiceMock.Verify(x => x.GetAllTrainingProgramIsActiveAsync(), Times.Once);
            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async void GetAllTrainingProgramIsActive_ReturnNotFound_WhenListEmpty()
        {
            //Arrange
            var mocks = _fixture.Build<TrainingProgramViewModels>().CreateMany(0).ToList();
            foreach (var item in mocks)
            {
                item.IsActive = true;
            }
            _trainingProgramServiceMock.Setup(x => x.GetAllTrainingProgramIsActiveAsync()).Throws(new Exception("No Item"));

            //Act
            var result = await _trainingProgramController.GetAllTrainingProgramIsActiveAsync() as NotFoundObjectResult;

            //Assert
            _trainingProgramServiceMock.Verify(x => x.GetAllTrainingProgramIsActiveAsync(), Times.Once);
            result.Should().BeOfType<NotFoundObjectResult>();
            result.Value.Should().BeEquivalentTo("No Item");
        }
        #endregion

        #region Get TrainingProgram Is Active By Name
        [Fact]
        public async void GetTrainingProgramIsActiveByName_ReturnList()
        {
            //Arrange
            var mocks = _fixture.Build<TrainingProgramViewModels>().CreateMany(3).ToList();
            _trainingProgramServiceMock.Setup(x => x.GetTrainingProgramIsActiveByNameAsync(It.IsAny<string>())).ReturnsAsync(mocks);

            //Act
            var result = await _trainingProgramController.GetTrainingProgramIsActiveByNameAsync("a") as OkObjectResult;

            //Assert
            _trainingProgramServiceMock.Verify(x => x.GetTrainingProgramIsActiveByNameAsync(It.IsAny<string>()), Times.Once);
            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async void GetTrainingProgramIsActiveByName_ReturnList_WhenNameIsEmpty()
        {
            //Arrange
            var mocks = _fixture.Build<TrainingProgramViewModels>().CreateMany(3).ToList();
            _trainingProgramServiceMock.Setup(x => x.GetAllTrainingProgramIsActiveAsync()).ReturnsAsync(mocks);

            //Act
            var result = await _trainingProgramController.GetTrainingProgramIsActiveByNameAsync("") as OkObjectResult;

            //Assert
            _trainingProgramServiceMock.Verify(x => x.GetTrainingProgramIsActiveByNameAsync(It.IsAny<string>()), Times.Never);
            result.Should().BeOfType<OkObjectResult>();
        }
        #endregion

        #region Get Training Program By Name
        [Fact]
        public async void GetTrainingProgramByName_ReturnList()
        {
            //Arrange
            var mocks = _fixture.Build<TrainingProgramViewModels>().CreateMany(3).ToList();
            _trainingProgramServiceMock.Setup(x => x.GetTrainingProgramByNameAsync(It.IsAny<string>())).ReturnsAsync(mocks);

            //Act
            var result = await _trainingProgramController.GetTrainingProgramByNameAsync("a") as OkObjectResult;

            //Assert
            _trainingProgramServiceMock.Verify(x => x.GetTrainingProgramByNameAsync(It.IsAny<string>()), Times.Once);
            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async void GetTrainingProgramByName_ReturnList_WhenNameIsEmpty()
        {
            //Arrange
            var mocks = _fixture.Build<TrainingProgramViewModels>().CreateMany(3).ToList();
            _trainingProgramServiceMock.Setup(x => x.GetAllAsync()).ReturnsAsync(mocks);

            //Act
            var result = await _trainingProgramController.GetTrainingProgramByNameAsync("") as OkObjectResult;

            //Assert
            _trainingProgramServiceMock.Verify(x => x.GetTrainingProgramByNameAsync(It.IsAny<string>()), Times.Never);
            result.Should().BeOfType<OkObjectResult>();
        }
        #endregion

        #region Get TrainingProgram By Date Range CreateOn
        [Fact]
        public async void GetTrainingProgramByDateRangeCreateOn_ReturnListTrainigProgram()
        {
            //Arrange
            var mock = _fixture.Build<TrainingProgramViewModels>().CreateMany(3).ToList();
            _trainingProgramServiceMock.Setup(x => x.GetTrainingProgramByDateRangeCreateOnAsync(It.IsAny<DateTime[]>())).ReturnsAsync(mock);

            //Action
            var result = await _trainingProgramController.GetTrainingProgramByDateRangeCreateOnAsync(new DateTime[] { new DateTime(2002, 01, 01), new DateTime(2010, 10, 2) }) as OkObjectResult;

            //Assert  
            result.Should().BeOfType<OkObjectResult>();
        }
        #endregion

        #region TrainingProgram By Date Range LastModify
        [Fact]
        public async void GetTrainingProgramByDateRangeListModify_ReturnListTrainigProgram()
        {
            //Arrange
            var mock = _fixture.Build<TrainingProgramViewModels>().CreateMany(3).ToList();
            _trainingProgramServiceMock.Setup(x => x.GetTrainingProgramByDateRangeLastModifyAsync(It.IsAny<DateTime[]>())).ReturnsAsync(mock);

            //Action
            var result = await _trainingProgramController.GetTrainingProgramByDateRangeLastModifyAsync(new DateTime[] { new DateTime(2002, 01, 01), new DateTime(2010, 10, 2) }) as OkObjectResult;

            //Assert  
            result.Should().BeOfType<OkObjectResult>();
        }
        #endregion

        #region UpdateTrainingProgramController
        [Fact]
        public async void UpdateTrainingProgram_ExistID_ReturnOk()
        {
            //arange
            var tpModel = _fixture.Build<UpdateTrainingProgramViewModel>().Create();
            _trainingProgramServiceMock.Setup(x => x.UpdateTrainingProgramsAsync(It.IsAny<int>(), It.IsAny<UpdateTrainingProgramViewModel>())).ReturnsAsync(true);

            //act
            var result = await _trainingProgramController.UpdateTrainingProgramsAsync(1, tpModel) as OkObjectResult;

            //assert
            _trainingProgramServiceMock.Verify(x => x.UpdateTrainingProgramsAsync(It.IsAny<int>(), It.IsAny<UpdateTrainingProgramViewModel>()), Times.Once);
            result.Should().BeOfType<OkObjectResult>();
            result.Should().NotBeNull();
        }

        [Fact]
        public async void UpdateTrainingProgram_NotExistID_ReturnNull()
        {
            //arange
            int id = 0;
            _trainingProgramServiceMock.Setup(x => x.UpdateTrainingProgramsAsync(id, It.IsAny<UpdateTrainingProgramViewModel>())).ThrowsAsync(new Exception());

            //act
            var result = await _trainingProgramController.UpdateTrainingProgramsAsync(id, It.IsAny<UpdateTrainingProgramViewModel>()) as BadRequestObjectResult;

            //assert
            _trainingProgramServiceMock.Verify(x => x.UpdateTrainingProgramsAsync(id, It.IsAny<UpdateTrainingProgramViewModel>()), Times.Once);
            result.Should().BeOfType<BadRequestObjectResult>();
        }
        #endregion

        #region Clone TrainingProgram
        [Fact]
        public async void CloneTrainingProgram_ReturnOk()
        { 
            //Arrange
            _trainingProgramServiceMock.Setup(x => x.CloneTrainingProgramAsync(It.IsAny<int>()));
            //Act
            var result = await _trainingProgramController.CloneTrainingProgramAsync(1) as OkObjectResult;
            //Assert
            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async void CloneTrainingProgram_ReturnBadRequest()
        {
            //Arrange
            _trainingProgramServiceMock.Setup(x => x.CloneTrainingProgramAsync(It.IsAny<int>())).ThrowsAsync(new Exception());
            //Act
            var result = await _trainingProgramController.CloneTrainingProgramAsync(1) as BadRequestObjectResult;
            //Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }
        #endregion

        #region FilterTrainingProgram
        [Fact]
        public async void FilterTrainingProgram_ReturnOk()
        {
            //Arrange
            var mock = _fixture.Build<TrainingProgramFilterModel>().Create();
            var mockTrainingProgramViewModel = _fixture.Build<TrainingProgramViewModels>().CreateMany().ToList();

            _trainingProgramServiceMock.Setup(x => x.Filter(It.IsAny<TrainingProgramFilterModel>())).ReturnsAsync(mockTrainingProgramViewModel);
            //Act
            var result = await _trainingProgramController.Filter(mock) as OkObjectResult;
            //Assert
            result.Should().BeOfType<OkObjectResult>();
        }
        #endregion

        #region Test Create Training Program
        [Fact]
        public async Task CreateTrainingProgram_InputNameNull_ShouldReturnBadRequest()
        {
            //arrange
            var mockModelRequest = _fixture.Build<CreateTrainingProgramViewModels>().With(x => x.Name, "").Create();

            var mockModelResponse = _fixture.Build<TrainingProgramViewModels>().Create();

            _trainingProgramServiceMock.Setup(x => x.CreateTrainingProgramAsync(It.IsAny<CreateTrainingProgramViewModels>()))
                .ReturnsAsync(mockModelResponse);

            //act
            var result = await _trainingProgramController.CreateTrainingProgram(mockModelRequest) as BadRequestObjectResult;

            //assert
            _trainingProgramServiceMock.Verify(x => x.CreateTrainingProgramAsync(mockModelRequest), Times.Never());

            result.Should().BeOfType<BadRequestObjectResult>();
            /*var a = (BaseFailedResponseModel)result.Value;
            a.Message.ToString().Equals("The name TrainingProgram isn't empty.");*/
        }

        [Fact]
        public async Task CreateTrainingProgram_NameDuplication_ReturnBadRequest()
        {
            //Arr
            var mockModelRequest = _fixture.Build<CreateTrainingProgramViewModels>().With(x => x.Name, "aaa").Create();

            _trainingProgramServiceMock.Setup(x => x.CreateTrainingProgramAsync(It.IsAny<CreateTrainingProgramViewModels>())).ThrowsAsync(new ArgumentNullException());

            //act
            var result = await _trainingProgramController.CreateTrainingProgram(mockModelRequest) as BadRequestObjectResult;

            //assert
            _trainingProgramServiceMock.Verify(x => x.CreateTrainingProgramAsync(It.IsAny<CreateTrainingProgramViewModels>()), Times.Once());

            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task CreateTrainingProgram_SaveFaied_ReturnBadRequest()
        {
            //Arr
            var mockModelRequest = _fixture.Build<CreateTrainingProgramViewModels>().With(x => x.Name, "aaa").Create();

            _trainingProgramServiceMock.Setup(x => x.CreateTrainingProgramAsync(mockModelRequest)).ThrowsAsync(new Exception());

            //act
            var result = await _trainingProgramController.CreateTrainingProgram(mockModelRequest) as BadRequestObjectResult;

            //assert
            _trainingProgramServiceMock.Verify(x => x.CreateTrainingProgramAsync(mockModelRequest), Times.Once());

            result.Should().BeOfType<BadRequestObjectResult>();
            Assert.IsAssignableFrom<BadRequestObjectResult>(result);
            Assert.Equal(StatusCodes.Status400BadRequest, (result as ObjectResult)!.StatusCode);
        }

        [Fact]
        public async Task CreateTrainingProgram_ShouldReturnOKResult()
        {
            var mockModelRequest = _fixture.Build<CreateTrainingProgramViewModels>().Create();

            var mockModelResponse = _fixture.Build<TrainingProgramViewModels>().Create();

            //arrange
            _trainingProgramServiceMock.Setup(x => x.CreateTrainingProgramAsync(mockModelRequest)).ReturnsAsync(mockModelResponse);

            //act
            var result = await _trainingProgramController.CreateTrainingProgram(mockModelRequest) as OkObjectResult;

            //assert
            _trainingProgramServiceMock.Verify(x => x.CreateTrainingProgramAsync(It.Is<CreateTrainingProgramViewModels>(
                x => x.Equals(mockModelRequest))), Times.Once());
            result.Should().BeOfType<OkObjectResult>();
        }
        #endregion

        #region DeleteTrainingProgram
        [Fact]
        public async void Delete_CorrectId_ChangeIsDeleteToTrue()
        {
            // Arrange
            int id = 2;
            _trainingProgramServiceMock.Setup(x => x.DeleteTrainingProgramsAsync(id));

            // Act
            var result = await _trainingProgramController.DeleteTrainingProgramsAsync(id) as OkObjectResult;

            // Assert
            _trainingProgramServiceMock.Verify(x => x.DeleteTrainingProgramsAsync(id), Times.Once);
            result.Should().BeOfType<OkObjectResult>();
            result.Value.Should().NotBeNull();
        }

        [Fact]
        public async void Delete_InvalidTrainingProgram_ReturnBadRequestResult()
        {
            // Arrange
            int id = 0;
            _trainingProgramServiceMock.Setup(x => x.DeleteTrainingProgramsAsync(id)).Throws(new Exception("Don't found this Training Program"));

            // Act
            var result = await _trainingProgramController.DeleteTrainingProgramsAsync(id) as BadRequestObjectResult;

            // Assert
            _trainingProgramServiceMock.Verify(x => x.DeleteTrainingProgramsAsync(id), Times.Once);
            result.Should().BeOfType<BadRequestObjectResult>();

        }

        #endregion

    }
}
