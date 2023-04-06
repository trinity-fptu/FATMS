using Domain.Models;
using Domain.Models.Syllabuses;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infracstructures.FluentAPIs
{
    public class ClassUnitDetailConfiguration : IEntityTypeConfiguration<ClassUnitDetail>
    {
        public void Configure(EntityTypeBuilder<ClassUnitDetail> builder)
        {
            builder.ToTable("ClassUnitDetail");

            builder.HasKey(x => new
            {
                x.ClassId,
                x.UnitId
            });

            builder.HasOne(x => x.Class)
                .WithMany(x => x.ClassUnitDetails)
                .HasForeignKey(x => x.ClassId);

            builder.HasOne(x => x.Unit)
                .WithMany(x => x.ClassUnitDetails)
                .HasForeignKey(x => x.UnitId);

            builder.HasOne(x => x.Trainer)
                .WithMany(x => x.ClassUnitDetails)
                .HasForeignKey(x => x.TrainerId);
        }
    }
}
