using Application.Interfaces;
using Application.IValidators;
using Application.ViewModels.UnitViewModels;
using AutoMapper;
using Domain.Models;
using Domain.Models.Syllabuses;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class UnitService : IUnitService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IClaimsService _claimsService;
        private readonly ICurrentTime _currentTime;
        private readonly IConfiguration _configuration;

        public UnitService(
            IUnitOfWork unitOfWork, 
            IMapper mapper, 
            IClaimsService claimsService, 
            ICurrentTime currentTime, 
            IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _claimsService = claimsService;
            _currentTime = currentTime;
            _configuration = configuration;
        }

        //public async Task AddAsync(UnitAddViewModel viewModel)
        //{
            
        //    var unit = _mapper.Map<Unit>(viewModel);
        //    var syllabus = await _unitOfWork.SyllabusRepo.GetByIdAsync(viewModel.SyllabusId);
        //    if (syllabus == null) throw new Exception("Syllabus not found");

        //    if (unit.Syllabuses == null) unit.Syllabuses = new List<Syllabus>();
        //    unit.Syllabuses.Add(syllabus);

        //    await _unitOfWork.UnitRepo.AddAsync(unit);
        //    await _unitOfWork.SaveChangesAsync();
        //}


        public async Task<UnitDetailModel> GetUnitByIdAsync(int unitId)
        {
            var unit = await _unitOfWork.UnitRepo.GetUnitById(unitId);

            if (unit == null) throw new KeyNotFoundException("Unit not found");

            var unitResult = _mapper.Map<UnitDetailModel>(unit);
            return unitResult;
        }
    }
}
