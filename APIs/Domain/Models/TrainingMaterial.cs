#nullable disable warnings

using Domain.Models.Base;
using Domain.Models.Users;

namespace Domain.Models
{
    public class TrainingMaterial : BaseModel
    {
        public string Name { get; set; } 
        public string Url { get; set; }

        public int LectureId { get; set; }
        public Lecture? Lecture { get; set; }
        public User? User { get; set; }
    }
}