using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels.AuditViewModels
{
    public class AuditResultViewModel
    {
        public string Question { get; set; } = string.Empty;
        public string TraineeAnswer { get; set; } = string.Empty;
        public string Rating { get; set; } = string.Empty;
    }
}
