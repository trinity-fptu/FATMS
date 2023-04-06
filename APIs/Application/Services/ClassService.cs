using Application.Interfaces;
using Application.ViewModels.ClassUnitViewModel;
using Application.ViewModels.ClassViewModels;
using Application.ViewModels.TrainingProgramViewModels;
using AutoMapper;
using Domain.Enums.ClassEnums;
using Domain.Models;
using Domain.Models.Users;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Globalization;
using System.Linq;

namespace Application.Services
{
    public class ClassService : IClassService
    {
        //can remove anything if dont use
        private readonly IUnitOfWork _unitOfWork;

        private readonly ICurrentTime _currentTime;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly IClaimsService _claimsService;
        private readonly ITrainingProgramService _trainingProgramService;

        public ClassService(IUnitOfWork unitOfWork, ICurrentTime currentTime, IConfiguration configuration, IMapper mapper, IClaimsService claimsService, ITrainingProgramService trainingProgramService)
        {
            _unitOfWork = unitOfWork;
            _currentTime = currentTime;
            _configuration = configuration;
            _mapper = mapper;
            _claimsService = claimsService;
            _trainingProgramService = trainingProgramService;
        }

        #region ListClass
        public async Task<List<ClassViewModel>> GetClassListAsync()
        {
            ICollection<ClassViewModel> classViewModels = new List<ClassViewModel>();
            var classes = await _unitOfWork.ClassRepo.GetAllAsync();

            if (classes.Count == 0)
            {
                throw new Exception("Class is null.");
            }

            foreach (var tmpClass in classes)
            {
                var classID = tmpClass.Id;
                var createdAd = await _unitOfWork.UserRepo.GetByIdAsync(classID);

                var mapper = _mapper.Map<ClassViewModel>(tmpClass);
                //mapper.CreatedBy = createdAd.FullName;

                var classUsers = await _unitOfWork.ClassUserRepo.GetClassUsers(classID);

                List<ClassUserViewModel> classUser = new List<ClassUserViewModel>();

                foreach (var user in classUsers)
                {
                    var users = await _unitOfWork.UserRepo.GetByIdAsync(user.UserId);
                    var mappers = _mapper.Map<ClassUserViewModel>(users);
                    classUser.Add(mappers);
                }

                mapper.Trainers = classUser;
                classViewModels.Add(mapper);
            }
            return (List<ClassViewModel>)classViewModels;

        }

        public async Task<List<ClassViewModel>> GetOpeningClassListAsync()
        {
            List<ClassViewModel> tempList = await GetClassListAsync();

            List<ClassViewModel> openingClassList = tempList
                .Where(x => x.Status.ToString().Equals("Openning")).ToList();
            if (openingClassList.Count == 0)
            {
                throw new Exception("NO OPENNING CLASS");
            }
            return openingClassList;

        }
        #endregion

