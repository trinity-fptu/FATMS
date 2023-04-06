using Application.Interfaces;
using Application.IValidators;
using Application.Utils;
using Application.ViewModels.AddLectureViewModels;
using Application.ViewModels.AttendanceViewModels;
using Application.ViewModels.LectureViewModels;
using Application.ViewModels.OutputStandardViewModels;
using Application.ViewModels.SyllabusViewModels;
using Application.ViewModels.UnitViewModels;
using AutoMapper;
using CsvHelper;
using CsvHelper.Configuration;
using Domain.Enums.LectureEnums;
using Domain.Enums.ResponeModelEnums;
using Domain.Enums.SyllabusEnums;
using Domain.Models;
using Domain.Models.Syllabuses;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using OfficeOpenXml;
using System.Globalization;
using System.Reflection;
using System.Text;

namespace Application.Services
{
    public class SyllabusService : ISyllabusService
    {
        //can remove anything if dont use
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IClaimsService _claimsService;
        private readonly ICurrentTime _currentTime;
        private readonly IConfiguration _configuration;
        private readonly ILectureValidator _validator;

        public SyllabusService(IUnitOfWork unitOfWork, IMapper mapper, IClaimsService claimsService, ICurrentTime currentTime, IConfiguration configuration, ILectureValidator validator)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _claimsService = claimsService;
            _currentTime = currentTime;
            _configuration = configuration;
            _validator = validator;
        }

        public async Task<ValidationResult> ValidateAddLectureAsync(AddLectureViewModel lecture)
        {
            return await _validator.AddLectureModel.ValidateAsync(lecture);
        }

        #region Service GetSyllabusDetail

        // Get a detailed syllabus by mapping the syllabus find by id to a syllabus detail model or null if syllabus does not exist
        // Return: the detailed syllabus
        public async Task<SyllabusDetailModel> GetSyllabusDetailByIdAsync(int syllabusId)
        {
            var syllabus = await _unitOfWork.SyllabusRepo.GetSyllabusDetailByIdAsync(syllabusId);
            if (syllabus == null)
            {
                throw new NullReferenceException("Syllabus not found");
            }

            var syllabusDetail = _mapper.Map<SyllabusDetailModel>(syllabus);
            var syllabusLectures = syllabusDetail.Units.SelectMany(x => x.Lectures).ToList();
            //syllabusDetail.Duration.Days = (syllabusDetail.Units.Count() == 0) ? 0 : syllabusDetail.Units.Max(x => x.Session);
            //syllabusDetail.Duration.Hours = Math.Round(Convert.ToDecimal(TimeSpan.FromMinutes(syllabusLectures.Sum(x => x.Duration)).TotalHours), 1);
            for (int i = 1; i <= syllabusDetail.Duration.Days; i++)
            {
                syllabusDetail.Days.Add(new Content
                {
                    Day = i,
                    Units = new List<UnitDetailModel>()
                });
            }

            foreach (var unit in syllabusDetail.Units)
            {
                foreach (var item in syllabusDetail.Days)
                {
                    if (unit.Session == item.Day)
                    {
                        item.Units.Add(unit);
                    }
                }

                unit.Duration = unit.Lectures.Sum(x => x.Duration);
            }
            if (syllabusLectures.Count > 0)
            {
                syllabusDetail.TimeAllocation = SyllabusTACal(syllabusLectures);
            }
            else
            {
                syllabusDetail.TimeAllocation = new SyllabusTimeAllocation();
            }

            return syllabusDetail;
        }

        #endregion Service GetSyllabusDetail

        #region Service GetTotalTimeForDeliveryType

        // Calculate total time of lectures of a delivery type
        // Return: total time of lectures of a delivery type
        public float GetTotalTimeForType(List<LectureDetailModel> syllabusLectures, LectureDeliveryType deliveryType)
        {
            int deliveryTypeTotalTime = 0;
            foreach (var item in syllabusLectures)
            {
                if (item.DeliveryType.ToString().Equals(deliveryType.ToString()))
                {
                    deliveryTypeTotalTime += item.Duration;
                }
            }

            return deliveryTypeTotalTime;
        }

        #endregion Service GetTotalTimeForDeliveryType

        #region Service CalculateTimeAllocationOfSyllabusLectures

