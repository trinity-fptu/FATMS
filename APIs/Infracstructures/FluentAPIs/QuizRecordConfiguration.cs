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
    internal class QuizRecordConfiguration : IEntityTypeConfiguration<QuizRecord>
    {
        public void Configure(EntityTypeBuilder<QuizRecord> builder)
        {
            builder.ToTable(nameof(QuizRecord));

            builder.HasKey(x => new
            {
                x.TraineeId,
                x.QuizDetailId
            });

            builder.HasOne(x => x.Trainee)
                .WithMany(x => x.QuizRecords)
                .HasForeignKey(x => x.TraineeId)
                .OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(x => x.QuizDetail)
                .WithMany(x => x.QuizRecords)
                .HasForeignKey(x => x.QuizDetailId);

        }
    }
}
