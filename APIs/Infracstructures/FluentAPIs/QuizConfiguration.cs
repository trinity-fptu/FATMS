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
    public class QuizConfiguration : IEntityTypeConfiguration<Quiz>
    {
        public void Configure(EntityTypeBuilder<Quiz> builder)
        {
            builder.ToTable(nameof(Quiz));

            builder.HasKey(x => x.Id);

            builder.HasMany(x => x.QuizDetails)
                .WithOne(x => x.Quiz)
                .HasForeignKey(x => x.QuizId);
        }
    }
}