        // Calculate time allocation of different lecture delivery type of a syllabus from lectures of that syllabus
        // Return: a time allocation object contains the time allocation for different delivery types
        public SyllabusTimeAllocation SyllabusTACal(List<LectureDetailModel> syllabusLectures)
        {
            float assignmentLabTotal = GetTotalTimeForType(syllabusLectures, LectureDeliveryType.AssignmentLab);
            float conceptLectureTotal = GetTotalTimeForType(syllabusLectures, LectureDeliveryType.ConceptLecture);
            float guideReviewTotal = GetTotalTimeForType(syllabusLectures, LectureDeliveryType.GuideReview);
            float testQuizTotal = GetTotalTimeForType(syllabusLectures, LectureDeliveryType.TestQuiz);
            float examTotal = GetTotalTimeForType(syllabusLectures, LectureDeliveryType.Exam);
            float totalTime = assignmentLabTotal + conceptLectureTotal + guideReviewTotal + testQuizTotal + examTotal;
            SyllabusTimeAllocation taObject = new SyllabusTimeAllocation
            {
                AssignmentLab = (assignmentLabTotal * 100) / totalTime,
                ConceptLecture = (conceptLectureTotal * 100) / totalTime,
                GuideReview = (guideReviewTotal * 100) / totalTime,
                TestQuiz = (testQuizTotal * 100) / totalTime,
                Exam = (examTotal * 100) / totalTime
            };
            return taObject;
        }

        #endregion Service CalculateTimeAllocationOfSyllabusLectures

        #region Service CloneSyllabus

        public async Task CloneSyllabusAsync(int syllabusId)
        {
            var syllabus = await _unitOfWork.SyllabusRepo.GetSyllabusDetailByIdAsync(syllabusId);
            if (syllabus == null)
            {
                throw new NullReferenceException("Syllabus not found");
            }

            var cloneSyllabusModel = _mapper.Map<CloneSyllabusViewModel>(syllabus);
            cloneSyllabusModel.CreatedBy = _claimsService.GetCurrentUserId;
            cloneSyllabusModel.CreatedOn = _currentTime.GetCurrentTime();
            cloneSyllabusModel.LastModifiedBy = _claimsService.GetCurrentUserId;
            cloneSyllabusModel.LastModifiedOn = _currentTime.GetCurrentTime();
            cloneSyllabusModel.Version += 1;
            var cloneSyllabus = _mapper.Map<Syllabus>(cloneSyllabusModel);
            await _unitOfWork.SyllabusRepo.AddAsync(cloneSyllabus);
            await _unitOfWork.SaveChangesAsync();
        }

        #endregion

        #region Update

        //action
        public async Task<SyllabusViewModel> UpdateSyllabusAsync(UpdateSyllabusViewModel updateSyllabusViewModel,
            int syllabusId)
        {
            var syllabus = await _unitOfWork.SyllabusRepo.GetByIdAsync(syllabusId);
            if (syllabus == null)
            {
                throw new NullReferenceException("Syllabus not found");
            }
            
            _mapper.Map(updateSyllabusViewModel, syllabus);
            
            syllabus.LastModifiedBy = _claimsService.GetCurrentUserId;
            syllabus.LastModifiedOn = _currentTime.GetCurrentTime();
            syllabus.Version ++;

            var syllabusViewModel = _mapper.Map<SyllabusViewModel>(syllabus);
            
            syllabusViewModel.OutputStandardCode = await _unitOfWork.SyllabusRepo.GetOutputStandardCode();

            _unitOfWork.SyllabusRepo.Update(syllabus);

            var check = await _unitOfWork.SaveChangesAsync() > 0;
            if (check)
            {
                return syllabusViewModel;
            }

            throw new ArgumentException("Update failed");
        }
        
        public async Task<UpdateUnitInSyllabuViewModel> UpdateUnitInSyllabusAsync(UnitUpdateViewModel unitUpdateViewModel,
            int syllabusId)
        {
            var syllabus = await _unitOfWork.SyllabusRepo.GetByIdAsync(syllabusId);
            if (syllabus == null)
            {
                throw new NullReferenceException("Syllabus not found");
            }

            var unit = syllabus.Units.FirstOrDefault(x => x.Id == unitUpdateViewModel.Id);
            if (unit == null)
            {
                throw new NullReferenceException("Unit not found");
            }

            var updateUnit = _mapper.Map(unitUpdateViewModel, unit);

            syllabus.LastModifiedBy = _claimsService.GetCurrentUserId;
            syllabus.LastModifiedOn = _currentTime.GetCurrentTime();

            _unitOfWork.UnitRepo.Update(updateUnit);

            var check = await _unitOfWork.SaveChangesAsync() > 0;
            if (check)
            {
                return _mapper.Map<UpdateUnitInSyllabuViewModel>(syllabus);
            }

            throw new ArgumentException("Update failed");
        }
        
