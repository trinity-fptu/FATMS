
namespace Application.ViewModels.AuditViewModels
{
    public class AddAuditPlanViewModel
    {
        public DateTime AuditDate { get; set; }
        public string Location { get; set; }
        public int SyllabusId { get; set; }
        public int ClassId { get; set; }
    }
}
