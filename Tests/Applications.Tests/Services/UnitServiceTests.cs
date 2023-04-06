using Application.Services;
using Application.ViewModels.UnitViewModels;
using Application.ViewModels.UserViewModels;
using AutoFixture;
using Domain.Models;
using Domain.Models.Syllabuses;
using Domain.Models.Users;
using Domain.Tests;
using FluentValidation.Results;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.Tests.Services
{
    public class UnitServiceTests :SetupTest
    {
        private readonly UnitService _unitService;

        public UnitServiceTests() 
        {
            _unitService = new UnitService(
                _unitOfWorkMock.Object,
                _mapperConfig,
                _claimsServiceMock.Object,
                _currentTimeMock.Object,
                _configuration);
        }

        ////[Fact]
        //public async void AddAsync_ValidData_ReturnVoid()
        //{
        //    // Assign
        //    var mocksUnitViewModel = _fixture.Build<UnitAddViewModel>().Create();
        //    var mocksUnit = _mapperConfig.Map<Unit>(mocksUnitViewModel);

        //    var mocksSyllabus = _fixture.Build<Syllabus>().Create();
        //    _unitOfWorkMock.Setup(x => x.SyllabusRepo.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(mocksSyllabus);

        //    if (mocksUnit.Syllabuses == null) mocksUnit.Syllabuses = new List<Syllabus>();
        //    mocksUnit.Syllabuses.Add(mocksSyllabus);

        //    _unitOfWorkMock.Setup(x => x.UnitRepo.AddAsync(It.IsAny<Unit>())).Returns(Task.CompletedTask);
        //    _unitOfWorkMock.Setup(x => x.SaveChangesAsync()).ReturnsAsync(1);

        //    // Act
        //    var result = _unitService.AddAsync(mocksUnitViewModel);

        //    // Assert
        //    _unitOfWorkMock.Verify(x => x.UnitRepo.AddAsync(It.IsAny<Unit>()),Times.Once());
        //    _unitOfWorkMock.Verify(x => x.SyllabusRepo
        //    .GetByIdAsync(It.IsAny<int>()), Times.Once());
        //    _unitOfWorkMock.Verify(x => x.SaveChangesAsync(), Times.Once);
        //}

        //[Fact]
        //public async void AddAsync_SyllabusNotFound_ThrowsException()
        //{
        //    // Assign
        //    var mockUnitViewModel = _fixture.Build<UnitAddViewModel>().Create();
        //    var mockUnit = _mapperConfig.Map<Unit>(mockUnitViewModel);

        //    _unitOfWorkMock.Setup(x => x.SyllabusRepo.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Syllabus)null);

        //    _unitOfWorkMock.Setup(x => x.SaveChangesAsync()).ReturnsAsync(1);

        //    // Act
        //    var result = _unitService.AddAsync(mockUnitViewModel);

        //    // Assert
        //    _unitOfWorkMock.Verify(x => x.SyllabusRepo.GetByIdAsync(It.IsAny<int>()), 
        //        Times.Once());
        //    var exception = await Assert.ThrowsAsync<Exception>(() => _unitService.AddAsync(mockUnitViewModel));
        //    Assert.Equal("Syllabus not found", exception.Message);
        //}

        
    }
}
