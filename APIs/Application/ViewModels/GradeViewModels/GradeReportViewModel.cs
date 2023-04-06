using Domain.Enums.GradeReportEnums;
using Domain.Models.Users;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels.GradeViewModels
{
    public class GradeReportViewModel
    {

        public int TraineeId { get; set; }
        public string type { get; set; }
        public string Trainee { get; set; }
        public int LectureId { get; set; }
        public string Lecture { get; set; }

        public string Grade { get; set; }

    }
}

