#nullable disable warnings

namespace Domain.Models
{
    public class OutputStandard
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }

        public ICollection<Lecture> Lectures { get; set; }
    }
}