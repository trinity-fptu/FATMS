using Domain.Enums.QuizEnums;
using Domain.Models.Base;
using Domain.Models.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Quiz : BaseModel
    {
        public int ClassId { get; set; }
        public string? Name { get; set; }
        public DateTime? Date { get; set; }
        public QuizStatus QuizStatus { get; set; }
        public DateTime? TimeLimit { get; set; }

        public Class Class { get; set; }
        public User CreateTrainer { get; set; }
        public ICollection<QuizDetail> QuizDetails { get; set; }
    }
}
