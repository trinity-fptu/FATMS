using Application.ViewModels.AttendanceViewModels;
using Domain.Models;

namespace Application.Repositories
{
    public interface IAttendancesRepository : IGenericRepository<Attendance>
    {
        //add your own func here
        Task<List<Attendance>> GetAllAttendancesByClassUserId(int classUserId);
        Task<List<Attendance>> GetAllAttendancesByClassUserIdDesc(int classUserId);
        Task<Attendance> GetAttendancesByClassUserId(int classUserId);
        Task<List<Attendance>> GetAbsenteeListAsync();
    }
}
