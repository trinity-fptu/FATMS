using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infracstructures.FluentAPIs
{
    public class AuditDetailConfiguration : IEntityTypeConfiguration<AuditDetail>
    {
        public void Configure(EntityTypeBuilder<AuditDetail> builder)
        {
            builder.ToTable("AuditDetail");

            //Id
            builder.HasKey(x => x.Id);

            //PlanId
            builder.Property(x => x.PlanId).IsRequired();

            //Feedback
            builder.Property(x => x.Feedback).HasMaxLength(500);

            //Status
            builder.Property(x => x.Status);


            //AuditPlan
            builder.HasOne(x => x.AuditPlan)
                .WithMany(x => x.AuditDetails)
                .HasForeignKey(x => x.PlanId);

            //Auditor
            builder.HasOne(x => x.Auditor)
                .WithMany(x => x.AuditedAuditDetails)
                .HasForeignKey(x => x.AuditorId);

            //Trainee
            builder.HasOne(x => x.Trainee)
                .WithMany(x => x.TakenAuditDetails)
                .HasForeignKey(x => x.TraineeId);

            //AuditResult
            builder.HasMany(x => x.Results)
                .WithOne(x => x.AuditDetail)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}