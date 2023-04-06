using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infracstructures.FluentAPIs
{
    public class FeebackFormConfiguration : IEntityTypeConfiguration<FeedbackForm>
    {
        public void Configure(EntityTypeBuilder<FeedbackForm> builder)
        {
            builder.ToTable("FeedBackForms");

            //Id
            builder.HasKey(x => x.Id);

            //Rating
            builder.Property(x => x.Rating).IsRequired();

            //Comment
            builder.Property(x => x.Comment).HasMaxLength(250);

            //Trainee
            builder.HasOne(x => x.Trainee)
                .WithMany(x => x.FeedbackTrainee)
                .HasForeignKey(x => x.TraineeId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            //Trainer
            builder.HasOne(x => x.Trainer)
                .WithMany(x => x.FeedbackTrainer)
                .HasForeignKey(x => x.TrainerId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        }
    }
}