        public async Task<UpdateLectureInSyllabusViewModel> UpdateLectureInSyllabusAsync(UpdateLectureViewModel updateLectureViewModel,
            int syllabusId)
        {
            var syllabus = await _unitOfWork.SyllabusRepo.GetByIdAsync(syllabusId);
            if (syllabus == null)
            {
                throw new NullReferenceException("Syllabus not found");
            }
            
            var lecture = syllabus.Units.SelectMany(x => x.Lectures).FirstOrDefault(x => x.Id == updateLectureViewModel.Id);
            if (lecture == null)
            {
                throw new NullReferenceException("Lecture not found");
            }

            var updateLecture = _mapper.Map(updateLectureViewModel, lecture);

            syllabus.LastModifiedBy = _claimsService.GetCurrentUserId;
            syllabus.LastModifiedOn = _currentTime.GetCurrentTime();

            _unitOfWork.LectureRepo.Update(updateLecture);

            var check = await _unitOfWork.SaveChangesAsync() > 0;
            if (check)
            {
                return _mapper.Map<UpdateLectureInSyllabusViewModel>(syllabus);
            }

            throw new ArgumentException("Update failed");
        }

        #endregion Update

        #region Service DeleteSyllabus

        //action
        public async Task<SyllabusViewModel> DeleteSyllabusAsync(int id)
        {
            if (id <= 0)
            {
                string msg = "Syllabus id cannot be less than 1.";

                // Log error message to console
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid parameters: " + msg);

                // throw argument exception with error message
                throw new ArgumentException(msg);
            }

            var syllabus = await _unitOfWork.SyllabusRepo.GetByIdAsync(id);
            if (syllabus == null)
            {
                throw new ArgumentException("Syllabus id not found!!");
            }

            syllabus.isDeleted = true;
            syllabus.LastModifiedBy = _claimsService.GetCurrentUserId;
            syllabus.LastModifiedOn = _currentTime.GetCurrentTime();
            _unitOfWork.SyllabusRepo.Update(syllabus);
            var check = await _unitOfWork.SaveChangesAsync() > 0;
            string success = "Delete Successful!!";
            var syllabusDetails = _mapper.Map<SyllabusViewModel>(syllabus);
            if (check)
            {
                return syllabusDetails;
            }

            return null;
        }

        #endregion

        #region Service AddLecture

        public async Task<LectureViewModel> AddLectureAsync(AddLectureViewModel model)
        {
            Lecture lec = _mapper.Map<Lecture>(model);
            _unitOfWork.LectureRepo.AddAsync(lec);
            //int checkdelivery = (int)model.DeliveryType;
            //if (checkdelivery > 4 || checkdelivery < 0)
            //{
            //    throw new Exception("Lesson type input must be from 0 to 1 and Delivery type input must be from 0 to 4");
            //}
            //var outputstandard=await _unitOfWork.OutputStandardRepo.GetByIdAsync(x=>x.);

            var check = await _unitOfWork.SaveChangesAsync() > 0;
            LectureViewModel view = _mapper.Map<LectureViewModel>(lec);
            //view.
            if (check)
            {
                return view;
            }

            return null;
        }

        #endregion

        #region Service AddLectureToUnit

        public async Task<LectureViewModel> AddLectureToUnitAsync(int lecture_id, int unit_id)
        {
            if (unit_id <= 0 || lecture_id <= 0)
            {
                string msg = "Unit id or Lecture id cannot be less than 1.";

                // Log error message to console
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid parameters: " + msg);
                // throw argument exception with error message
                throw new Exception(msg);
            }

            var unit = await _unitOfWork.UnitRepo.GetByIdAsync(unit_id);
            if (unit == null)
            {
                string msg = "Unit id not found";
                throw new Exception(msg);
            }
            else
            {
                var lec = await _unitOfWork.LectureRepo.GetByIdAsync(lecture_id);
                if (lec == null)
                {
                    string msg = "Lecture id not found";
                    throw new Exception(msg);
                }

                if (lec != null)
                {
                    lec.UnitId = unit_id;
                    _unitOfWork.LectureRepo.Update(lec);
                }

                var result = _mapper.Map<LectureViewModel>(lec);
                var check = await _unitOfWork.SaveChangesAsync() > 0;
                if (check)
                {
                    return result;
                }

                throw new Exception("Add Error");
            }
        }

