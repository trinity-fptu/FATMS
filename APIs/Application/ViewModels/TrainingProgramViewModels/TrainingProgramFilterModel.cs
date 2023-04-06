using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels.TrainingProgramViewModels
{
    public  class TrainingProgramFilterModel
    {
        public string Name { get; set; }
        public string[] CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public bool? IsActive { get; set; }
        public int Duration { get; set; }
    }
}
