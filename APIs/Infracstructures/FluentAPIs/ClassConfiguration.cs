using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infracstructures.FluentAPIs
{
    public class ClassConfiguration : IEntityTypeConfiguration<Class>
    {
        public void Configure(EntityTypeBuilder<Class> builder)
        {
            builder.ToTable("Class");

            //Id
            builder.HasKey(x => x.Id);

            //Code
            builder.Property(x => x.Code).IsRequired();

            //StartedOn
            builder.Property(x => x.StartedOn).IsRequired();

            //FinishedOn
            builder.Property(x => x.FinishedOn).IsRequired();

            //CreatedBy
            builder.Property(x => x.CreatedBy).IsRequired();

            //ApprovedBy
            builder.Property(x => x.ApprovedBy).IsRequired();

            //CreatedBy
            builder.Property(x => x.CreatedBy).IsRequired(false);

            //ApprovedBy
            builder.Property(x => x.ApprovedBy).IsRequired(false);

            //ClassUser
            builder.HasMany(x => x.ClassUsers)
                .WithOne(x => x.Class)
                .OnDelete(DeleteBehavior.Cascade);

            //Created
            builder.HasOne(x => x.CreatedAdmin)
                .WithMany(x => x.CreatedClass)
                .HasForeignKey(x => x.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull);

            //Approved
            builder.HasOne(x => x.ApprovedAdmin)
                .WithMany(x => x.ApprovedClass)
                .HasForeignKey(x => x.ApprovedBy)
                .OnDelete(DeleteBehavior.ClientSetNull);

            // Training program
            builder.HasOne(x => x.TrainingProgram)
                .WithMany(x => x.Classes)
                .HasForeignKey(x => x.TrainingProgramId);

            //Audit
            builder.HasMany(x => x.AuditPlans)
                .WithOne(x => x.Class)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}