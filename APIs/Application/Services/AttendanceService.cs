using Application.Interfaces;
using Application.IValidators;
using Application.ViewModels.AddLectureViewModels;
using Application.ViewModels.AttendanceViewModels;
using Application.ViewModels.TrainingProgramViewModels;
using Application.ViewModels.UserViewModels;
using AutoMapper;
using Domain.Enums.AttendanceEnums;
using Domain.Models;
using FluentValidation.Results;
using Microsoft.Extensions.Configuration;
using Org.BouncyCastle.Utilities;

using System.Web.Mvc;

namespace Application.Services
{
    public class AttendanceService : IAttendanceService
    {
        //can remove anything if dont use
        private readonly IUnitOfWork _unitOfWork;

        private readonly IMapper _mapper;
        private readonly IClaimsService _claimsService;
        private readonly ICurrentTime _currentTime;
        private readonly IConfiguration _configuration;
        private readonly IAttendanceValidator _validator;

        public AttendanceService(IUnitOfWork unitOfWork, IMapper mapper, IClaimsService claimsService, ICurrentTime currentTime, IConfiguration configuration, IAttendanceValidator validator)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _claimsService = claimsService;
            _currentTime = currentTime;
            _configuration = configuration;
            _validator = validator;
        }

        public async Task<ValidationResult> ValidateTakeAttendanceAsync(TakeAttendanceModel attendance)
        {
            return await _validator.TakeAttendanceModel.ValidateAsync(attendance);
        }
        //action
        public async Task<bool> EditAttendanceAsync(EditAttendanceViewModel editAttendanceViewModel, int id)
        {
            var attendanceObj = await _unitOfWork.AttendancesRepo.GetByIdAsync(id);

            if (attendanceObj == null)
            {
                throw new Exception("Attendance Not Found");
            }

            //Map updateTrainingProgramViewModel to TrainingProgram
            _mapper.Map(editAttendanceViewModel, attendanceObj);

            _unitOfWork.AttendancesRepo.Update(attendanceObj);

            //Save change
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<AttendanceViewModel> TakeAttendance(TakeAttendanceModel model)
        {
            var checkID = await _unitOfWork.AttendancesRepo.GetAllAsync();
            //Check attendance existence
            var exist = await _unitOfWork.AttendancesRepo.GetAttendancesByClassUserId(model.ClassUserID);
            if (exist != null)
            {
                //Get latest attendance
                var checkDate = (await _unitOfWork.AttendancesRepo.GetAllAttendancesByClassUserIdDesc(model.ClassUserID))[0];
                //Compared Latest Date with Now
                int result = DateTime.Now.Date.CompareTo(checkDate.Day.Date);
                 
            if (checkID?.FirstOrDefault(x => x.ClassUserId.Equals(model.ClassUserID)) != null && result == 0)
                    
                    throw new Exception("This Class userId attendance have been already taken today!!");
            }
            //Check Enum
            //int checkstatus = (int)model.AttendanceStatus;
            //if (checkstatus > 4 || checkstatus < 0)
            //{
            //    throw new Exception("Attendace Status Input must be from 0 to 4 !!");
            //}
            Attendance attendance = _mapper.Map<Attendance>(model);
            attendance.Day = DateTime.Now;
            //Add Attendance
            await _unitOfWork.AttendancesRepo.AddAsync(attendance);
            //Save change
            var check = await _unitOfWork.SaveChangesAsync() > 0;
            AttendanceViewModel view = _mapper.Map<AttendanceViewModel>(attendance);
            if (check)
            {
                return view;

            }
            return null;

        }
        public async Task<List<AttendanceViewModel>> GetAllAttendanceAsync(int id)
        {
            // get all classUserId by classId
            var listClassUserId = await _unitOfWork.ClassUserRepo.GetClassUsers(id);
            if (listClassUserId == null || !listClassUserId.Any())
            {
                throw new ArgumentException("No class users found for given class ID.");
            }
            // get all attendance by classUserId
            var listAttendance = new List<AttendanceViewModel>();
            foreach (var classUserId in listClassUserId)
            {
                var attendance = await _unitOfWork.AttendancesRepo.GetAllAttendancesByClassUserId(classUserId.Id);
                var attendanceViewModel = _mapper.Map<List<AttendanceViewModel>>(attendance);
                listAttendance.AddRange(attendanceViewModel);
                if (listAttendance == null || !listAttendance.Any())
                {
                    throw new ArgumentException("No attendance found for given class user ID.");
                }
            }
            return listAttendance;
        }

    }
}
































































































