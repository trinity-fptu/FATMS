#nullable disable warnings

using Domain.Models.Syllabuses;

namespace Domain.Models
{
    public class Unit
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Session { get; set; }
        public bool isDeleted { get; set; } = false;
        public int Duration { get; set; }

        public ICollection<Syllabus> Syllabuses { get; set; }
        public ICollection<Lecture> Lectures { get; set; }
        public ICollection<ClassUnitDetail> ClassUnitDetails { get; set; }
        public ICollection<QuizBank> QuizBanks { get; set; }
    }
}