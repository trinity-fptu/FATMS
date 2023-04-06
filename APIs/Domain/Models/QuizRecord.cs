using Domain.Models.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class QuizRecord
    {
        public int QuizDetailId { get; set; }
        public int TraineeId { get; set; }
        public string TraineeAnswer { get; set; }
        public double Grade { get; set; }
        
        public User Trainee { get; set; }
        public QuizDetail QuizDetail { get; set; }
    }
}