        #endregion

        #region Service GetSyllabus

        public async Task<List<SyllabusViewModel>> GetSyllabusAsync()
        {
            var syllabuses = await _unitOfWork.SyllabusRepo.GetAllAsync(x => x
                .Include(x => x.CreatedAdmin)
                .Include(x => x.ModifiedAdmin));
            var validSyllabuses = syllabuses.FindAll(x => x.isDeleted == false);
            var syllabusDetails = _mapper.Map<List<SyllabusViewModel>>(validSyllabuses);

            for (int i = 0; i < syllabusDetails.Count; i++)
            {
                var unit = await _unitOfWork.UnitRepo.GetUnitsBySyllabusId(syllabusDetails[i].Id);
                if (unit.Count > 0)
                {
                    syllabusDetails[i].OutputStandardCode = unit.SelectMany(u => u.Lectures).Where(l => l.OutputStandard != null)
                        .Select(l => l.OutputStandard.Code).Distinct().Take(3).ToList();

                    var durationInMinutes = unit.SelectMany(u => u.Lectures)?.Sum(l => l.Duration) ?? 0;
                    syllabusDetails[i].TimeDuration = durationInMinutes;
                }
            }

            return syllabusDetails;
        }

        #endregion


        public async Task<List<OutputStandardViewModel>> GetOutputStandardsAsync()
        {
            var outputStandards = await _unitOfWork.OutputStandardRepo.GetAllAsync();
            var outputStandardsModels = _mapper.Map<List<OutputStandardViewModel>>(outputStandards);

            return outputStandardsModels;

        }
        #region Service AddSyllabus

        public async Task<SyllabusViewModel> AddSyllabusAsync(AddSyllabusViewModel syllabusViewModel)
        {
            var syllabus = _mapper.Map<Syllabus>(syllabusViewModel);
            syllabus.Version = 1;
            syllabus.CreatedOn = _currentTime.GetCurrentTime();
            syllabus.LastModifiedOn = _currentTime.GetCurrentTime();
            syllabus.CreatedBy = _claimsService.GetCurrentUserId;
            syllabus.LastModifiedBy = _claimsService.GetCurrentUserId;
            syllabus.isDeleted = false;

            // Iterate through the days and units, adding them to the syllabus
            syllabus.Units = new List<Unit>();
            int index = 0;
            foreach (var day in syllabusViewModel.Days)
            {
                index++;
                foreach (var unit in day.Units)
                {
                    var mappedUnit = _mapper.Map<Unit>(unit);
                    mappedUnit.Session = index;
                    syllabus.Units.Add(mappedUnit);
                }

            }

            await _unitOfWork.SyllabusRepo.AddAsync(syllabus);
            var check = await _unitOfWork.SaveChangesAsync() > 0;
            if (check)
            {
                var result = _mapper.Map<SyllabusViewModel>(syllabus);
                return result;
            }

            return null;
        }

        #endregion

        #region Service GetSyllabusByName

        public async Task<List<SyllabusViewModel>> GetSyllabusByNameAsync(string? searchString)
        {
            var syllabuses = await GetSyllabusAsync();
            syllabuses.ForEach(x => x.Name += " v" + x.Version);
            if (searchString.IsNullOrEmpty())
            {
                return syllabuses;
            }

            var result = syllabuses.Where(t => t.Name.ToLower().Contains(searchString.ToLower().Trim())).ToList();
            return result;
        }

        #endregion

        #region Service GetSyllabusByDateRange

