using Application.Interfaces;
using Application.Services;
using Application.ViewModels.AttendanceViewModels;
using Application.ViewModels.TrainingProgramViewModels;
using AutoFixture;
using Domain.Models;
using Domain.Models.Syllabuses;
using Domain.Tests;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;

namespace Application.Tests.Services;

public class AttendanceServiceTests : SetupTest
{
    private readonly IAttendanceService _attendanceService;

    public AttendanceServiceTests()
    {
        _attendanceService = new AttendanceService(_unitOfWorkMock.Object, _mapperConfig, _claimsServiceMock.Object, _currentTimeMock.Object, _configuration, _attendanceValidator.Object);
    }

    //[Fact]
    public async Task GetAllAttendanceAsync_ReturnsAttendanceViewModelList()
    {
        // arrange
        var classId = _fixture.Create<int>();
        var classUser = _fixture.Build<ClassUsers>()
            .With(x => x.Id, () => _fixture.Create<int>())
            .Create();
        var attendances = _fixture.Build<Attendance>()
            .With(x => x.ClassUserId, () => classUser.Id)
            .CreateMany(3)
            .ToList();
        var attendanceViewModels = _mapperConfig.Map<List<AttendanceViewModel>>(attendances);

        _unitOfWorkMock.Setup(x => x.ClassUserRepo.GetClassUsers(classId))
            .ReturnsAsync(new List<ClassUsers> { classUser });
        _unitOfWorkMock.Setup(x => x.AttendancesRepo.GetAllAttendancesByClassUserId(classUser.Id))
            .ReturnsAsync(attendances);

        // act
        var result = await _attendanceService.GetAllAttendanceAsync(classId);

        // assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(attendanceViewModels);
    }

    // [Fact]
    // public async Task GetAllAttendanceAsync_IsNullAttendance_ReturnNotFound()
    // {
    //     // arrange
    //     var classId = _fixture.Create<int>();
    //     var classUser = _fixture.Build<ClassUsers>()
    //         .With(x => x.Id, () => _fixture.Create<int>())
    //         .Create();
    //
    //     _unitOfWorkMock.Setup(x => x.ClassUserRepo.GetClassUsers(classId))
    //         .ReturnsAsync(new List<ClassUsers> { classUser });
    //     _unitOfWorkMock.Setup(x => x.AttendancesRepo.GetAllAttendancesByClassUserId(classUser.Id))
    //         .ReturnsAsync((List<Attendance>)null);
    //
    //     // act
    //     var result = await _attendanceService.GetAllAttendanceAsync(classId);
    //
    //     // assert
    //     result.Should().NotBeNull();
    //     result.Should().BeOfType<NotFoundResult>();
    // }
    [Fact]
    public async Task TakeAttendace_ShouldReturnCurrentData_WhenSavedSuccess()
    {
        //arrange
        var mock = _fixture.Build<TakeAttendanceModel>().Create();
        var mock1 = _fixture.Build<Attendance>().Without(e => e.ClassUser).Create();

        var mockAttendace = _fixture.Build<TakeAttendanceModel>().CreateMany(2).ToList();
        var list = _mapperConfig.Map<List<Attendance>>(mockAttendace);

        var mockSyllabus = _fixture.Build<ClassUsers>().Without(e => e.Class).Without(e => e.User).Create();

        _unitOfWorkMock.Setup(x => x.AttendancesRepo.GetAllAsync()).ReturnsAsync(list);
        _unitOfWorkMock.Setup(x => x.AttendancesRepo.GetAttendancesByClassUserId(It.IsAny<int>())).ReturnsAsync(mock1);
        _unitOfWorkMock.Setup(x => x.AttendancesRepo.GetAllAttendancesByClassUserIdDesc(It.IsAny<int>())).ReturnsAsync(list);
        _unitOfWorkMock.Setup(x => x.AttendancesRepo.AddAsync(It.IsAny<Attendance>())).Returns(Task.CompletedTask);
        _unitOfWorkMock.Setup(x => x.SaveChangesAsync()).ReturnsAsync(1);

        //act
        var result = await _attendanceService.TakeAttendance(mock);

        //assert
        _unitOfWorkMock.Verify(x => x.AttendancesRepo.AddAsync(It.IsAny<Attendance>()), Times.Once());
        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(), Times.Once());
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(AttendanceViewModel));
    }
    [Fact]
    public async Task TakeAttendace_ShouldReturnNull_WhenSavedFailed()
    {
        //arrange
        var mock = _fixture.Build<TakeAttendanceModel>().Create();
        var mock1 = _fixture.Build<Attendance>().Without(e => e.ClassUser).Create();

        var mockAttendace = _fixture.Build<TakeAttendanceModel>().CreateMany(2).ToList();
        var list = _mapperConfig.Map<List<Attendance>>(mockAttendace);
        var mockSyllabus = _fixture.Build<ClassUsers>().Without(e => e.Class).Without(e => e.User).Create();

        _unitOfWorkMock.Setup(x => x.AttendancesRepo.GetAllAsync()).ReturnsAsync(list);
        _unitOfWorkMock.Setup(x => x.AttendancesRepo.GetAttendancesByClassUserId(It.IsAny<int>())).ReturnsAsync(mock1);
        _unitOfWorkMock.Setup(x => x.AttendancesRepo.GetAllAttendancesByClassUserIdDesc(It.IsAny<int>())).ReturnsAsync(list);
        _unitOfWorkMock.Setup(x => x.AttendancesRepo.AddAsync(It.IsAny<Attendance>())).Returns(Task.CompletedTask);
        _unitOfWorkMock.Setup(x => x.SaveChangesAsync()).ReturnsAsync(0);

        //act
        var result = await _attendanceService.TakeAttendance(mock);

        //assert
        _unitOfWorkMock.Verify(x => x.AttendancesRepo.AddAsync(It.IsAny<Attendance>()), Times.Once());
        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(), Times.Once());
        result.Should().BeNull();
    }
    [Fact]
    public async Task TakeAttendace_ShouldReturnException_WhenAlreadyTakeAtendance()
    {
        //arrange
        var mock = _fixture.Build<TakeAttendanceModel>().Create();
        var mock1 = _fixture.Build<Attendance>().Without(e => e.ClassUser).Create();
        var mockAttendace = _fixture.Build<TakeAttendanceModel>().CreateMany(2).ToList();
        var list = _mapperConfig.Map<List<Attendance>>(mockAttendace);
        list[0].ClassUserId = mock.ClassUserID;
        list[0].Day = _currentTimeMock.Object.GetCurrentTime();
        _unitOfWorkMock.Setup(x => x.AttendancesRepo.GetAllAsync()).ReturnsAsync(list);
        _unitOfWorkMock.Setup(x => x.AttendancesRepo.GetAttendancesByClassUserId(It.IsAny<int>())).ReturnsAsync(mock1);
        _unitOfWorkMock.Setup(x => x.AttendancesRepo.GetAllAttendancesByClassUserIdDesc(It.IsAny<int>())).ReturnsAsync(list);
        //act
        var result = async () => await _attendanceService.TakeAttendance(mock);

        //assert
        await result.Should().ThrowAsync<Exception>().WithMessage("This Class userId attendance have been already taken today!!");
    }



}