        public async Task<String> CreateClassAsync(CreateClassViewModel createClassViewModel)
        {
            bool status = await _unitOfWork.TrainingProgramRepo.getStatusTrainingProgram((int)createClassViewModel.TrainingProgramId);
            if (status == false)
            {
                throw new Exception("Training Program is not avaiable");

            }

            var ClassObj = _mapper.Map<Class>(createClassViewModel);
            ClassObj.Status = 0;
            //ClassObj.CreatedBy = _claimsService.GetCurrentUserId;
            ClassObj.CreatedBy = 20;
            ClassObj.CreatedOn = DateTime.Now;

            _unitOfWork.ClassRepo.AddAttach(ClassObj);
            var isSuccess = await _unitOfWork.SaveChangesAsync() > 0;
            if (!isSuccess) throw new Exception("Save Failed");

            var isAddClassUnitSuccess = await AddClassUnit((int)ClassObj.TrainingProgramId, ClassObj.Id);

            if (!isAddClassUnitSuccess) throw new Exception("Add Class Unit Failed");
            return "Create Class Succeed";
        }
        public async Task<String> AddUserToClassAsync(AddUserToClassViewModel addUserToClassViewModel)
        {
            var tempUser = await _unitOfWork.UserRepo.GetByIdAsync(addUserToClassViewModel.UserId);
            var tempClass = await _unitOfWork.ClassRepo.GetByIdAsync(addUserToClassViewModel.ClassId);
            var tempUserClass = await _unitOfWork.ClassUserRepo.IsExist(addUserToClassViewModel.ClassId, addUserToClassViewModel.UserId);
            if (tempClass == null)
            {
                throw new Exception("Class Not Found");
            }
            if (tempUser == null)
            {
                throw new Exception("User Not Found");
            }

            if (tempUserClass == true)
            {
                throw new Exception("This user is already in this class");
            }
            var classUserObj = _mapper.Map<ClassUsers>(addUserToClassViewModel);
            await _unitOfWork.ClassUserRepo.AddAsync(classUserObj);
            var isSuccess = await _unitOfWork.SaveChangesAsync() > 0;
            if (isSuccess)
            {
                return "Add User To Class Succeed";
            }
            else throw new Exception("Save Failed");
        }
        public async Task<bool> UpdateClassAsync(int classId, UpdateClassViewModel updateClass)
        {
            var classObj = await _unitOfWork.ClassRepo.GetByIdAsync(classId) ?? throw new Exception("Not found class");

            ClassStatus updateStatus;
            ClassLocation updateLocation;
            ClassAttendeeType updateAttendee;

            var isTrueEnumStatusClass = Enum.TryParse<ClassStatus>(updateClass.StatusClass, out updateStatus) ? true : throw new Exception("Error Status Class");
            var isTrueEnumLocation = Enum.TryParse<ClassLocation>(updateClass.Location, out updateLocation) ? true : throw new Exception("Error Location");
            var isTrueEnumAttendee = Enum.TryParse<ClassAttendeeType>(updateClass.Attendee, out updateAttendee) ? true : throw new Exception("Error Attendee");

            classObj = _mapper.Map<Class>(updateClass);

            classObj.Status = updateStatus;
            classObj.Location = updateLocation;
            classObj.AttendeeType = updateAttendee;
            _unitOfWork.ClassRepo.Update(classObj);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
        public async Task<List<int>> GetUnitIdList(int trainingProgramId)
        {
            var tempList = await _unitOfWork.ClassRepo.GetSyllabusId(trainingProgramId);
            return await _unitOfWork.SyllabusRepo.GetUnitId(tempList); ;
        }
        public async Task<bool> AddClassUnit(int trainingProgramId, int classId)
        {
            List<int> tempList = await GetUnitIdList(trainingProgramId);

            foreach (var unitId in tempList)
            {
                var addClassUnitViewModel = new AddClassUnitViewModel()
                {
                    ClassId = classId,
                    UnitId = unitId
                };
                var temp = _mapper.Map<ClassUnitDetail>(addClassUnitViewModel);
                await _unitOfWork.ClassUnitRepo.AddAsync(temp);
                var isSuccess = await _unitOfWork.SaveChangesAsync() > 0;

                if (isSuccess == false) { return false; }
            }
            return true;
        }

        #region Details Class
        public async Task<ClassDetailViewModels> GetClassDetail(int id)
        {
            var classDt = await _unitOfWork.ClassRepo.GetByIdAsync(id);

            List<Admin> admins = new List<Admin>();

            foreach (var item in classDt.ClassUsers)
            {
                if (item.Role.ToString().Equals("Admin"))
                {
                    var admin = _mapper.Map<Admin>(item);
                    admins.Add(admin);
                }
            }

            var result = _mapper.Map<ClassDetailViewModels>(classDt);
            //Get Location list
            HashSet<string> locationDetails = new HashSet<string>();
            foreach (var item in classDt.ClassUnitDetails)
            {

                locationDetails.Add(item.Location.ToString());
            }
            result.LocationDetail = locationDetails;

            //Get List Trainer

            List<Trainer> listTrainer = new List<Trainer>();
            var a = result.Trainer.Distinct(new TrainerComparer());
            foreach (var item in a)
            {
                if (item.TrainerId != 0)
                    listTrainer.Add(item);
            }
            result.Trainer = listTrainer;

            //Get largest day no
            int largestDayNo = 0;
            foreach (var item in classDt.ClassUnitDetails)
            {
                if (item.DayNo > largestDayNo)
                {
                    largestDayNo = (int)item.DayNo;
                }
            }

            //Get list unit detail
            List<ClassTimeFrame> listTemp = new List<ClassTimeFrame>();
            for (int i = 1; i <= largestDayNo; i++)
            {
                string dateTemp = null;
                List<UnitDetail> listUnitDetails = new List<UnitDetail>();
                foreach (var x in classDt.ClassUnitDetails)
                {

                    if (x.DayNo == i)
                    {
                        listUnitDetails.Add(_mapper.Map<UnitDetail>(x));
                        dateTemp = x.OperationDate.ToString();
                    }
                }
                if (dateTemp != null)
                    listTemp.Add(new ClassTimeFrame(dateTemp, listUnitDetails));
            }
            result.ClassTimeFrame = listTemp;
            result.Admin = admins;

            return result;
        }
        #endregion

        #region Clone Class
        public async Task CloneClassAsync(int id)
        {
            if (id <= 0) throw new Exception("Class id must be an integer with value equal or greater than 1");
            var EClass = await _unitOfWork.ClassRepo.GetByIdAsync(id);

            if (EClass == null) throw new Exception("Class not found");

            EClass.CreatedBy = _claimsService.GetCurrentUserId;
            EClass.CreatedOn = _currentTime.GetCurrentTime();

            var model = await _unitOfWork.ClassRepo.CloneAsync(EClass);
            model.Id = 0;
            await _unitOfWork.ClassRepo.AddAsync(model);
            await _unitOfWork.SaveChangesAsync();
            await _trainingProgramService.CloneTrainingProgramAsync((int)model.TrainingProgramId);
        }
        #endregion
    }
}