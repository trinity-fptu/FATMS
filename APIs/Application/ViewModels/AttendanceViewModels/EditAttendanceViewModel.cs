using Domain.Enums.AttendanceEnums;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels.AttendanceViewModels
{
    public class EditAttendanceViewModel
    {
        public AttendanceStatus? AttendanceStatus { get; set; }
        public string Reason { get; set; }

        //public ClassUsers? ClassUser { get; set; }
    }
}
