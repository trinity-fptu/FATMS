using Domain.Enums.AuditDetailsEnums;
using Domain.Models.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class AuditResult
    {
        public int Id { get; set; }
        public string Question { get; set; }
        public string TraineeAnswer { get; set; }
        public AuditAnswerRating Rating { get; set; }
        public int AuditDetailId { get; set; }
        public AuditDetail AuditDetail { get; set; }
    }
}
