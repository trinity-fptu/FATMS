using Application.Interfaces;
using Application.Repositories;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Infracstructures.Repositories
{
    public class ClassUserRepository : GenericRepository<ClassUsers>, IClassUserRepository
    {
        private readonly AppDbContext _context;
        public ClassUserRepository(AppDbContext context, ICurrentTime timeService, IClaimsService claimsService) : base(context, timeService, claimsService)
        {
            _context = context;
    }

        public async Task<bool> IsExist(int classId, int userId)
        { 
            {
                var temp = await _dbSet.FirstOrDefaultAsync(x => x.ClassId == classId && x.UserId == userId);

                return (temp == null);
            }
        }

        //Hàm này xử lý DB đề lấy ra list ClassUser By ID
        public async Task<List<ClassUsers>> GetClassUsers(int id)
        {
            var listClassUsers = _context.ClassUsers.Where(x => x.ClassId == id).ToList();
            return listClassUsers;
        }

    }
}