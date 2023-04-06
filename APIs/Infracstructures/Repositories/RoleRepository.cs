using Application.Interfaces;
using Application.Repositories;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Infracstructures.Repositories
{
    public class RoleRepository : GenericRepository<Role>, IRoleRepository
    {
        public RoleRepository(AppDbContext context, ICurrentTime timeService, IClaimsService claimsService) : base(context, timeService, claimsService) { }

        #region IsExistRoleAsync
        /// <summary>
        /// Function for checking this role is exist in database or not
        /// </summary>
        /// <param name="name"> The role's name </param>
        /// <returns> return true if this role is exist, otherwise return false. </returns>
        public async Task<bool> IsExistRoleAsync(string name)
        {
            return await _dbSet.FirstOrDefaultAsync(x => x.Name.Equals(name)) != null;
        }
        #endregion

        #region GetRoleByNameAsync
        /// <summary>
        /// Function for find a role by it name
        /// </summary>
        /// <param name="name"> The role's name </param>
        /// <returns> Return correct role if it's exists. </returns>
        public async Task<Role> GetRoleByNameAsync(string name)
        {
            var role = await _dbSet.FirstOrDefaultAsync(x => x.Name.Equals(name));
            if (role == null) throw new Exception("This role doesn't exist.");
            return role;
        }
        #endregion

        #region GenerateRoleIdAsync
        /// <summary>
        /// Function for generate an Id for table role.
        /// </summary>
        /// <returns> Return new role Id. </returns>
        public async Task<int> GenerateRoleIdAsync()
        {
            var roleId = (await _dbSet.ToListAsync()).Count;
            while (await _dbSet.FindAsync(roleId) != null)
            {
                roleId++;
            }
            return roleId;
        }
        #endregion
    }
}