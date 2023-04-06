using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infracstructures.FluentAPIs
{
    public class LectureConfiguration : IEntityTypeConfiguration<Lecture>
    {
        public void Configure(EntityTypeBuilder<Lecture> builder)
        {
            builder.ToTable("Lectures")
                .ToTable(x => x
                .HasTrigger("trgLectureInsert"))
                .ToTable(x => x
                .HasTrigger("trgLectureDelete"))
                .ToTable(x => x
                .HasTrigger("trgLectureUpdate"));

            //Id
            builder.HasKey(x => x.Id);

            //Name
            builder.Property(x => x.Name).IsRequired()
                .HasMaxLength(100);

            //Duration
            builder.Property(x => x.Duration).IsRequired();

            //OutputStandardId
            builder.Property(x => x.OutputStandardId).IsRequired(false);

            //UnitId
            builder.Property(x => x.UnitId).IsRequired(false);

            //Unit
            builder.HasOne(x => x.Unit)
                .WithMany(x => x.Lectures)
                .HasForeignKey(x => x.UnitId);

            //OutputStandard
            builder.HasOne(x => x.OutputStandard)
                .WithMany(x => x.Lectures)
                .HasForeignKey(x => x.OutputStandardId);

            //GradeReport
            builder.HasMany(x => x.GradeReports)
                .WithOne(x => x.Lecture)
                .OnDelete(DeleteBehavior.Cascade);

            //TrainingMaterials
            builder.HasMany(x => x.TrainingMaterials)
                .WithOne(x => x.Lecture)
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}