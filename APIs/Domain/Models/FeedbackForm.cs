#nullable disable warnings

using Domain.Models.Users;

namespace Domain.Models
{
    public class FeedbackForm
    {
        public int Id { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; }

        public int? TraineeId { get; set; }
        public User? Trainee { get; set; }
        public int? TrainerId { get; set; }
        public User? Trainer { get; set; }
    }
}