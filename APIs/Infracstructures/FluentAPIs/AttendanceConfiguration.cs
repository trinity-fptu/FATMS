using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infracstructures.FluentAPIs
{
    public class AttendanceConfiguration : IEntityTypeConfiguration<Attendance>
    {
        public void Configure(EntityTypeBuilder<Attendance> builder)
        {
            builder.ToTable("Attendances");

            //Id
            builder.HasKey(x => x.AttendanceId);

            //ClassUserId
            builder.Property(x => x.ClassUserId).IsRequired();

            //Day
            builder.Property(x => x.Day).IsRequired();

            //Reason
            builder.Property(a => a.Reason).HasMaxLength(1000);

            //ClassUsers
            builder.HasOne(x => x.ClassUser)
                .WithMany(x => x.Attendances)
                .HasForeignKey(x => x.ClassUserId);
        }
    }
}