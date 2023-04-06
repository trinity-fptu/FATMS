using Domain.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class QuizDetail
    {
        public int Id { get; set; }
        public int QuizId { get; set; }
        public int QuizBankId { get; set; }
        public int? Grade { get; set; }

        public Quiz Quiz { get; set; }
        public QuizBank QuizBank { get; set; }

        public ICollection<QuizRecord> QuizRecords { get; set; }
    }
}
