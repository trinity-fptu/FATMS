using Application.ViewModels.TimeManagementSystemViewModels;
using Domain.Models;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface ITMSService
    {
        public Task<bool> CreateAbsentRequestAsync(string reason);
        public Task<bool> ApproveAbsentRequestAsync(int id, string status);
        public Task<TMS> GetAbsentRequestByIdAsync(int id);
        public Task<List<TMSListViewModel>> GetAllAbsentRequestsAsync();
        public Task<List<TMSPendingListViewModel>> GetAllPendingAbsentRequestsAsync();
        public Task<List<TMSListViewModel>> SearchAbsentRequest(string searchBy, string searchElement);
        public Task SendAttendanceMailAsync();
    }
}