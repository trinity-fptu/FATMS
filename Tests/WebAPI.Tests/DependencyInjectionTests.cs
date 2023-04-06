using Application.Interfaces;
using Application.Repositories;
using Application.Services;
using FluentAssertions;
using Infracstructures;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WebAPI.Services;

namespace WebAPI.Tests
{
    public class DependencyInjectionTests
    {
        private readonly ServiceProvider _serviceProvider;

        private IConfiguration GetConfiguration()
        {
            var config = new Dictionary<string, string>();
            return new ConfigurationBuilder().AddInMemoryCollection(config!).Build();
        }

        public DependencyInjectionTests()
        {
            var service = new ServiceCollection();
            //service.AddWebAPIService();
            service.AddInfractstructure(GetConfiguration());
            service.AddDbContext<AppDbContext>(
                option => option.UseInMemoryDatabase("test"));
            _serviceProvider = service.BuildServiceProvider();
        }

        //[Fact]
        public void DependencyInjectionTests_ServiceShouldResolveCorrectly()
        {
            var currentTimeServiceResolved = _serviceProvider.GetRequiredService<ICurrentTime>();
            var claimsServiceServiceResolved = _serviceProvider.GetRequiredService<IClaimsService>();

            IConfiguration configuration = GetConfiguration();

            //User
            var userServiceResolved = _serviceProvider.GetRequiredService<IUserService>();

            //Attendance
            var attendanceServiceResolved = _serviceProvider.GetRequiredService<IAttendanceService>();

            //Audit
            var auditServiceResolved = _serviceProvider.GetRequiredService<IAuditService>();

            //Class
            var classServiceResolved = _serviceProvider.GetRequiredService<IClassService>();

            //GradeReports
            var gradeReportsServicesResolved = _serviceProvider.GetRequiredService<IGradeReportService>();

            //Syllabus
            var syllabusServiceResolved = _serviceProvider.GetRequiredService<ISyllabusService>();

            //TMS
            var tmsServiceResolved = _serviceProvider.GetRequiredService<ITMSService>();

            var trainingProgramServiceResolved = _serviceProvider.GetRequiredService<ITrainingProgramService>();

            //Unit
            var unitRepositoryService = _serviceProvider.GetRequiredService<IUnitRepository>();

            currentTimeServiceResolved.GetType().Should().Be(typeof(CurrentTime));
            claimsServiceServiceResolved.GetType().Should().Be(typeof(ClaimsService));
            userServiceResolved.GetType().Should().Be(typeof(UserService));

            attendanceServiceResolved.GetType().Should().Be(typeof(Application.Services.AttendanceService));

            //Audit
            Assert.Equal(typeof(AuditService), auditServiceResolved.GetType());

            //Class
            Assert.Equal(typeof(ClassService), classServiceResolved.GetType());

            //GradeReports
            Assert.Equal(typeof(GradeReportSevice), gradeReportsServicesResolved.GetType());

            //Syllabus
            Assert.Equal(typeof(SyllabusService), syllabusServiceResolved.GetType());

            //TMS
            Assert.Equal(typeof(TMSService), tmsServiceResolved.GetType());

            //Training Program
            Assert.Equal(typeof(TrainingProgramService), trainingProgramServiceResolved.GetType());
        }
    }
}
