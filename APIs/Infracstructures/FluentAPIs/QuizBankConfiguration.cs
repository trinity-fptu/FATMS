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
    public class QuizBankConfiguration : IEntityTypeConfiguration<QuizBank>
    {
        public void Configure(EntityTypeBuilder<QuizBank> builder)
        {
            builder.ToTable(nameof(QuizBank));
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Question)
                .IsRequired();

            builder.HasMany(x => x.QuizDetails)
                .WithOne(x => x.QuizBank)
                .HasForeignKey(x => x.QuizBankId);
            builder.HasOne(x => x.Unit)
                .WithMany(x => x.QuizBanks)
                .HasForeignKey(x => x.UnitId);
            builder.HasOne(x => x.CreateTrainer)
                .WithMany(x => x.QuizBanks)
                .HasForeignKey(x => x.CreatedBy);
        }
    }
}
