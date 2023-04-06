using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infracstructures.FluentAPIs
{
    public class UnitConfiguration : IEntityTypeConfiguration<Unit>
    {
        public void Configure(EntityTypeBuilder<Unit> builder)
        {
            builder.ToTable("Units")
                .ToTable(x => x
                .HasTrigger("trgUnitUpdate"));

            //Id
            builder.HasKey(x => x.Id);

            //Name
            builder.Property(x => x.Name).HasMaxLength(250);

            //Session
            builder.Property(x => x.Session).IsRequired();

            //Syllabus
            builder.HasMany(x => x.Syllabuses)
                .WithMany(x => x.Units);

            //Lecture
            builder.HasMany(x => x.Lectures)
                .WithOne(x => x.Unit)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}