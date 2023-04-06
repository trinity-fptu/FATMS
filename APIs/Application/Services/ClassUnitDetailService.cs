using Application.Interfaces;
using Application.ViewModels.ClassUnitViewModel;
using Application.ViewModels.TrainingProgramViewModels;
using AutoMapper;
using AutoMapper.Configuration.Conventions;
using Domain.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class ClassUnitDetailService: IClassUnitDetailService
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly ICurrentTime _currentTime;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public ClassUnitDetailService(IUnitOfWork unitOfWork, ICurrentTime currentTime, IConfiguration configuration, IMapper mapper, IClaimsService claimsService)
        {
            _unitOfWork = unitOfWork;
            _currentTime = currentTime;
            _configuration = configuration;
            _mapper = mapper;
        }

        public async Task<bool> AddClassUnitAsync(AddClassUnitViewModel addClassUnitViewModel)
        {
            var classUnitObj = _mapper.Map<ClassUnitDetail>(addClassUnitViewModel);
            await _unitOfWork.ClassUnitRepo.AddAsync(classUnitObj);
            var isSucces = await _unitOfWork.SaveChangesAsync() > 0;
            if (isSucces)
            {
                return true;
            }
            else return false;
        }

        public async Task EditClassUnitAsync(EditClassUnitViewModel editClassUnitViewModel,int classId)
        {
            var classUnitObj = _mapper.Map<ClassUnitDetail>(editClassUnitViewModel);
            classUnitObj.ClassId= classId;
            _unitOfWork.ClassUnitRepo.Update(classUnitObj);
            var isSucces= await _unitOfWork.SaveChangesAsync()>0;
            if (!isSucces) throw new Exception("Save Failed");
        }

        public async Task<string> EditListClassUnitAsync(List<EditClassUnitViewModel> listClassUnit, int classId)
        {
            foreach(var classUnit in listClassUnit)
            {
                await EditClassUnitAsync(classUnit, classId);
            }
            return "Edit Succeed";
        }
    }
}
