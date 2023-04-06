using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels.ClassViewModels
{
    public class ClassTimeFrame
    {
        public ClassTimeFrame(string date, ICollection<UnitDetail> listUnit)
        {
            Date = date;
            ListUnit = listUnit;
        }

        public string Date { get; set; }
        public ICollection<UnitDetail> ListUnit { get; set; }
    }
    public class UnitDetail
    {
        public int UnitId { get; set; }
        public int TrainerId { get; set; }
        public string Location { get; set; }
    }
}
