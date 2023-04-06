using Domain.Enums.AttendanceEnums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels.AttendanceViewModels
{
    public class TakeAttendanceModel
    {
        public int ClassUserID { get; set; }
        public AttendanceStatus? AttendanceStatus { get; set; }
        public string Reason { get; set; }
    }

}
