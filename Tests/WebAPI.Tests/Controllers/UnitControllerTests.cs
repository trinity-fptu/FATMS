using Xunit;
using WebAPI.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Tests;
using WebAPI.Tests.Controllers;
using Application.Commons;
using Application.ViewModels.UserViewModels;
using AutoFixture;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Application.ViewModels.UnitViewModels;
using FluentAssertions;
using Application.Services;
using Domain.Models;
using Domain.Models.Syllabuses;
using FluentValidation.Results;

namespace WebAPI.Controllers.Tests
{
    public class UnitControllerTests : SetupTest
    {
        private readonly UnitController _unitController;

        public UnitControllerTests()
        {
            _unitController = new UnitController(_unitServiceMock.Object);
        }

        #region Create
        //// [Fact()]
        //public async void Create_ValidParameter_ReturnOkResult()
        //{
        //    // Arrange
        //    var mockedUnit = _fixture.Build<UnitAddViewModel>().Create();
        //    var validationResult = new ValidationResult();

        //    _unitServiceMock.Setup(x => x.AddAsync(mockedUnit)).Returns(Task.CompletedTask);
        //    _unitServiceMock.Setup(x => x.ValidateAddAsync(mockedUnit)).ReturnsAsync(validationResult);

        //    var syllabus = new Syllabus();

        //    var unitEntity = _mapperConfig.Map<Unit>(mockedUnit);
        //    if (unitEntity.Syllabuses == null) unitEntity.Syllabuses = new List<Syllabus>();
        //    unitEntity.Syllabuses.Add(syllabus);

        //    // Act
        //    var result = await _unitController.Create(mockedUnit) as OkResult;

        //    // Assert
        //    _unitServiceMock.Verify(
        //        x => x.AddAsync(mockedUnit),
        //        Times.Once);
        //    _unitServiceMock.Verify(
        //        x => x.ValidateAddAsync(mockedUnit),
        //        Times.Once);            
        //    result.Should().BeOfType<OkResult>();
        //}

        //[Fact()]
        //public async void Create_InvalidValidation_ReturnBadRequestResult()
        //{
        //    // Arrange
        //    var mockedUnit = _fixture.Build<UnitAddViewModel>().Create();
        //    var mockValidationResults = new ValidationResult()
        //    {
        //        Errors = new List<ValidationFailure>() { new ValidationFailure("Session", "Syllabus Session must greater than 0") }
        //    };

        //    _unitServiceMock.Setup(x => x.AddAsync(mockedUnit)).Returns(Task.CompletedTask);
        //    _unitServiceMock.Setup(x => x.ValidateAddAsync(mockedUnit)).ReturnsAsync(mockValidationResults);

        //    var syllabus = new Syllabus();

        //    var unitEntity = _mapperConfig.Map<Unit>(mockedUnit);
        //    if (unitEntity.Syllabuses == null) unitEntity.Syllabuses = new List<Syllabus>();
        //    unitEntity.Syllabuses.Add(syllabus);

        //    // Act
        //    var result = await _unitController.Create(mockedUnit);

        //    // Assert
        //    _unitServiceMock.Verify(
        //        x => x.AddAsync(mockedUnit),
        //        Times.Never);
        //    _unitServiceMock.Verify(
        //        x => x.ValidateAddAsync(mockedUnit),
        //        Times.Once);
        //    result.Should().BeOfType<BadRequestObjectResult>();
        //}

        //[Fact()]
        //public async void Create_CreateUnsucceed_ReturnBadRequestResult()
        //{
        //    // Arrange
        //    var mockedUnit = _fixture.Build<UnitAddViewModel>().Create();
        //    var mockValidationResults = new ValidationResult()
        //    {
        //        Errors = new List<ValidationFailure>() { new ValidationFailure("Session", "Syllabus Session must greater than 0") }
        //    };

        //    _unitServiceMock.Setup(x => x.AddAsync(mockedUnit)).Throws(() => new Exception("Syllabus not found"));
        //    _unitServiceMock.Setup(x => x.ValidateAddAsync(mockedUnit)).ReturnsAsync(mockValidationResults);

        //    var syllabus = new Syllabus();

        //    var unitEntity = _mapperConfig.Map<Unit>(mockedUnit);
        //    if (unitEntity.Syllabuses == null) unitEntity.Syllabuses = new List<Syllabus>();
        //    unitEntity.Syllabuses.Add(syllabus);

        //    // Act
        //    var result = await _unitController.Create(mockedUnit);

        //    // Assert
        //    _unitServiceMock.Verify(
        //        x => x.AddAsync(mockedUnit),
        //        Times.Never);
        //    _unitServiceMock.Verify(
        //        x => x.ValidateAddAsync(mockedUnit),
        //        Times.Once);
        //    result.Should().BeOfType<BadRequestObjectResult>();
        //}
        #endregion
    }
}