        public async Task<List<SyllabusViewModel>> GetSyllabusByDateRangeAsync(DateTime[] fromToDate)
        {
            var syllabuses = await GetSyllabusAsync();
            if (fromToDate.Length == 0)
            {
                return syllabuses;
            }

            //DateTime fromtDate = DateTime.ParseExact(fromToDate[0], "dd/MM/yyyy", CultureInfo.InvariantCulture);
            //DateTime toDate = DateTime.ParseExact(fromToDate[1], "dd/MM/yyyy", CultureInfo.InvariantCulture);
            var result = new List<SyllabusViewModel>();

            foreach (var item in syllabuses)
            {
                DateTime createdDate = DateTime.ParseExact(item.CreatedOn, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                if (createdDate.Date >= fromToDate[0].Date && createdDate.Date <= fromToDate[1].Date)
                {
                    result.Add(item);
                }
            }

            return result;
        }

        #endregion

        #region ExportCsvFile

        public byte[] ExportCsvFile(string columnSeperator)
        {
            string[] columnNames = new string[] { "Name", "Code", "Level" ,
                "Attendee_Number", "Technical_Requirements", "Course_Objectives",
                "Training_Delivery_Principle", "Quiz", "Assignment", "Final", "Final_Theory",
                "Final_Practical", "GPA" };
            string csvFile = string.Empty;

            foreach (var column in columnNames)
            {
                csvFile += column + columnSeperator;
            }

            csvFile += "\n#Example Template: Import with this format.\n";
            csvFile += "#Name, #Code, #All Levels (Basic...), #1, " +
                       "#Technical Requirement, #Course Object, #Training Delivery Principle (Allow Empty)," +
                       "#1, #1, #1, #1, #1, #1.1";
            csvFile.Replace(",", columnSeperator);

            csvFile += "\r\n";

            return Encoding.UTF8.GetBytes(csvFile);
        }

        #endregion

        #region ImportCsvFile

        public async Task<List<SyllabusViewModel>> ImportCsvFileAsync(IFormFile formfile,
            bool isScanCode, bool isScanName,
            DuplicateHandle duplicateHandle)
        {
            //List Syllabus Read From Csv File
            var listSyllabusFromCsv = new List<AddSyllabusViewModel>();
            var currentUser = _claimsService.GetCurrentUserId;

            #region ReadCsvFile

            var fileName = Guid.NewGuid().ToString() + ".csv";
            var binPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var filePath = Path.Combine(binPath!, fileName);

            using (FileStream fs = File.Create(filePath))
            {
                await formfile.CopyToAsync(fs);
            }

            using (var streamReader = new StreamReader(filePath))
            {
                var csvConfig = new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    HasHeaderRecord = true,
                    Comment = '#',
                    AllowComments = true,
                };

                var csv = new CsvReader(streamReader, csvConfig);

                try
                {
                    var syllabusRecord = csv.GetRecords<dynamic>();

                    foreach (var record in syllabusRecord)
                    {
                        if (string.IsNullOrWhiteSpace(record.Name))
                        {
                            break;
                        }

                        string level = record.Level;

                        var createdSyllabus = new AddSyllabusViewModel
                        {
                            Name = record.Name,
                            Code = record.Code,
                            Version = 1,
                            Level = (SyllabusLevel)Enum.Parse(typeof(SyllabusLevel), level.RegenerateStringFormat()),
                            AttendeeNumber = int.Parse(record.Attendee_Number),
                            TechnicalRequirements = record.Technical_Requirements,
                            CourseObjectives = record.Course_Objectives,
                            TrainingDeliveryPrinciple = record.Training_Delivery_Principle,
                            QuizCriteria = float.Parse(record.Quiz),
                            AssignmentCriteria = float.Parse(record.Assignment),
                            FinalCriteria = float.Parse(record.Final),
                            FinalTheoryCriteria = float.Parse(record.Final_Theory),
                            FinalPracticalCriteria = float.Parse(record.Final_Practical),
                            PassingGPA = float.Parse(record.GPA),
                            isActive = true,
                        };

                        if (!isScanCode && !isScanName)
                        {
                            var isExistSyllabus =
                                (await _unitOfWork.SyllabusRepo.GetLatestByCodeAsync(createdSyllabus.Code) != null)
                                || (await _unitOfWork.SyllabusRepo.GetLatestByNameAsync(createdSyllabus.Name) != null);

                            if (isExistSyllabus)
                            {
                                throw new Exception("This syllabus name or code is already exist.");
                            }
                        }
                        else if (isScanCode && !isScanName)
                        {
                            var isExistSyllabus =
                                (await _unitOfWork.SyllabusRepo.GetLatestByNameAsync(createdSyllabus.Name) != null);
                            if (isExistSyllabus)
                            {
                                throw new Exception(
                                    "This syllabus name is already exist. Please choose 'Scan Name' in order to handle it.");
                            }
                        }

                        listSyllabusFromCsv.Add(createdSyllabus);
                    }
                }
                catch (CsvHelperException)
                {
                    streamReader.Dispose();
                    File.Delete(filePath);
                    throw new Exception($"Error at line {csv.Context.Parser.Row}, " +
                                        $"Column: {csv.CurrentIndex}, " +
                                        $"Column Name: {csv.HeaderRecord?[csv.CurrentIndex]}");
                }
                catch (Exception ex)
                {
                    streamReader.Dispose();
                    File.Delete(filePath);
                    throw new Exception($"Error at line {csv.Context.Parser.Row}: " + ex.Message);
                }
            }

            File.Delete(filePath);

            #endregion

            #region ImportToDb

            //List Add Syllabus Array
            //first element is list create syllabus
            //second element is list update syllabus for 'Replace' option.
            var arrayListSyllabus = await ImportScanningHandle(listSyllabusFromCsv,
                isScanCode, isScanName, duplicateHandle);

            //List Update After Scanning
            var listUpdateSyllabus = new List<Syllabus>();

            if (arrayListSyllabus[1].Count != 0)
            {
                foreach (var syllabus in arrayListSyllabus[1])
                {
                    var tempSyllabus = new Syllabus();

                    if (isScanCode)
                    {
                        tempSyllabus = await _unitOfWork.SyllabusRepo.GetLatestByCodeAsync(syllabus.Code) ??
                                       new Syllabus();
                    }

                    if (isScanName)
                    {
                        tempSyllabus = await _unitOfWork.SyllabusRepo.GetLatestByNameAsync(syllabus.Name) ??
                                       new Syllabus();
                    }

                    _mapper.Map(syllabus, tempSyllabus);
                    tempSyllabus.LastModifiedBy = currentUser;
                    tempSyllabus.LastModifiedOn = _currentTime.GetCurrentTime();
                    listUpdateSyllabus.Add(tempSyllabus);
                }

                _unitOfWork.SyllabusRepo.UpdateRange(listUpdateSyllabus);
            }

            //List Add After Scanning
            var listCreate = new List<Syllabus>();

            foreach (var syllabus in arrayListSyllabus[0])
            {
                var tempSyllabus = _mapper.Map<Syllabus>(syllabus);
                tempSyllabus.CreatedBy = tempSyllabus.LastModifiedBy = currentUser;
                tempSyllabus.CreatedOn = tempSyllabus.LastModifiedOn = _currentTime.GetCurrentTime();

                listCreate.Add(tempSyllabus);
            }

            await _unitOfWork.SyllabusRepo.AddRangeAsync(listCreate);

            if (await _unitOfWork.SaveChangesAsync() >= listCreate.Count)
            {
                return _mapper.Map<List<SyllabusViewModel>>(listCreate);
            }

            #endregion

            return new List<SyllabusViewModel>();
        }

