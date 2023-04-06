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
    public class GradeViewModel
    {
        public string Type { get; set; }
//        public string GradedOn { get; set; }
        public string Grade { get; set; }

        public int TraineeId { get; set; }       
        public int LectureId { get; set; }
    }
}
