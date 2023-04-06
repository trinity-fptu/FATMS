using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infracstructures.FluentAPIs
{
    public class AuditPlanConfiguration : IEntityTypeConfiguration<AuditPlan>
    {
        public void Configure(EntityTypeBuilder<AuditPlan> builder)
        {
            builder.ToTable("AuditPlan");

            //Id
            builder.HasKey(x => x.Id);

            //AuditDate
            builder.Property(x => x.AuditDate).IsRequired();

            //Location
            builder.Property(x => x.Location).IsRequired();

            //Syllabus
            builder.HasOne(x => x.Syllabus)
                .WithMany(x => x.AuditPlans)
                .HasForeignKey(x => x.SyllabusId);

            //Class
            builder.HasOne(x => x.Class)
                .WithMany(x => x.AuditPlans)
                .HasForeignKey(x => x.ClassId);

            //User
            builder.HasOne(x => x.CreatedUser)
                .WithMany(x => x.CreatedAuditPlans)
                .HasForeignKey(x => x.PlannedBy);

            //AuditDetails
            builder.HasMany(x => x.AuditDetails)
                .WithOne(x => x.AuditPlan)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}