        #endregion

        #region ImportScanningHandle

        private async Task<List<AddSyllabusViewModel>[]> ImportScanningHandle(List<AddSyllabusViewModel> listSyllabus,
            bool isScanCode, bool isScanName,
            DuplicateHandle duplicateHandle)
        {
            var updateList = new List<AddSyllabusViewModel>();

            switch (duplicateHandle)
            {
                case DuplicateHandle.Allow:
                    if (isScanCode)
                    {
                        foreach (var syllabus in listSyllabus)
                        {
                            var tempSyllabus = await _unitOfWork.SyllabusRepo.GetLatestByCodeAsync(syllabus.Code);
                            if (tempSyllabus != null)
                            {
                                syllabus.Version = tempSyllabus.Version + 1;
                            }
                        }
                    }

                    if (isScanName)
                    {
                        foreach (var syllabus in listSyllabus)
                        {
                            var tempSyllabus = await _unitOfWork.SyllabusRepo.GetLatestByNameAsync(syllabus.Name);
                            if (tempSyllabus != null)
                            {
                                syllabus.Version = tempSyllabus.Version + 1;
                            }
                        }
                    }

                    break;
                case DuplicateHandle.Replace:
                    if (isScanCode)
                    {
                        foreach (var syllabus in listSyllabus)
                        {
                            var tempSyllabus = await _unitOfWork.SyllabusRepo.GetLatestByCodeAsync(syllabus.Code);
                            if (tempSyllabus != null)
                            {
                                syllabus.Version = tempSyllabus.Version;
                                updateList.Add(syllabus);
                            }
                        }
                    }

                    if (isScanName)
                    {
                        foreach (var syllabus in listSyllabus)
                        {
                            var tempSyllabus = await _unitOfWork.SyllabusRepo.GetLatestByNameAsync(syllabus.Name);
                            if (tempSyllabus != null)
                            {
                                syllabus.Version = tempSyllabus.Version;
                                updateList.Add(syllabus);
                            }
                        }
                    }

                    listSyllabus = listSyllabus.Except(updateList).ToList();
                    break;
                case DuplicateHandle.Skip:
                    if (isScanCode)
                    {
                        foreach (var syllabus in listSyllabus)
                        {
                            var isExistSyllabus =
                                (await _unitOfWork.SyllabusRepo.GetLatestByCodeAsync(syllabus.Code)) != null;
                            if (isExistSyllabus)
                            {
                                updateList.Add(syllabus);
                            }
                        }
                    }

                    if (isScanName)
                    {
                        foreach (var syllabus in listSyllabus)
                        {
                            var isExistSyllabus =
                                (await _unitOfWork.SyllabusRepo.GetLatestByNameAsync(syllabus.Name)) != null;
                            if (isExistSyllabus)
                            {
                                updateList.Add(syllabus);
                            }
                        }
                    }

                    listSyllabus = listSyllabus.Except(updateList).ToList();
                    updateList.Clear();
                    break;
            }

            return new List<AddSyllabusViewModel>[] { listSyllabus, updateList };
        }

