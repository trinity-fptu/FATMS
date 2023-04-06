using Domain.Enums.ClassEnums;
using Domain.Models.Users;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels.ClassUnitViewModel
{
    public class EditClassUnitViewModel
    {
        public int UnitId { get; set; }
        public ClassLocation Location { get; set; }
        public int TrainerId { get; set; }
        public int DayNo { get; set; }
        public DateTime OperationDate { get; set; }
    }
}
