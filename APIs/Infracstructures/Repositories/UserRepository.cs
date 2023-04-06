using Application.Interfaces;
using Application.Repositories;
using Domain.Models.Users;
using Microsoft.EntityFrameworkCore;

namespace Infracstructures.Repositories
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(AppDbContext context, ICurrentTime timeService, IClaimsService claimsService) : base(context, timeService, claimsService)
        {
        }

        #region GetAllAsync
        /// <summary>
        /// Method for get all user from database.
        /// </summary>
        /// <returns> Return list of user. </returns>
        public override async Task<List<User>> GetAllAsync()
        {
            return await _dbSet.Include(x => x.Role).Where(x => !x.isDeleted).ToListAsync();
        }
        #endregion

        #region GetUserByEmail
        public async Task<User> GetUserByEmailAsync(string email)
        {
            var user = await _dbSet.Include(x => x.Role).FirstOrDefaultAsync(user => user.Email == email);
            if (user == null) throw new Exception("User not found");
            return user;
        }
        #endregion

        #region GetUserByUserEmailAndPasswordHash
        public async Task<User?> GetUserByUserEmailAndPasswordHash(string email, string passwordHash)
        {
            var user = await _dbSet.Include(x => x.Role).FirstOrDefaultAsync(us => us.Email.Equals(email) && us.Password.Equals(passwordHash));
            return user;
        }
        #endregion

        #region IsExistsUser
        public async Task<bool> IsExistsUser(string email)
        {
            return await _dbSet.FirstOrDefaultAsync(us => us.Email.Equals(email)) != null;
        }
        #endregion

        #region IsExistNameAsync
        public async Task<bool> IsExistNameAsync(string name)
        {
            return await _dbSet.FirstOrDefaultAsync(us => us.FullName.Equals(name) && !us.isDeleted) != null;
        }
        #endregion

        #region GetUserByRoleAsync
        public async Task<List<User>> GetUserByRoleAsync(string roleName)
        {
            return await _dbSet.Include(x => x.Role)
                .Where(x => x.Role.Name.Equals(roleName) && !x.isDeleted)
                .ToListAsync();
        }
        #endregion

        #region GetByIdAsyncIncludeRole
        public async Task<User?> GetByIdAsync(int id) 
        {
            return await _dbSet.Include(x => x.Role).FirstOrDefaultAsync(x => x.Id == id);
        }
        #endregion
    }
}