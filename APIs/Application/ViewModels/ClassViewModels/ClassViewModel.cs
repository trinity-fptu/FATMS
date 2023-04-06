
using Domain.Enums.ClassEnums;
using Domain.Models;
using Domain.Models.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels.ClassViewModels
{
    public class ClassViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string ClassTime { get; set; }
        public string CreatedOn { get; set; }
        public string StartedOn { get; set; }
        public string FinishedOn { get; set; }
        public string? AttendeeType { get; set; }
        public string? Location { get; set; }
        public string? FSU { get; set; }
        public string Status { get; set; }
        public string? CreatedBy { get; set; }
        public int TrainingProgramId { get; set; }
        public List<ClassUserViewModel> Trainers { get; set; }
    }
}
