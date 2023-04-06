using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infracstructures.FluentAPIs
{
    public class ClassUserConfiguration : IEntityTypeConfiguration<ClassUsers>
    {
        public void Configure(EntityTypeBuilder<ClassUsers> builder)
        {
            builder.ToTable("ClassUsers");

            //Id
            builder.HasKey(x => x.Id);

            //Role
            builder.Property(x => x.Role).IsRequired();

            //Class
            builder.HasOne(x => x.Class)
                .WithMany(x => x.ClassUsers)
                .HasForeignKey(x => x.ClassId);

            //User
            builder.HasOne(x => x.User)
                .WithMany(x => x.ClassUsers)
                .HasForeignKey(x => x.UserId);

            //Attendance
            builder.HasMany(x => x.Attendances)
                .WithOne(x => x.ClassUser)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}