using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infracstructures.FluentAPIs
{
    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.ToTable("Role");

            //Id
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).IsRequired().ValueGeneratedNever();

            //Name
            builder.HasIndex(x => x.Name).IsUnique();

            //Users
            builder.HasMany(x => x.Users)
                 .WithOne(x => x.Role)
                 .HasForeignKey(x => x.RoleId)
                 .OnDelete(DeleteBehavior.ClientSetNull);
        }
    }
}
