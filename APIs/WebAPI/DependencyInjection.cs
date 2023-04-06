using Application.Interfaces;
using FluentValidation.AspNetCore;
using System.Text.Json.Serialization;
using WebAPI.Services;
using Application.Configuration;
using FluentValidation;
using WebAPI.Validations.UserValidations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Quartz;
using Application.Cronjob;
using Application.Services;
using Serilog;
using WebAPI.Validations.AttendanceValidations;
using WebAPI.Validations.LectureValidations;
//using Microsoft.SqlServer.Management.Smo.Agent;

namespace WebAPI
{
    public static class DependencyInjection
    {
        public static void AddWebAPIService(this IServiceCollection services, WebApplicationBuilder builder)
        {
            services.AddControllers().AddJsonOptions(opt =>
            {
                opt.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
                opt.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
            });
            services.AddControllers().AddNewtonsoftJson(o =>
            {
                o.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            });
            //Mail
            services.AddTransient<IEmailService, MailService>();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            services.AddHealthChecks();
            services.AddHttpContextAccessor();
            services.AddScoped<IClaimsService, ClaimsService>();
            //Adding Session
            services.AddDistributedMemoryCache(); //Adding cache in memory for session.
            services.AddSession(); //Adding session.
            services.AddTransient<ISessionServices, SessionServices>();
            services.AddAuthorization();
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidAudience = builder.Configuration["Jwt:Audience"],
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
                };
            });
            services.Configure<MailConfigurations>(builder.Configuration.GetSection(nameof(MailConfigurations)));
            services.AddFluentValidationClientsideAdapters();
            services.AddScoped<TMSService>();
            services.AddTransient<SendAttendanceMailJob>();
            services.AddQuartz(q =>
            {
                q.UseMicrosoftDependencyInjectionJobFactory();
                q.ScheduleJob<SendAttendanceMailJob>(job => job
                    .WithIdentity("AttendanceMailJob")
                    .WithDescription("Sends attendance report emails at 11pm every day.")
                    .WithSchedule(CronScheduleBuilder.DailyAtHourAndMinute(10, 50))
                );
            });

            services.AddQuartzHostedService(options => options.WaitForJobsToComplete = true);
            // Add CORS Policy
            services.AddCors(options =>
            {
                options.AddPolicy(name: "_myAllowSpecificOrigins",
                                  policy =>
                                  {
                                      policy.WithOrigins(
                                          "https://educationmanagementapptest.web.app",
                                          "https://educationmanagementfaapp.web.app",
                                          "https://fsoftinternmockproject.web.app")
                                      .AllowAnyHeader()
                                      .AllowAnyMethod(); 
                                  });
            });
            //Adding Serilog
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .MinimumLevel.Override("Microsoft.EntityFrameworkCore", Serilog.Events.LogEventLevel.Information)
                .MinimumLevel.Override("Microsoft.EntityFrameworkCore.Database.Command", Serilog.Events.LogEventLevel.Warning)
                .Enrich.FromLogContext()
                .Enrich.WithCorrelationId()
                .WriteTo.Console(outputTemplate: "[{Timestamp:yyyy-MM-dd HH:mm:ss} {CorrelationId} {SourceContext} {Level:u3}] {Message:lj}{NewLine}{Exception}")
                .WriteTo.File(@$"{Path.Combine(builder.Environment.ContentRootPath, "Logs", "logs.txt")}",
                rollingInterval: RollingInterval.Day,
                outputTemplate: "[{Timestamp:yyyy-MM-dd HH:mm:ss} {CorrelationId} {SourceContext} {Level:u3}] {Message:lj}{NewLine}{Exception}{NewLine}")
                .CreateLogger();

            builder.Host.UseSerilog(Log.Logger);
        }

        public static IServiceCollection AddModelValidator(this IServiceCollection services)
        {
            services.AddValidatorsFromAssemblyContaining<UserCreateModelValidator>();

            services.AddValidatorsFromAssemblyContaining<UserUpdateModelValidator>();

            services.AddValidatorsFromAssemblyContaining<AddLectureValidation>();

            services.AddValidatorsFromAssemblyContaining<TakeAttendanceValidation>();

            return services;
        }
    }
}