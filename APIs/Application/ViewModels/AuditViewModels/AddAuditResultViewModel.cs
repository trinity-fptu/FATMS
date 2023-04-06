using Domain.Enums.AuditDetailsEnums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels.AuditViewModels
{
    public class AddAuditResultViewModel
    {
        public string Question { get; set; }
        public string TraineeAnswer { get; set; }
        public string Rating { get; set; }
        public int AuditDetailId { get; set; }
    }
}
