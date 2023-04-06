using Application.Interfaces;
using Application.Repositories;
using Domain.Enums.AttendanceEnums;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Infracstructures.Repositories
{
    public class AttendancesRepository : GenericRepository<Attendance>, IAttendancesRepository
    {
        public AttendancesRepository(AppDbContext context, ICurrentTime timeService, IClaimsService claimsService) : base(context, timeService, claimsService)
        {
        }

        //your func

        public async Task<List<Attendance>> GetAllAttendancesByClassUserId(int classUserId)
        {
            return await _dbSet.Where(x => x.ClassUserId == classUserId).ToListAsync();
        }
        public async Task<Attendance> GetAttendancesByClassUserId(int classUserId)
        {
            var attendance = await _dbSet.FirstOrDefaultAsync(x => x.ClassUserId == classUserId);
            return attendance;
        }

        public async Task<List<Attendance>> GetAllAttendancesByClassUserIdDesc(int classUserId)
        {
            return await _dbSet.Where(x => x.ClassUserId == classUserId).OrderByDescending(x => x.Day).ToListAsync();
        }
        public async Task<List<Attendance>> GetAbsenteeListAsync()
        {
            return await _dbSet
                .Include(a => a.ClassUser)
                .ThenInclude(a => a.User)
                .Include(a => a.ClassUser)
                .ThenInclude(a => a.Class)
                .ThenInclude(a => a.TrainingProgram)
                .ThenInclude(a => a.CreatedAdmin)
                .Where(a => a.AttendanceStatus == AttendanceStatus.Absent)
                .ToListAsync();
        } 
    }
}