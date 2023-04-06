using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels.AuditViewModels
{
    public class AddAuditDetailViewModel
    {
        public int PlanId { get; set; }
        public int AuditorId { get; set; }
        public int TraineeId { get; set; }
    }
}
