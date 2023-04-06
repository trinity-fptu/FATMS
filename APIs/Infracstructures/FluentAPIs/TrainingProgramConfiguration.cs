using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infracstructures.FluentAPIs
{
    public class TrainingProgramConfiguration : IEntityTypeConfiguration<TrainingProgram>
    {
        public void Configure(EntityTypeBuilder<TrainingProgram> builder)
        {
            builder.ToTable("TrainingPrograms");

            //Id
            builder.HasKey(x => x.Id);

            //Name
            builder.Property(x => x.Name).HasMaxLength(500);

            //CreatedBy
            builder.Property(x => x.CreatedBy).IsRequired(false);

            //ModifiedBy
            builder.Property(x => x.LastModifyBy).IsRequired(false);

            //Class
            builder.HasMany(x => x.Classes)
                .WithOne(x => x.TrainingProgram)
                .OnDelete(DeleteBehavior.SetNull);

            //Syllabus
            builder.HasMany(x => x.Syllabuses)
                .WithMany(x => x.TrainingPrograms);

            //Modified
            builder.HasOne(x => x.ModifiedAdmin)
                .WithMany(x => x.ModifyTrainingProgram)
                .HasForeignKey(x => x.LastModifyBy);

            //Created
            builder.HasOne(x => x.CreatedAdmin)
                .WithMany(x => x.CreatedTrainingProgram)
                .HasForeignKey(x => x.CreatedBy);
        }
    }
}