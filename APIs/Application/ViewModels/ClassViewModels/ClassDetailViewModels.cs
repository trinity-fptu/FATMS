using System.Diagnostics.CodeAnalysis;

namespace Application.ViewModels.ClassViewModels
{
    public class ClassDetailViewModels
    {
        public int Id { get; set; }
        public int TrainingProgramId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public DateTime CreateOn { get; set; }
        public Admin CreatedAdmin { get; set; }
        public string? AttendeeType { get; set; }
        public string? Location { get; set; }
        public string? FSU { get; set; }
        public string Status { get; set; }
        public string LectureStartedTime { get; set; }
        public string LectureFinishedTime { get; set; }
        public ICollection<string> LocationDetail { get; set; }
        public ICollection<Trainer> Trainer { get; set; }
        public ICollection<Admin> Admin { get; set; }
        public ICollection<ClassTimeFrame> ClassTimeFrame { get; set; }
        public ICollection<string> ListDateHighLight { get; set; }
        public int ClassAttendeePlanned { get; set; }
        public int ClassAttendeeAccepted { get; set; }
        public int ClassAttendeeActual { get; set; }
    }

    public class Trainer
    {
        public int TrainerId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string AvatarUrl { get; set; }
    }

    public class TrainerComparer : IEqualityComparer<Trainer>
    {
        public bool Equals(Trainer? x, Trainer? y)
        {
            return x.TrainerId == y.TrainerId;
        }

        public int GetHashCode([DisallowNull] Trainer obj)
        {
            return obj.TrainerId.GetHashCode();
        }
    }
    public class Admin
    {
        public int AdminId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
    }
}


