using Application.Interfaces;
using Application.IValidators;
using Application.Utils;
using Application.ViewModels.RoleViewModels;
using AutoMapper;
using Domain.Enums.RoleEnums;
using Domain.Models;
using FluentValidation.Results;

namespace Application.Services
{
    public class RoleService : IRoleService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IRoleValidator _roleValidator;

        public RoleService(IUnitOfWork unitOfWork, IMapper mapper, IRoleValidator roleValidator)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _roleValidator = roleValidator;
        }

        #region UpdateRoleValidationAsync
        public async Task<ValidationResult> UpdateRoleValidationAsync(RoleUpdateModel updateRole)
        {
            return await _roleValidator.RoleUpdateModel.ValidateAsync(updateRole);
        }
        #endregion

        #region CreateRoleValidatAsync
        public async Task<ValidationResult> CreateRoleValidatAsync(RoleCreateModel createRole)
        {
            return await _roleValidator.RoleCreateModel.ValidateAsync(createRole);
        }
        #endregion

        #region GetListRoleAsync
        /// <summary>
        /// The function to get all the role with its permisisons.
        /// </summary>
        /// <returns> Return the list of role </returns>
        public async Task<List<RoleViewModel>> GetListRoleAsync()
        {
            var roles = await _unitOfWork.RoleRepo.GetAllAsync();
            return _mapper.Map<List<RoleViewModel>>(roles);
        }
        #endregion

        #region GetListPermission
        /// <summary>
        /// The function to get the list of permission.
        /// </summary>
        /// <returns> Return the list string of permission. </returns>
        public List<string> GetListPermission()
        {
            return Enum.GetValues(typeof(UserPermission))
                .Cast<UserPermission>()
                .Select(x => x.ToString().GenerateStringFormat())
                .ToList();
        }
        #endregion

        #region EditPermissionAsync
        /// <summary>
        /// Function for update an permission of each role
        /// </summary>
        /// <param name="roles"> A list of update role model. </param>
        /// <returns> Return true if update succeed, otherwise false. </returns>
        /// <exception cref="NullReferenceException"></exception>
        public async Task<bool> EditPermissionAsync(List<RoleUpdateModel> roles)
        {
            var listUpdatedRole = new List<Role>();
            foreach (var role in roles)
            {
                var updatedRole = await _unitOfWork.RoleRepo.GetByIdAsync(role.Id);
                if (updatedRole == null)
                {
                    throw new NullReferenceException(nameof(updatedRole));
                }
                _mapper.Map(role, updatedRole);
                listUpdatedRole.Add(updatedRole);
            }
            _unitOfWork.RoleRepo.UpdateRange(listUpdatedRole);
            return await _unitOfWork.SaveChangesAsync() >= listUpdatedRole.Count;
        }
        #endregion

        #region AddRoleAsync
        /// <summary>
        /// Function for adding a new role.
        /// </summary>
        /// <param name="role"> A create role model. </param>
        /// <returns> Return true if add succeed, otherwise false. </returns>
        public async Task<bool> AddRoleAsync(RoleCreateModel role)
        {
            var createdRole = _mapper.Map<Role>(role);
            createdRole.Id = await _unitOfWork.RoleRepo.GenerateRoleIdAsync();
            await _unitOfWork.RoleRepo.AddAsync(createdRole);
            return await _unitOfWork.SaveChangesAsync() > 0;
        }
        #endregion
    }
}