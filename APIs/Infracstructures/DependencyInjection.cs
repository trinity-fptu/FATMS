using Application;
using Application.Interfaces;
using Application.IValidators;
using Application.Repositories;
using Application.Services;
using Azure.Storage.Blobs;
using Infracstructures.Mappers;
using Infracstructures.Repositories;
using Infracstructures.Validators;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infracstructures
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfractstructure(this IServiceCollection services, IConfiguration config)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            #region Config Repository and Service
            //User
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IUserService, UserService>();

            //User Validator
            services.AddTransient<IUserValidator, UserValidator>();

            //Attendance
            services.AddTransient<IAttendancesRepository, AttendancesRepository>();
            services.AddTransient<IAttendanceService, AttendanceService>();

            //Audit
            services.AddTransient<IAuditService, AuditService>();

            //Audit Result
            services.AddTransient<IAuditResultRepository, AuditResultRepository>();

            //Audit Plan
            services.AddTransient<IAuditPlanRepository, AuditPlanRepository>();

            //Audit Question
            services.AddTransient<IAuditDetailRepository, AuditDetailRepository>();

            //Class
            services.AddTransient<IClassRepository, ClassRepository>();
            services.AddTransient<IClassService, ClassService>();

            //ClassUser
            services.AddTransient<IClassUserRepository, ClassUserRepository>();

            //ClassUnit
            services.AddTransient<IClassUnitDetailRepository, ClassUnitDetailRepository>();
            services.AddTransient<IClassUnitDetailService, ClassUnitDetailService>();

            //FeedbackForms
            services.AddTransient<IFeedbackFormsRepository, FeedbackFormsRepository>();

            //GradeReports
            services.AddTransient<IGradeReportsRepository, GradeReportsRepository>();
            services.AddTransient<IGradeReportService, GradeReportSevice>();

            //Lectures
            services.AddTransient<ILecturesRepository, LectureRepository>();

            //OutputStandard
            services.AddTransient<IOutputStandardRepository, OutputStandardRepository>();

            //Syllabus
            services.AddTransient<ISyllabusRepository, SyllabusRepository>();
            services.AddTransient<ISyllabusService, SyllabusService>();

            //TMS
            services.AddTransient<ITimeMngSystemsRepository, TimeMngSystemsRepository>();
            services.AddTransient<ITMSService, TMSService>();

            //TrainingMaterial
            services.AddTransient<ITrainingMaterialRepository, TrainingMaterialRepository>();
            services.AddTransient<ITrainingMaterialService, TrainingMaterialService>();

            //TrainingProgram
            services.AddTransient<ITrainingProgramRepository, TrainingProgramRepository>();
            services.AddTransient<ITrainingProgramService, TrainingProgramService>();

            //Unit
            services.AddScoped<IUnitRepository, UnitRepository>();
            services.AddScoped<IUnitService, UnitService>();

            //Role
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IRoleService, RoleService>();
            #endregion

            #region Config validators
            //UserValidator
            services.AddTransient<IUserValidator, UserValidator>();

            //RoleValidator
            services.AddTransient<IRoleValidator, RoleValidator>();

            //AttendanceValidator
            services.AddTransient<IAttendanceValidator, AttendanceValidator>();

            //LectureValidator 
            services.AddTransient<ILectureValidator, LectureValidator>();
            #endregion


            services.AddSingleton<ICurrentTime, CurrentTime>();

            // Use local DB
            services.AddDbContext<AppDbContext>(opt => opt.UseSqlServer(config.GetConnectionString("FA.TMS.Db")));

            // Use Azure DB
            // services.AddDbContext<AppDbContext>(opt => opt.UseSqlServer(config.GetConnectionString("FATMS.AzureDB")));

            // Use Azure storage 
            services.AddScoped(_ =>
            {
                return new BlobServiceClient(config.GetConnectionString("AzureBlobStorage"));
            });
            services.AddAutoMapper(typeof(MapperConfigs).Assembly);

            
            return services;
        }
    }
}
