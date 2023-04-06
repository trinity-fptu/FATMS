using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infracstructures.FluentAPIs
{
    public class AuditResultConfiguration : IEntityTypeConfiguration<AuditResult>
    {
        public void Configure(EntityTypeBuilder<AuditResult> builder)
        {
            builder.ToTable("AuditResult")
                .ToTable(x => x
                .HasTrigger("trgAuditResultInsert"));

            //Id
            builder.HasKey(x => x.Id);

            //Question
            builder.Property(x => x.Question).IsRequired();

            //Answer
            builder.Property(x => x.TraineeAnswer).IsRequired();

            //Rating
            builder.Property(x=> x.Rating).IsRequired();

            //AuditDetail
            builder.HasOne(x => x.AuditDetail)
                .WithMany(x => x.Results)
                .HasForeignKey(x => x.AuditDetailId);


        }
    }
}