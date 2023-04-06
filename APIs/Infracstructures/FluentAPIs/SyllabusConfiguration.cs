using Domain.Models.Syllabuses;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infracstructures.FluentAPIs
{
    public class SyllabusConfiguration : IEntityTypeConfiguration<Syllabus>
    {
        public void Configure(EntityTypeBuilder<Syllabus> builder)
        {
            builder.ToTable("Syllabus");

            //Id
            builder.HasKey(x => x.Id);

            //Name
            builder.Property(x => x.Name).IsRequired().HasMaxLength(250);

            //Code
            builder.Property(x => x.Code).IsRequired().HasMaxLength(10);

            //isActive
            builder.Property(x => x.isActive).IsRequired();

            //isDeleted
            builder.Property(x => x.isDeleted).IsRequired();

            //CreatedBy
            builder.Property(x => x.CreatedBy).IsRequired(false);

            //Modified
            builder.Property(x => x.LastModifiedBy).IsRequired(false);

            //TrainingDeliveryPrincipleId
            builder.Property(x => x.TrainingDeliveryPrinciple)
                .IsRequired(false);

            //Units
            builder.HasMany(x => x.Units)
                .WithMany(x => x.Syllabuses);

            //TrainingPrograms
            builder.HasMany(x => x.TrainingPrograms)
                .WithMany(x => x.Syllabuses);

            //AuditPlans
            builder.HasMany(x => x.AuditPlans)
                .WithOne(x => x.Syllabus)
                .OnDelete(DeleteBehavior.Cascade);

            //Created
            builder.HasOne(x => x.CreatedAdmin)
                .WithMany(x => x.CreatedSyllabus)
                .HasForeignKey(x => x.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull);

            //Modified
            builder.HasOne(x => x.ModifiedAdmin)
                .WithMany(x => x.ModifiedSyllabus)
                .HasForeignKey(x => x.LastModifiedBy)
                .OnDelete(DeleteBehavior.ClientSetNull);
        }
    }
}