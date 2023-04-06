using Application;
using Application.Interfaces;
using Application.IValidators;
using Application.Repositories;
using Application.ViewModels.AddLectureViewModels;
using Application.ViewModels.AttendanceViewModels;
using Application.ViewModels.UnitViewModels;
using Application.ViewModels.UserViewModels;
using AutoFixture;
using AutoMapper;
using Domain.Models;
using FluentValidation;
using Infracstructures;
using Infracstructures.Mappers;
using Infracstructures.Validators;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;

namespace Domain.Tests
{
    public class SetupTest : IDisposable
    {
        // Setup interfaces, repositories, services,... for testing
        protected readonly IMapper _mapperConfig;

        // using fixture for generating test data
        protected readonly Fixture _fixture;

        protected readonly AppDbContext _dbContext;

        //IConfiguration
        protected readonly IConfiguration _configuration;

        protected readonly Mock<IUnitOfWork> _unitOfWorkMock;

        protected readonly Mock<IAttendanceService> _attendanceServiceMock;
        protected readonly Mock<IAuditService> _auditServiceMock;
        protected readonly Mock<IClaimsService> _claimsServiceMock;
        protected readonly Mock<IClassService> _classServiceMock;
        protected readonly Mock<ICurrentTime> _currentTimeMock;
        protected readonly Mock<IGradeReportService> _gradeReportServiceMock;
        protected readonly Mock<ISyllabusService> _syllabusServiceMock;
        protected readonly Mock<ITMSService> _tmsServiceMock;
        protected readonly Mock<ITrainingProgramService> _trainingProgramServiceMock;
        protected readonly Mock<IUserService> _userServiceMock;
        protected readonly Mock<IEmailService> _emailServiceMock;
        protected readonly Mock<IUnitService> _unitServiceMock;
        protected readonly Mock<ISessionServices> _sessionServicesMock;

        protected readonly Mock<IAttendancesRepository> _attendanceRepositoryMock;
        protected readonly Mock<IAuditResultRepository> _auditDetailsRepositoryMock;
        protected readonly Mock<IAuditPlanRepository> _auditPlansRepositoryMock;
        protected readonly Mock<IClassRepository> _classRepositoryMock;
        protected readonly Mock<IClassUserRepository> _classUserRepositoryMock;
        protected readonly Mock<IFeedbackFormsRepository> _feedbackFormsRepositoryMock;
        protected readonly Mock<IGradeReportsRepository> _gradeReportsRepositoryMock;
        protected readonly Mock<ILecturesRepository> _lectureRepositoryMock;
        protected readonly Mock<IOutputStandardRepository> _outputStandardRepositoryMock;
        protected readonly Mock<ISyllabusRepository> _syllabusRepositoryMock;
        protected readonly Mock<ITimeMngSystemsRepository> _timeMngSystemsRepositoryMock;
        protected readonly Mock<ITrainingMaterialRepository> _trainingMaterialRepositoryMock;
        protected readonly Mock<ITrainingProgramRepository> _trainingProgramRepositoryMock;
        protected readonly Mock<IUnitRepository> _unitRepositoryMock;
        protected readonly Mock<IUserRepository> _userRepositoryMock;

        //validator
        protected readonly Mock<IUserValidator> _userValidator;
        protected readonly Mock<IAttendanceValidator> _attendanceValidator;
        protected readonly Mock<IValidator<TakeAttendanceModel>> _attendanceAddValidator;
        protected readonly Mock<ILectureValidator> _lectureValidator;
        protected readonly Mock<IValidator<UserCreateModel>> _userCreateValidator;
        protected readonly Mock<IValidator<UnitAddViewModel>> _unitCreateValidator;

        public SetupTest()
        {
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MapperConfigs());
            });
            _mapperConfig = mappingConfig.CreateMapper();

            _fixture = new Fixture();

            //IConfiguration
            var config = new Dictionary<string, string>
            {
                { "Jwt:Key", "TqofsXAj1s5jHz83cSh9" },
                { "Jwt:Issuer", "FATMSAuthenticator" },
                { "Jwt:Audience", "FATMSPostmanClient" },
                { "Jwt:Subject", "FATMSServiceAccessToken" }
            };

            _configuration = new ConfigurationBuilder().AddInMemoryCollection(config!).Build();

            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
            .ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            _fixture.Customize<Lecture>(c => c
  .With(x => x.Duration, () => new Random().Next(1, 61)));
            // Using inmemory db for testing
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            _dbContext = new AppDbContext(options);

            _unitOfWorkMock = new Mock<IUnitOfWork>();

            _attendanceServiceMock = new Mock<IAttendanceService>();
            _auditServiceMock = new Mock<IAuditService>();
            _claimsServiceMock = new Mock<IClaimsService>();
            _classServiceMock = new Mock<IClassService>();
            _currentTimeMock = new Mock<ICurrentTime>();
            _gradeReportServiceMock = new Mock<IGradeReportService>();
            _syllabusServiceMock = new Mock<ISyllabusService>();
            _tmsServiceMock = new Mock<ITMSService>();
            _trainingProgramServiceMock = new Mock<ITrainingProgramService>();
            _unitServiceMock = new Mock<IUnitService>();
            _userServiceMock = new Mock<IUserService>();
            _emailServiceMock = new Mock<IEmailService>();
            _sessionServicesMock = new Mock<ISessionServices>();

            _attendanceRepositoryMock = new Mock<IAttendancesRepository>();
            _auditDetailsRepositoryMock = new Mock<IAuditResultRepository>();
            _auditPlansRepositoryMock = new Mock<IAuditPlanRepository>();
            _classRepositoryMock = new Mock<IClassRepository>();
            _classUserRepositoryMock = new Mock<IClassUserRepository>();
            _feedbackFormsRepositoryMock = new Mock<IFeedbackFormsRepository>();
            _gradeReportsRepositoryMock = new Mock<IGradeReportsRepository>();
            _lectureRepositoryMock = new Mock<ILecturesRepository>();
            _outputStandardRepositoryMock = new Mock<IOutputStandardRepository>();
            _syllabusRepositoryMock = new Mock<ISyllabusRepository>();
            _timeMngSystemsRepositoryMock = new Mock<ITimeMngSystemsRepository>();
            _trainingMaterialRepositoryMock = new Mock<ITrainingMaterialRepository>();
            _trainingProgramRepositoryMock = new Mock<ITrainingProgramRepository>();
            _unitRepositoryMock = new Mock<IUnitRepository>();
            _userRepositoryMock = new Mock<IUserRepository>();

            // setup current time service mock to return current time value
            _currentTimeMock.Setup(x => x.GetCurrentTime()).Returns(DateTime.Now);

            // setup claims service mock to return user id with value 0
            _claimsServiceMock.Setup(x => x.GetCurrentUserId).Returns(0);

            //validator
            _userValidator = new Mock<IUserValidator>();
            _attendanceValidator = new Mock<IAttendanceValidator>();
            _attendanceAddValidator = new Mock<IValidator<TakeAttendanceModel>>();
            _lectureValidator = new Mock<ILectureValidator>();
            _userCreateValidator = new Mock<IValidator<UserCreateModel>>();
            _unitCreateValidator = new Mock<IValidator<UnitAddViewModel>>();
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }
    }
}
