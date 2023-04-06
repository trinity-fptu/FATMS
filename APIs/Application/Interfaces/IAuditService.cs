 using Application.ViewModels.AuditViewModels;

namespace Application.Interfaces
{
    public interface IAuditService
    {
        //your action is here
        Task<AddAuditPlanViewModel> AddAuditPlanAsync(AddAuditPlanViewModel model);
        Task<AddAuditDetailViewModel> AddAuditDetailAsync(AddAuditDetailViewModel model);
        Task<AddAuditResultViewModel> AddAuditResultAsync(AddAuditResultViewModel model);
        Task<AuditResultDetailViewModel> GetAuditResultDetailAsync(int auditDetailId);
        Task<AuditResultDetailViewModel> UpdateFeedbackAuditDeatilsAsync(int id, string feedback);
    }
}