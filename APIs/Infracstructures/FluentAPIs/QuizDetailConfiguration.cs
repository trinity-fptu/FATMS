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
    public class QuizDetailConfiguration : IEntityTypeConfiguration<QuizDetail>
    {
        public void Configure(EntityTypeBuilder<QuizDetail> builder)
        {
            builder.ToTable(nameof(QuizDetail));

            builder.HasKey(x => x.Id);

            builder.HasOne(x => x.Quiz)
                .WithMany(x => x.QuizDetails)
                .HasForeignKey(x => x.QuizId);
            builder.HasOne(x => x.QuizBank)
                .WithMany(x => x.QuizDetails)
                .HasForeignKey(x => x.QuizBankId);

            builder.HasMany(x => x.QuizRecords)
                .WithOne(x => x.QuizDetail)
                .HasForeignKey(x => x.QuizDetailId);

        }
    }
}
