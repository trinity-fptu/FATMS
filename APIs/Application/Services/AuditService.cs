using Application.Interfaces;
using Application.ViewModels.AuditViewModels;
using AutoMapper;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Application.Services
{
    public class AuditService : IAuditService
    {
        //can remove anything if dont use
        private readonly IUnitOfWork _unitOfWork;

        private readonly IMapper _mapper;
        private readonly IClaimsService _claimsService;
        private readonly ICurrentTime _currentTime;
        private readonly IConfiguration _configuration;

        public AuditService(IUnitOfWork unitOfWork, IMapper mapper, IClaimsService claimsService, ICurrentTime currentTime, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _claimsService = claimsService;
            _currentTime = currentTime;
            _configuration = configuration;
        }

        public async Task<AddAuditDetailViewModel> AddAuditDetailAsync(AddAuditDetailViewModel model)
        {
            try
            {
                var auditDetail = _mapper.Map<AuditDetail>(model);
                await _unitOfWork.AuditDetailRepo.AddAsync(auditDetail);
                await _unitOfWork.SaveChangesAsync();
                return model;
            }
            catch (DbUpdateException)
            {
                throw new ArgumentException("Add audit detail fail");
            }
        }

        public async Task<AddAuditPlanViewModel> AddAuditPlanAsync(AddAuditPlanViewModel model)
        {
            try
            {
                var auditPlan = _mapper.Map<AuditPlan>(model);
                auditPlan.PlannedBy = _claimsService.GetCurrentUserId;
                await _unitOfWork.AuditPlanRepo.AddAsync(auditPlan);
                await _unitOfWork.SaveChangesAsync();
                return model;
            }
            catch (DbUpdateException)
            {
                throw new ArgumentException("Add audit plan fail");
            }
        }

        public async Task<AddAuditResultViewModel> AddAuditResultAsync(AddAuditResultViewModel model)
        {
            try
            {
                var auditResult = _mapper.Map<AuditResult>(model);
                await _unitOfWork.AuditResultRepo.AddAsync(auditResult);
                await _unitOfWork.SaveChangesAsync();
                model.Rating = auditResult.Rating.ToString();
                return model;
            }
            catch (DbUpdateException)
            {
                throw new ArgumentException("Add audit result fail");
            }
        }

        public async Task<AuditResultDetailViewModel> GetAuditResultDetailAsync(int auditDetailId)
        {
            var auditDetail = await _unitOfWork.AuditDetailRepo.GetByIdAsync(auditDetailId);
            if (auditDetail == null)
            {
                throw new NullReferenceException("Audit detail not found");
            }
            var auditResultDetail = _mapper.Map<AuditResultDetailViewModel>(auditDetail);
            return auditResultDetail;
        }
        public async Task<AuditResultDetailViewModel> UpdateFeedbackAuditDeatilsAsync(int id, string feedback)
        {
            var audtiDeatil = await _unitOfWork.AuditDetailRepo.GetByIdAsync(id);
            if (audtiDeatil == null) throw new NullReferenceException("AuditDetail not found");
            audtiDeatil.Feedback = feedback;
            _unitOfWork.AuditDetailRepo.Update(audtiDeatil);
            _unitOfWork.SaveChangesAsync();
            var mapper = _mapper.Map<AuditResultDetailViewModel>(audtiDeatil);
            return mapper;
        }
    }
}