        #endregion

        #region GetListSyllabusLevel
        private List<string> GetListSyllabuseLevel()
        {
            return Enum.GetValues(typeof(SyllabusLevel))
                .Cast<SyllabusLevel>()
                .Select(x => x.ToString().GenerateStringFormat())
                .ToList();
        }
        #endregion

        #region ExportExcelFile
        public async Task<Stream> ExportExcelFileAsync()
        {
            string[] columnNames = new string[] { "Name", "Code", "Level" ,
                "Attendee Number", "Technical Requirements", "Course Objectives",
                "Training Delivery Principle", "Quiz", "Assignment", "Final", "Final Theory",
                "Final Practical", "GPA" };
            string header = string.Empty;
            var syllabusLevel = GetListSyllabuseLevel();

            foreach (var column in columnNames)
            {
                header += column + ",";
            }

            var stream = new MemoryStream();

            using (var package = new ExcelPackage(stream))
            {
                var worksheet = package.Workbook.Worksheets.Add("Syllabus");
                worksheet.Cells.LoadFromText(header);

                //Create dropdown list option for Syllabus Level
                var levelOptions = worksheet.DataValidations.AddListValidation("C2:C1000");
                levelOptions.ShowInputMessage = levelOptions.ShowErrorMessage = true;
                foreach (var level in syllabusLevel)
                {
                    levelOptions.Formula.Values.Add(level);
                }

                await package.SaveAsync();
            }

            stream.Position = 0;

            return stream;
        }
        #endregion

