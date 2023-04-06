using Domain.Models;
using Domain.Models.Syllabuses;
using Domain.Models.Users;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Infracstructures
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Attendance> Attendances { get; set; }
        public DbSet<AuditDetail> AuditDetails { get; set; }
        public DbSet<AuditPlan> AuditPlans { get; set; }
        public DbSet<AuditResult> AuditResults { get; set; }
        public DbSet<Class> Classes { get; set; }
        public DbSet<ClassUsers> ClassUsers { get; set; }
        public DbSet<FeedbackForm> FeedbackForms { get; set; }
        public DbSet<GradeReport> GradeReports { get; set; }
        public DbSet<Lecture> Lectures { get; set; }
        public DbSet<OutputStandard> OutputStandards { get; set; }
        public DbSet<TMS> TimeMngSystems { get; set; }
        public DbSet<TrainingMaterial> TrainingMaterials { get; set; }
        public DbSet<TrainingProgram> TrainingPrograms { get; set; }
        public DbSet<Unit> Units { get; set; }
        public DbSet<Syllabus> Syllabus { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<ClassUnitDetail> ClassUnitDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging();
        }
    }
}