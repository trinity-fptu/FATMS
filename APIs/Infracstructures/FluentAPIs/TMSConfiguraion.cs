using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infracstructures.FluentAPIs
{
    public class TMSConfiguration : IEntityTypeConfiguration<TMS>
    {
        public void Configure(EntityTypeBuilder<TMS> builder)
        {
            builder.ToTable("TMS");

            //Id
            builder.HasKey(x => x.Id);

            //Reason
            builder.Property(x => x.Reason)
                .HasMaxLength(1000);

            //CheckedId
            builder.Property(x => x.CheckedBy).IsRequired(false);

            //Checked
            builder.HasOne(x => x.Admin)
                .WithMany(x => x.TimeMngSystem)
                .HasForeignKey(x => x.CheckedBy)
                .OnDelete(DeleteBehavior.ClientSetNull);

            //Trainee
            builder.HasOne(x => x.Trainee)
                .WithMany(x => x.TimeMngSystemList)
                .HasForeignKey(x => x.TraineeId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        }
    }
}