using Application.ViewModels.ClassViewModels;
using Application.ViewModels.TrainingProgramViewModels;
using Application.ViewModels.UserViewModels;
using AutoFixture;
using Domain.Enums.UserEnums;
using Domain.Models;
using Domain.Tests;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAPI.Controllers;
using Xunit;

namespace WebAPI.Tests.Controllers
{
    public class ClassControllerTests : SetupTest
    {

        private readonly ClassController _classController;
        private Fixture _customFixture;
        public ClassControllerTests() {
            _classController = new ClassController(_classServiceMock.Object);
        }

        #region GetListClass
        [Fact]
        public async void GetListClass_ReturnList()
        {
            //Arrange
            var mocks = _fixture.Build<ClassViewModel>().CreateMany(3).ToList();
            _classServiceMock.Setup(x => x.GetClassListAsync()).ReturnsAsync(mocks);

            //Act
            var result = await _classController.ListClassAsync() as OkObjectResult;

            //Assert
            _classServiceMock.Verify(x => x.GetClassListAsync(), Times.Once);
            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async void GetListClass_ReturnNotFound_WhenListEmpty()
        {
            //Arrange
            var mocks = _fixture.Build<ClassViewModel>().Create();
            _classServiceMock.Setup(x => x.GetClassListAsync()).Throws(new Exception());

            //Act
            var result = await _classController.ListClassAsync() as BadRequestObjectResult;

            //Assert
            _classServiceMock.Verify(x => x.GetClassListAsync(), Times.Once);
            result.Should().BeOfType<BadRequestObjectResult>();
        }
        #endregion

        #region ListOpeningClassAsync
        [Fact]
        public async void ListOpeningClassAsync_ReturnOk()
        {
            //Arrange
            var mocks = _fixture.Build<ClassViewModel>().With(x => x.Status, "Opening").CreateMany(3).ToList();
            _classServiceMock.Setup(x => x.GetOpeningClassListAsync()).ReturnsAsync(mocks);

            //Act
            var result = await _classController.ListOpeningClassAsync() as OkObjectResult;

            //Assert
            _classServiceMock.Verify(x => x.GetOpeningClassListAsync(), Times.Once);
            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async void ListOpeningClassAsync_ReturnNotFound_WhenListEmpty()
        {
            //Arrange
            _classServiceMock.Setup(x => x.GetOpeningClassListAsync()).Throws(new Exception());

            //Act
            var result = await _classController.ListOpeningClassAsync() as BadRequestObjectResult;

            //Assert
            _classServiceMock.Verify(x => x.GetOpeningClassListAsync(), Times.Once);
            result.Should().BeOfType<BadRequestObjectResult>();
        }
        #endregion

        #region UpdateClass
        [Fact]
        public async void UpdateClass_ExistID_ReturnOk()
        {
            //arange
            var tpModel = _fixture.Build<UpdateClassViewModel>().Create();
            _classServiceMock.Setup(x => x.UpdateClassAsync(It.IsAny<int>(), It.IsAny<UpdateClassViewModel>())).ReturnsAsync(true);

            //act
            var result = await _classController.UpdateClass(1, tpModel) as OkObjectResult;

            //assert
            _classServiceMock.Verify(x => x.UpdateClassAsync(It.IsAny<int>(), It.IsAny<UpdateClassViewModel>()), Times.Once);
            result.Should().BeOfType<OkObjectResult>();
            result.Should().NotBeNull();
        }

        [Fact]
        public async void UpdateClass_NotExistID_ReturnNull()
        {
            //arange
            int id = 0;
            _classServiceMock.Setup(x => x.UpdateClassAsync(id, It.IsAny<UpdateClassViewModel>())).ThrowsAsync(new Exception());

            //act
            var result = await _classController.UpdateClass(id, It.IsAny<UpdateClassViewModel>()) as NotFoundObjectResult;

            //assert
            _classServiceMock.Verify(x => x.UpdateClassAsync(id, It.IsAny<UpdateClassViewModel>()), Times.Once);
            result.Should().BeOfType<NotFoundObjectResult>();
        }
        #endregion

        #region CreateClass
        [Fact]
        public async void CreateClass_RetrunOk()
        {
            //Arrange
            var mocks = _fixture.Build<CreateClassViewModel>().Create();
            _classServiceMock.Setup(x => x.CreateClassAsync(It.IsAny<CreateClassViewModel>())).ReturnsAsync("Create Class Succeed");

            //Act
            var result = await _classController.CreateClass(mocks) as OkObjectResult;

            //Assert
            _classServiceMock.Verify(x => x.CreateClassAsync(It.IsAny<CreateClassViewModel>()), Times.Once);
            result.Should().BeOfType<OkObjectResult>();

        }

        [Fact]
        public async void CreateClass_ReturnBadRequest()
        {
            //Arrange
            var mock = _fixture.Build<CreateClassViewModel>().Create();
            _classServiceMock.Setup(x => x.CreateClassAsync(It.IsAny<CreateClassViewModel>())).ThrowsAsync(new Exception());
            //Act
            var result = await _classController.CreateClass(mock) as BadRequestObjectResult;
            //Assert
            _classServiceMock.Verify(x => x.CreateClassAsync(It.IsAny<CreateClassViewModel>()), Times.Once);
            result.Should().BeOfType<BadRequestObjectResult>();
        }
        #endregion

        #region AddUserToClass
        [Fact]
        public async void AddUserToClass_ReturnOk()
        {
            //Arrange
            var mock = _fixture.Build<AddUserToClassViewModel>().Create();
            _classServiceMock.Setup(x => x.AddUserToClassAsync(It.IsAny<AddUserToClassViewModel>()));
            //Act
            var result = await _classController.AddUserToClass(mock) as OkObjectResult;
            //Assert
            _classServiceMock.Verify(x => x.AddUserToClassAsync(It.IsAny<AddUserToClassViewModel>()));
            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async void AddUserToClass_ReturnBadRequest()
        {
            //Arrange
            var mock = _fixture.Build<AddUserToClassViewModel>().Create();
            _classServiceMock.Setup(x => x.AddUserToClassAsync(It.IsAny<AddUserToClassViewModel>())).ThrowsAsync(new Exception("Training Program is not avaiable"));
            //Act
            var result = await _classController.AddUserToClass(mock) as BadRequestObjectResult;
            //Assert
            _classServiceMock.Verify(x => x.AddUserToClassAsync(It.IsAny<AddUserToClassViewModel>()), Times.Once);
            result.Should().BeOfType<BadRequestObjectResult>();
        }
        #endregion

        #region Clone Class
        [Fact]
        public async void CloneClass_ReturnOk()
        {
            //Arrange
            _classServiceMock.Setup(x => x.CloneClassAsync(It.IsAny<int>()));
            //Act
            var result = await _classController.CloneClassAsync(1) as OkObjectResult;
            //Assert
            _classServiceMock.Verify(x => x.CloneClassAsync(It.IsAny<int>()), Times.Once);
            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async void CloneClass_ReturnBadRequest()
        {
            //Arrange
            _classServiceMock.Setup(x => x.CloneClassAsync(It.IsAny<int>())).ThrowsAsync(new Exception());
            //Act
            var result = await _classController.CloneClassAsync(1) as BadRequestObjectResult;
            //Assert
            _classServiceMock.Verify(x => x.CloneClassAsync(It.IsAny<int>()), Times.Once);
            result.Should().BeOfType<BadRequestObjectResult>();
        }
        #endregion

        #region Get Class Details
        [Fact]
        public async void GetClassDetails_RetunOk()
        {
            //Arrange
            var mock = _fixture.Build<ClassDetailViewModels>().Create();
            _classServiceMock.Setup(x => x.GetClassDetail(It.IsAny<int>())).ReturnsAsync(mock);
            //Act
            var result = await _classController.GetClassDetails(1) as OkObjectResult;
            //Assert
            _classServiceMock.Verify(x => x.GetClassDetail(It.IsAny<int>()), Times.Once);
            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async void GetClassDetails_RetunBadRequest_WhenIdEqual0()
        {
            //Arrange
            int id = 0;
            _classServiceMock.Setup(x => x.GetClassDetail(id));
            //Act
            var result = await _classController.GetClassDetails(id) as BadRequestObjectResult;
            //Assert
            _classServiceMock.Verify(x => x.GetClassDetail(id), Times.Never);
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async void GetClassDetails_ReturnBadRequest_WhenNotFound()
        {
            //Arrange
            _classServiceMock.Setup(x => x.GetClassDetail(It.IsAny<int>())).ThrowsAsync(new InvalidOperationException());
            //Act
            var result = await _classController.GetClassDetails(2) as BadRequestObjectResult;
            //Assert
            _classServiceMock.Verify(x => x.GetClassDetail(It.IsAny<int>()), Times.Once);
            result.Should().BeOfType<BadRequestObjectResult>();
        }
        #endregion
    }
}
