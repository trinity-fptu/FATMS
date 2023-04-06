using Domain.Models.Base;
using Domain.Models.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class QuizBank : BaseModel
    {
        public string Question { get; set; }
        public string? Answer { get; set; }
        public int UnitId { get; set; }
        public int? Grade { get; set; }

        public Unit Unit { get; set; }
        public User CreateTrainer { get; set; }
        public ICollection<QuizDetail> QuizDetails { get; set; }
    }
}
