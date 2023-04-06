using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels.ClassViewModels
{
    public class UpdateClassViewModel
    {
        public string Name { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Location { get; set; }
        public int TrainingProgramId { get; set; }
        public string StatusClass { get; set; }
        public string Attendee { get; set; }
    }
}
