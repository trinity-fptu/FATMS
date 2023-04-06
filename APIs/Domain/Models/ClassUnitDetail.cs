using Domain.Enums.ClassEnums;
using Domain.Models.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class ClassUnitDetail
    {
        public int ClassId { get; set; }
        public int UnitId { get; set; }

        public ClassLocation? Location { get; set; }
        public int? TrainerId { get; set; }
        public int? DayNo { get; set; }
        public DateTime? OperationDate { get; set; }

        public Class Class { get; set; }
        public Unit Unit { get; set; }
        public User? Trainer { get; set; }  
    }
}
