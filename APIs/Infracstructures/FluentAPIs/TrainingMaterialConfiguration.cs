using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infracstructures.FluentAPIs
{
    public class TrainingMaterialConfiguration : IEntityTypeConfiguration<TrainingMaterial>
    {
        public void Configure(EntityTypeBuilder<TrainingMaterial> builder)
        {
            builder.ToTable("TrainingMaterials");

            //Id
            builder.HasKey(x => x.Id);

            //Name
            builder.Property(x => x.Name).HasMaxLength(250);

            //EditedBy
            builder.Property(x => x.CreatedBy).HasColumnName("EditedBy");

            //EditedOn
            builder.Property(x => x.CreatedOn).HasColumnName("EditedOn");

            //Lecture
            builder.HasOne(x => x.Lecture)
                .WithMany(x => x.TrainingMaterials)
                .HasForeignKey(x => x.LectureId);

            //EditedBy
            builder.HasOne(x => x.User)
                .WithMany(x => x.TrainingMaterials)
                .HasForeignKey(x => x.CreatedBy);
        }
    }
}