        #region ImportExcelFile
        public async Task<List<SyllabusViewModel>> ImportExcelFileAsync(IFormFile formfile,
            bool isScanCode, bool isScanName,
            DuplicateHandle duplicateHandle)
        {
            //List Syllabus Read From Csv File
            var listSyllabusFromFile = new List<AddSyllabusViewModel>();
            var currentUser = _claimsService.GetCurrentUserId;

            #region ReadCsvFile
            using (var stream = new MemoryStream())
            {
                await formfile.CopyToAsync(stream);

                using (var excelPackage = new ExcelPackage(stream))
                {
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets[0];
                    var rowCount = worksheet.Dimension.Rows;

                    for (int row = 2; row <= rowCount; row++)
                    {
                        var col = 1;

                        try
                        {
                            var createdSyllabus = new AddSyllabusViewModel
                            {
                                Name = worksheet.Cells[row, col++].Value.ToString()!.Trim(),
                                Code = worksheet.Cells[row, col++].Value.ToString()!.Trim(),
                                Version = 1,

                                Level = (SyllabusLevel)Enum.Parse(typeof(SyllabusLevel),
                                worksheet.Cells[row, col++].Value.ToString()!
                                .Trim()
                                .RegenerateStringFormat()),

                                AttendeeNumber = int.Parse(worksheet.Cells[row, col++].Value.ToString()!.Trim()),
                                TechnicalRequirements = worksheet.Cells[row, col++].Value.ToString()!.Trim(),
                                CourseObjectives = worksheet.Cells[row, col++].Value.ToString()!.Trim(),

                                TrainingDeliveryPrinciple = worksheet.Cells[row, col++].Value != null ?
                                worksheet.Cells[row, col-1].Value.ToString()!.Trim() : string.Empty,

                                QuizCriteria = float.Parse(worksheet.Cells[row, col++].Value.ToString()!.Trim()),
                                AssignmentCriteria = float.Parse(worksheet.Cells[row, col++].Value.ToString()!.Trim()),
                                FinalCriteria = float.Parse(worksheet.Cells[row, col++].Value.ToString()!.Trim()),
                                FinalTheoryCriteria = float.Parse(worksheet.Cells[row, col++].Value.ToString()!.Trim()),
                                FinalPracticalCriteria = float.Parse(worksheet.Cells[row, col++].Value.ToString()!.Trim()),
                                PassingGPA = float.Parse(worksheet.Cells[row, col++].Value.ToString()!.Trim()),
                                isActive = true,
                            };

                            if (!isScanCode && !isScanName)
                            {
                                var isExistSyllabusCode = await _unitOfWork.SyllabusRepo.GetLatestByCodeAsync(createdSyllabus.Code) != null;
                                var isExistSyllabusName = await _unitOfWork.SyllabusRepo.GetLatestByNameAsync(createdSyllabus.Name) != null;

                                if (isExistSyllabusCode || isExistSyllabusName)
                                {
                                    col = isExistSyllabusName ? 2 : 3;
                                    throw new Exception("This syllabus name or code is already exist.");
                                }
                            }
                            else if (isScanCode && !isScanName)
                            {
                                var isExistSyllabus = (await _unitOfWork.SyllabusRepo.GetLatestByNameAsync(createdSyllabus.Name) != null);
                                if (isExistSyllabus)
                                {
                                    col = 2;
                                    throw new Exception("This syllabus name is already exist. Please choose 'Scan Name' in order to handle it.");
                                }
                            }

                            listSyllabusFromFile.Add(createdSyllabus);
                        }
                        catch (Exception ex)
                        {
                            await stream.DisposeAsync();
                            throw new Exception($"Error at line {row}, Column Name: {worksheet.Cells[1, col - 1].Value}, Error: {ex.Message}");
                        }
                    }
                }
            }
            #endregion

            #region ImportToDb
            //List Add Syllabus Array
            //first element is list create syllabus
            //second element is list update syllabus for 'Replace' option.
            var arrayListSyllabus = await ImportScanningHandle(listSyllabusFromFile,
                isScanCode, isScanName, duplicateHandle);

            //List Update After Scanning
            var listUpdatSyllabus = new List<Syllabus>();

            if (arrayListSyllabus[1].Count != 0)
            {
                foreach (var syllabus in arrayListSyllabus[1])
                {
                    var tempSyllabus = new Syllabus();

                    if (isScanCode)
                    {
                        tempSyllabus = await _unitOfWork.SyllabusRepo.GetLatestByCodeAsync(syllabus.Code) ?? new Syllabus();
                    }

                    if (isScanName)
                    {
                        tempSyllabus = await _unitOfWork.SyllabusRepo.GetLatestByNameAsync(syllabus.Name) ?? new Syllabus();
                    }

                    _mapper.Map(syllabus, tempSyllabus);
                    tempSyllabus.LastModifiedBy = currentUser;
                    tempSyllabus.LastModifiedOn = _currentTime.GetCurrentTime();
                    listUpdatSyllabus.Add(tempSyllabus);
                }

                _unitOfWork.SyllabusRepo.UpdateRange(listUpdatSyllabus);
            }

            //List Add After Scanning
            var listCreate = new List<Syllabus>();

            foreach (var syllabus in arrayListSyllabus[0])
            {
                var tempSyllabus = _mapper.Map<Syllabus>(syllabus);
                tempSyllabus.CreatedBy = tempSyllabus.LastModifiedBy = currentUser;
                tempSyllabus.CreatedOn = tempSyllabus.LastModifiedOn = _currentTime.GetCurrentTime();

                listCreate.Add(tempSyllabus);
            }

            await _unitOfWork.SyllabusRepo.AddRangeAsync(listCreate);

            if (await _unitOfWork.SaveChangesAsync() >= listCreate.Count)
            {
                return _mapper.Map<List<SyllabusViewModel>>(listCreate);
            }

            #endregion

            return new List<SyllabusViewModel>();
        }
        #endregion
    }
}