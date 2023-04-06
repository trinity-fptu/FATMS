using Domain.Models.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infracstructures.FluentAPIs
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");

            //Id
            builder.HasKey(u => u.Id);

            //FullName
            builder.Property(u => u.FullName).IsRequired().HasMaxLength(50);

            //Email
            builder.Property(u => u.Email).IsRequired().HasMaxLength(50);
            builder.HasIndex(x => x.Email).IsUnique();

            //Password
            builder.Property(u => u.Password).IsRequired().HasMaxLength(100);

            //Phone
            builder.Property(u => u.Phone).HasMaxLength(10);

            //Level
            builder.Property(u => u.Level).IsRequired(false);

            //Status
            builder.Property(u => u.Status).IsRequired(false);

            //TMS Trainee
            builder.HasMany(x => x.TimeMngSystemList)
                .WithOne(x => x.Trainee)
                .OnDelete(DeleteBehavior.Cascade);

            //TMS Checked
            builder.HasMany(x => x.TimeMngSystem)
                .WithOne(x => x.Admin)
                .OnDelete(DeleteBehavior.ClientSetNull);

            //TrainingProgram Created
            builder.HasMany(x => x.CreatedTrainingProgram)
                .WithOne(x => x.CreatedAdmin)
                .OnDelete(DeleteBehavior.ClientSetNull);

            //TrainingProgram Modified
            builder.HasMany(x => x.ModifyTrainingProgram)
                .WithOne(x => x.ModifiedAdmin)
                .OnDelete(DeleteBehavior.ClientSetNull);

            //Syllabys Created
            builder.HasMany(x => x.CreatedSyllabus)
                .WithOne(x => x.CreatedAdmin)
                .OnDelete(DeleteBehavior.ClientSetNull);

            //Syllabus Modified
            builder.HasMany(x => x.ModifiedSyllabus)
                .WithOne(x => x.ModifiedAdmin)
                .OnDelete(DeleteBehavior.ClientSetNull);

            //Class Created
            builder.HasMany(x => x.CreatedClass)
                .WithOne(x => x.CreatedAdmin)
                .OnDelete(DeleteBehavior.ClientSetNull);

            //Class Approved
            builder.HasMany(x => x.ApprovedClass)
                .WithOne(x => x.ApprovedAdmin)
                .OnDelete(DeleteBehavior.ClientSetNull);

            //FeedbackForms Trainee
            builder.HasMany(x => x.FeedbackTrainee)
                .WithOne(x => x.Trainee)
                .OnDelete(DeleteBehavior.Cascade);

            //FeedbackForms Trainer
            builder.HasMany(x => x.FeedbackTrainer)
                .WithOne(x => x.Trainer)
                .OnDelete(DeleteBehavior.ClientSetNull);

            //GradeReport
            builder.HasMany(x => x.GradeReports)
                .WithOne(x => x.User)
                .OnDelete(DeleteBehavior.ClientSetNull);

            //ClassUsers
            builder.HasMany(x => x.ClassUsers)
                .WithOne(x => x.User)
                .OnDelete(DeleteBehavior.Cascade);

            //AuditPlans
            builder.HasMany(x => x.CreatedAuditPlans)
                .WithOne(x => x.CreatedUser)
                .OnDelete(DeleteBehavior.ClientSetNull);

            //AuditDetails Trainee
            builder.HasMany(x => x.TakenAuditDetails)
                .WithOne(x => x.Trainee)
                .OnDelete(DeleteBehavior.ClientSetNull);

            //AuditDetails Auditor
            builder.HasMany(x => x.AuditedAuditDetails)
                .WithOne(x => x.Auditor)
                .OnDelete(DeleteBehavior.ClientSetNull);

            //TrainingMaterials
            builder.HasMany(x => x.TrainingMaterials)
                .WithOne(x => x.User)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}