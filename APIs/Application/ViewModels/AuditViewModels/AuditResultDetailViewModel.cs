using Domain.Models.Users;
using Domain.Models;

namespace Application.ViewModels.AuditViewModels
{
    public class AuditResultDetailViewModel
    {
        public string ClassCode { get; set; }
        public string SyllabusCode { get; set; }
        public string Feedback { get; set; }
        public string Status { get; set; }
        public int PlanId { get; set; }
        public string AuditorName { get; set; }
        public string TraineeName { get; set; }
        public string Location { get; set; }
        public string Date { get; set; }
        public int NumOfQuestion { get; set; } = 0;
        public ICollection<AuditResultViewModel> Results { get; set; }
    }
}
