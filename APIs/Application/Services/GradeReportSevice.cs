using Application.Interfaces;
using Application.ViewModels.GradeViewModels;
using AutoMapper;
using Domain.Enums.ClassEnums;
using Domain.Models;
using Microsoft.Extensions.Configuration;

namespace Application.Services
{
    public class GradeReportSevice : IGradeReportService
    {
        //can remove anything if dont use
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICurrentTime _currentTime;
        private readonly IConfiguration _configuration;

        public GradeReportSevice(IUnitOfWork unitOfWork, ICurrentTime currentTime, IConfiguration configuration, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _currentTime = currentTime;
            _configuration = configuration;
            _mapper = mapper;
        }

        #region Create GradeReport
        public async Task CreateReportAsync(GradeViewModel gradeViewModel)
        {
            var report = _mapper.Map<GradeReport>(gradeViewModel);
            report.GradedOn = _currentTime.GetCurrentTime();
            
            await _unitOfWork.GradeReportRepo.AddAsync(report);
            var isSuccess = await _unitOfWork.SaveChangesAsync()>0;
            if (isSuccess == false)
            throw new Exception("Save failed");
        }


        #endregion
        #region ViewGradeReport
        public async Task<GradeReportViewModel> ViewGradeReportByIdAsync(int id)
        {
            var grade = await _unitOfWork.GradeReportRepo.GetByIdAsync(id);
            var report = _mapper.Map<GradeReportViewModel>(grade);
            if (report != null)
            {
                return report;
            }
            return null;
        }
        public async Task<List<GradeReportViewModel>> ViewAllGradeReportAsync()
        {
            ICollection<GradeReportViewModel> GradeList = new List<GradeReportViewModel>();
            var grade = await _unitOfWork.GradeReportRepo.GetAllAsync();
            GradeList = _mapper.Map<List<GradeReportViewModel>>(grade);
            if (GradeList.Count != 0)
            {
                return (List<GradeReportViewModel>)GradeList;
            }
            return null;
        }
        #endregion
    }
}