using Application.Interfaces;
using Application.Utils;
using Application.ViewModels.SyllabusViewModels;
using Application.ViewModels.TrainingProgramViewModels;
using Application.ViewModels.UserViewModels;
using AutoMapper;
using CsvHelper;
using CsvHelper.Configuration;
using Domain.Enums.ResponeModelEnums;
using Domain.Models;
using Domain.Models.Syllabuses;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using OfficeOpenXml;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;

namespace Application.Services
{
    public class TrainingProgramService : ITrainingProgramService
    {
        //can remove anything if dont use
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICurrentTime _currentTime;
        private readonly IConfiguration _configuration;
        private readonly IClaimsService _claimsService;
        private readonly ISyllabusService _syllabusService;

        public TrainingProgramService(IUnitOfWork unitOfWork, ICurrentTime currentTime, IConfiguration configuration, IMapper mapper, IClaimsService claimsService, ISyllabusService syllabusService)
        {
            _unitOfWork = unitOfWork;
            _currentTime = currentTime;
            _configuration = configuration;
            _mapper = mapper;
            _claimsService = claimsService;
            _syllabusService = syllabusService;
        }

        #region GetTrainingProgramById
        public async Task<TrainingProgramViewModels> GetByIdAsync(int id)
        {
            if (id <= 0)
            {
                string msg = "Training Program id cannot be less than 1.";

                // Log error message to console
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid parameters: " + msg);

                // throw argument exception with error message
                throw new ArgumentException(msg);
            }

            var getTrainingById = await _unitOfWork.TrainingProgramRepo.GetByIdAsync(id);
            var result = _mapper.Map<TrainingProgramViewModels>(getTrainingById);

            return result;

        }
        #endregion

        #region GetTrainingProgramAsync
        public async Task<List<TrainingProgramViewModels>> GetAllAsync()
        {
            // Get paginated user entities list
            var listTrainingProgram = await _unitOfWork.TrainingProgramRepo.GetAllAsync();

            // Map user entities to user list model
            var result = _mapper.Map<List<TrainingProgramViewModels>>(listTrainingProgram);

            if (result.IsNullOrEmpty())
            {
                string msg = "No Item";
                throw new Exception(msg);
            }

            return result;
        }
        #endregion

        #region GetAllTrainingProgramIsActive
        public async Task<List<TrainingProgramViewModels>> GetAllTrainingProgramIsActiveAsync()
        {
            // Get paginated user entities list
            var listTrainingProgram = await _unitOfWork.TrainingProgramRepo.GetAllAsync();
            var listTrainingProgramFalse = listTrainingProgram.FindAll(x => x.IsActive);

            // Map user entities to user list model
            var result = _mapper.Map<List<TrainingProgramViewModels>>(listTrainingProgramFalse);

            if (result.IsNullOrEmpty())
            {
                string msg = "No Item";
                throw new Exception(msg);
            }

            return result;
        }
        #endregion

        #region UpdateTrainingProgram
        public async Task<bool> UpdateTrainingProgramsAsync(int id, UpdateTrainingProgramViewModel updatetrainingProgramViewModels)
        {

            //Get TrainingProgram by ID
            var trainingProgramObj = await _unitOfWork.TrainingProgramRepo.GetByIdAsync(id);

            if (trainingProgramObj == null) throw new Exception("Training program is null");

            //Get Syllabus
            List<Syllabus> syllabus = new List<Syllabus>();
            foreach (var item in updatetrainingProgramViewModels.SyllabusId)
            {
                syllabus.Add(await _unitOfWork.SyllabusRepo.GetSyllabusByID(item));
            }

            //Map updateTrainingProgramViewModel to TrainingProgram
            _mapper.Map(updatetrainingProgramViewModels, trainingProgramObj);

            //Update TrainingProgram          
            trainingProgramObj.LastModify = _currentTime.GetCurrentTime();
            trainingProgramObj.LastModifyBy = _claimsService.GetCurrentUserId;
            trainingProgramObj.Syllabuses = syllabus;

            _unitOfWork.TrainingProgramRepo.Update(trainingProgramObj);

            //Save change
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
        #endregion UpdateTrainingProgram

        #region DeleteTrainingProgramBy
        public async Task<bool> DeleteTrainingProgramsAsync(int id)
        {
            var trainingProgramInDb = await _unitOfWork.TrainingProgramRepo.GetByIdAsync(id);
            if (trainingProgramInDb == null)
            {
                String mess = "Don't found this Training Program";
                throw new Exception(mess);
            }
            else
            {
                trainingProgramInDb.isDeleted = true;
                await _unitOfWork.SaveChangesAsync();
                return true;
            }
        }
        #endregion

        #region Create new Training Program
        public async Task<TrainingProgramViewModels> CreateTrainingProgramAsync(CreateTrainingProgramViewModels trainingProgramViewModels)
        {
            //check Duplicate training program name
            var checkName = await _unitOfWork.TrainingProgramRepo.GetAllAsync();
            if (checkName?.FirstOrDefault(x => x.Name.Equals(trainingProgramViewModels.Name, StringComparison.OrdinalIgnoreCase)) != null)
                throw new ArgumentNullException();

            var trainingProgramObj = _mapper.Map<TrainingProgram>(trainingProgramViewModels);

            ICollection<Syllabus> syllabuss = new List<Syllabus>();

            var x = trainingProgramViewModels.SyllabusIds.Any(x => x == 0);
            if (!x)
            {
                foreach (var item in trainingProgramViewModels.SyllabusIds)
                {
                    syllabuss.Add(await _unitOfWork.SyllabusRepo.GetByIdAsync(item));
                }
                trainingProgramObj.Syllabuses = syllabuss;
            }
            trainingProgramObj.CreatedBy = _claimsService.GetCurrentUserId;
            trainingProgramObj.LastModifyBy = _claimsService.GetCurrentUserId;
            trainingProgramObj.LastModify = _currentTime.GetCurrentTime();
            trainingProgramObj.CreatedOn = _currentTime.GetCurrentTime();

            await _unitOfWork.TrainingProgramRepo.AddAsync(trainingProgramObj);

            if (await _unitOfWork.SaveChangesAsync() > 0)
                return _mapper.Map<TrainingProgramViewModels>(trainingProgramObj);
            else throw new Exception("Add Syllabus into TrainingProgam faild.");
        }

        #endregion

        #region Get All TrainingProgram Is Active By Name
        public async Task<List<TrainingProgramViewModels>> GetTrainingProgramIsActiveByNameAsync(string name)
        {
            // Get paginated user entities list
            var listTrainingProgram = await _unitOfWork.TrainingProgramRepo.GetAllAsync();
            var listTrainingProgramFalse = listTrainingProgram.FindAll(x => x.IsActive
                && (x.Name.Contains(name, StringComparison.OrdinalIgnoreCase)));

            // Map user entities to user list model
            var result = _mapper.Map<List<TrainingProgramViewModels>>(listTrainingProgramFalse);

            return result;

        }
        #endregion

        #region Get All TrainingProgram By Name
        public async Task<List<TrainingProgramViewModels>> GetTrainingProgramByNameAsync(string name)
        {
            // Get paginated user entities list
            var listTrainingProgram = await _unitOfWork.TrainingProgramRepo.GetAllAsync();
            var listTrainingProgramFalse = listTrainingProgram.FindAll(x => (x.Name.Contains(name, StringComparison.OrdinalIgnoreCase)));

            // Map user entities to user list model
            var result = _mapper.Map<List<TrainingProgramViewModels>>(listTrainingProgramFalse);

            return result;

        }
        #endregion

        #region ExportCsvFile
        public byte[] ExportCsvFile(string columnSeperator)
        {
            string[] columnNames = new string[] { "Name", "Syllabuses" };
            string csv = string.Empty;

            foreach (var column in columnNames)
            {
                csv += column + columnSeperator;
            }

            csv += "\n#Example Template: Insert Training Program With this format.\n" +
                "#Name, #Syllabus1-Syllabus2-Syllabus3-...";
            csv.Replace(",", columnSeperator);

            csv += "\r\n";

            return Encoding.UTF8.GetBytes(csv);
        }
        #endregion

        #region ImportCsvFileAsync
        public async Task<List<TrainingProgramViewModels>> ImportCsvFileAsync(IFormFile formFile,
            bool isScanName,
            DuplicateHandle duplicateHandle)
        {
            Dictionary<string, List<Syllabus>> listTrainingProgramsCsvFile = new Dictionary<string, List<Syllabus>>();

            #region ReadCsvFile
            var fileName = Guid.NewGuid().ToString() + ".csv";
            var binPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var filePath = Path.Combine(binPath!, fileName);

            using (FileStream fs = File.Create(filePath))
            {
                await formFile.CopyToAsync(fs);
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
                    var trainingProgramRecords = csv.GetRecords<dynamic>();

                    foreach (var record in trainingProgramRecords)
                    {
                        if (!isScanName && await _unitOfWork.TrainingProgramRepo.IsExistNameAsync(record.Name))
                        {
                            streamReader.Dispose();
                            File.Delete(filePath);
                            throw new Exception($"Error at line {csv.Context.Parser.Row}, " +
                                $"Column Name: {csv.HeaderRecord?[csv.GetFieldIndex("Name")]}, " +
                                $"Error: This Training Program Name is already exists.");
                        }
                        listTrainingProgramsCsvFile.Add(record.Name,
                            await GetListTraningProgramSyllabus(record.Syllabuses));
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
            }

            File.Delete(filePath);
            #endregion

            #region InsertToDb
            //Get Current User Id
            var currentUserId = _claimsService.GetCurrentUserId;

            //Scanning Handle
            var listCreateTrainingProgramArray = await ImportScanningHandle(listTrainingProgramsCsvFile, isScanName, duplicateHandle);

            //List Create Training Program
            var createdList = new List<TrainingProgram>();

            foreach (var trainingProgram in listCreateTrainingProgramArray[0].Keys)
            {
                TrainingProgram trainingPrg = new TrainingProgram
                {
                    Name = trainingProgram,
                    LastModify = _currentTime.GetCurrentTime(),
                    IsActive = true,
                    CreatedOn = _currentTime.GetCurrentTime(),
                    LastModifyBy = currentUserId,
                    CreatedBy = currentUserId,
                    isDeleted = false,
                    Duration = 0,
                    DaysDuration = 0,
                    TimeDuration = 0,
                    Syllabuses = listCreateTrainingProgramArray[0][trainingProgram]
                };
                createdList.Add(trainingPrg);
            }

            await _unitOfWork.TrainingProgramRepo.AddRangeAsync(createdList);

            //List Update After Scanning with 'Replace' Option
            if (listCreateTrainingProgramArray[1].Count != 0)
            {
                var updatedList = new List<TrainingProgram>();
                foreach (var trainingProgram in listCreateTrainingProgramArray[1].Keys)
                {
                    var trainingPrg = await _unitOfWork.TrainingProgramRepo.GetByNameAsync(trainingProgram);
                    trainingPrg!.LastModify = _currentTime.GetCurrentTime();
                    trainingPrg.IsActive = true;
                    trainingPrg.LastModifyBy = currentUserId;
                    trainingPrg.isDeleted = false;
                    trainingPrg.Syllabuses = listCreateTrainingProgramArray[1][trainingProgram];

                    updatedList.Add(trainingPrg);
                };

                _unitOfWork.TrainingProgramRepo.UpdateRange(updatedList);
            }

            if (await _unitOfWork.SaveChangesAsync() >= listTrainingProgramsCsvFile.Keys.Count())
                return _mapper.Map<List<TrainingProgramViewModels>>(createdList);
            #endregion

            return new List<TrainingProgramViewModels>();
        }
        #endregion

        #region ImportScanningHandle
        private async Task<Dictionary<string, List<Syllabus>>[]> ImportScanningHandle(Dictionary<string, List<Syllabus>> listTrainingPrograms,
            bool isScanName,
            DuplicateHandle duplicateHandle)
        {
            Dictionary<string, List<Syllabus>> updateList = new Dictionary<string, List<Syllabus>>();
            switch (duplicateHandle)
            {
                case DuplicateHandle.Allow:
                    if (isScanName)
                    {
                        throw new Exception("Training Program Name cannot be duplicated.");
                    }
                    break;
                case DuplicateHandle.Replace:
                    if (isScanName)
                    {
                        foreach (var trainingProg in listTrainingPrograms.Keys)
                        {
                            if (await _unitOfWork.TrainingProgramRepo.IsExistNameAsync(trainingProg))
                            {
                                updateList.Add(trainingProg, listTrainingPrograms[trainingProg]);
                            }
                        }
                        listTrainingPrograms = listTrainingPrograms
                            .Where(x => !updateList.ContainsKey(x.Key))
                            .ToDictionary(x => x.Key, x => x.Value);
                    }
                    break;
                case DuplicateHandle.Skip:
                    if (isScanName)
                    {
                        foreach (var trainingProg in listTrainingPrograms.Keys)
                        {
                            if (await _unitOfWork.TrainingProgramRepo.IsExistNameAsync(trainingProg))
                            {
                                updateList.Add(trainingProg, listTrainingPrograms[trainingProg]);
                            }
                        }
                        listTrainingPrograms = listTrainingPrograms
                            .Where(x => !updateList.ContainsKey(x.Key))
                            .ToDictionary(x => x.Key, x => x.Value);
                        updateList.Clear();
                    }
                    break;
            }
            return new Dictionary<string, List<Syllabus>>[] { listTrainingPrograms, updateList };
        }
        #endregion

        #region Get TrainingProgram ByDate Range CreateOn
        public async Task<List<TrainingProgramViewModels>> GetTrainingProgramByDateRangeCreateOnAsync(DateTime[] fromToDate)
        {
            var listTrainingProgram = await GetAllTrainingProgramIsActiveAsync();

            if (fromToDate.Length == 0)
            {
                return listTrainingProgram;
            }
            listTrainingProgram = listTrainingProgram
                .Where(x =>
                {
                    DateTime createdDate = DateTime.ParseExact(x.CreatedOn, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    return createdDate >= fromToDate[0].Date
                        && createdDate <= fromToDate[1].Date;
                }).ToList();
            return listTrainingProgram;
        }
        #endregion

        #region Get TrainingProgram ByDate Range ListModify
        public async Task<List<TrainingProgramViewModels>> GetTrainingProgramByDateRangeLastModifyAsync(DateTime[] fromToDate)
        {
            var listTrainingProgram = await GetAllTrainingProgramIsActiveAsync();

            if (fromToDate.Length == 0)
            {
                return listTrainingProgram;
            }
            listTrainingProgram = listTrainingProgram
                .Where(x =>
                {
                    DateTime lastModifiedDate = DateTime.ParseExact(x.CreatedOn, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    return lastModifiedDate >= fromToDate[0].Date
                        && lastModifiedDate <= fromToDate[1].Date;
                }).ToList();
            return listTrainingProgram;
        }
        #endregion

        #region GetListTraningProgramSyllabus
        private async Task<List<Syllabus>> GetListTraningProgramSyllabus(string listSyllabus)
        {
            string[] syllabusArray = listSyllabus.Split('-');
            List<Syllabus> syllabuses = new List<Syllabus>();
            foreach (var syllabus in syllabusArray)
            {
                var tmpSyllabus = await _unitOfWork.SyllabusRepo.GetLatestByNameAsync(syllabus.Trim());
                if (tmpSyllabus == null)
                {
                    throw new InvalidDataException($"Syllabus '{syllabus}' doesn't exist.");
                }

                syllabuses.Add(tmpSyllabus);
            }
            return syllabuses;
        }
        #endregion

        #region Clone TrainingProgram
        public async Task CloneTrainingProgramAsync(int id)
        {
            if (id <= 0) throw new Exception("TrainingProgram id must be an integer with value equal or greater than 1");

            var trainingProgram = await _unitOfWork.TrainingProgramRepo.GetByIdAsync(id);

            if (trainingProgram == null) throw new Exception("TrainingProgram not found");

            trainingProgram.CreatedBy = _claimsService.GetCurrentUserId;
            trainingProgram.LastModifyBy = _claimsService.GetCurrentUserId;
            trainingProgram.LastModify = _currentTime.GetCurrentTime();
            trainingProgram.CreatedOn = _currentTime.GetCurrentTime();

            var model = await _unitOfWork.TrainingProgramRepo.CloneAsync(trainingProgram);
            model.Id = 0;
            model.Syllabuses = trainingProgram.Syllabuses;
            await _unitOfWork.TrainingProgramRepo.AddAsync(model);
            await _unitOfWork.SaveChangesAsync();
            foreach (var item in model.Syllabuses)
            {
                await _syllabusService.CloneSyllabusAsync(item.Id);
            }
        }
        #endregion

        #region Filter
        public async Task<List<TrainingProgramViewModels>> Filter(TrainingProgramFilterModel filter)
        {
            var trainingPrograms = await _unitOfWork.TrainingProgramRepo.GetAllAsync();
            if (!string.IsNullOrEmpty(filter.Name))
            {
                trainingPrograms = trainingPrograms.Where(x => x.Name.ToUpper().Contains(filter.Name.ToUpper())).ToList();
            }

            if (!string.IsNullOrEmpty(filter.CreatedBy))
            {
                trainingPrograms = trainingPrograms.Where(x => x.CreatedAdmin.FullName.ToUpper().Contains(filter.CreatedBy.ToUpper())).ToList();
            }

            if (!string.IsNullOrEmpty(filter.CreatedOn[0]) && !string.IsNullOrEmpty(filter.CreatedOn[1]))
            {
                trainingPrograms = trainingPrograms.Where(x => x.CreatedOn >= DateTime.ParseExact(filter.CreatedOn[0], "dd/MM/yyyy", CultureInfo.InvariantCulture) && x.CreatedOn <= DateTime.ParseExact(filter.CreatedOn[1], "dd/MM/yyyy", CultureInfo.InvariantCulture)).ToList();
            }

            if (filter.IsActive != null)
            {
                trainingPrograms = trainingPrograms.Where(x => x.IsActive.Equals(filter.IsActive)).ToList();
            }

            if (filter.Duration != 0)
            {
                trainingPrograms = trainingPrograms.Where(x => x.Duration.Equals(filter.Duration)).ToList();
            }

            trainingPrograms = trainingPrograms.Where(x => x.isDeleted.Equals(false)).ToList();

            var list = _mapper.Map<List<TrainingProgramViewModels>>(trainingPrograms);
            return list;
        }
        #endregion

        #region ExportExcelFile
        public async Task<Stream> ExportExcelFileAsync()
        {
            string[] columnNames = new string[] { "Name", "Syllabuses" };
            string header = string.Empty;

            foreach (var column in columnNames)
            {
                header += column + ",";
            }

            var stream = new MemoryStream();

            using (var package = new ExcelPackage(stream))
            {
                var worksheet = package.Workbook.Worksheets.Add("Training Programs");
                worksheet.Cells.LoadFromText(header);

                await package.SaveAsync();
            }

            stream.Position = 0;

            return stream;
        }
        #endregion

        #region ImportExcelFile
        public async Task<List<TrainingProgramViewModels>> ImportExcelFileAsync(IFormFile formFile,
            bool isScanName,
            DuplicateHandle duplicateHandle)
        {
            Dictionary<string, List<Syllabus>> listFromFile = new Dictionary<string, List<Syllabus>>();

            #region ReadCsvFile
            using (var stream = new MemoryStream())
            {
                await formFile.CopyToAsync(stream);

                using (var excelPackage = new ExcelPackage(stream))
                {
                    ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets[0];
                    var rowCount = worksheet.Dimension.Rows;

                    for (int row = 2; row <= rowCount; row++)
                    {
                        var col = 1;
                        try
                        {
                            var name = worksheet.Cells[row, col++].Value.ToString()!.Trim();
                            var syllabus = worksheet.Cells[row, col++].Value.ToString()!.Trim();
                                if (!isScanName && await _unitOfWork.TrainingProgramRepo.IsExistNameAsync(name))
                                {
                                    await stream.DisposeAsync();
                                    throw new Exception($"Error at line {row}, " +
                                        $"Column Name: {worksheet.Cells[1, col - 1].Value}, " +
                                        $"Error: This Training Program Name is already exists.");
                                }
                            listFromFile.Add(name,
                                    await GetListTraningProgramSyllabus(syllabus));
                        }
                        catch (InvalidDataException ex)
                        {
                            await stream.DisposeAsync();
                            throw new InvalidDataException($"Exception at line {row}, Column Name: {worksheet.Cells[1, col - 1].Value}, " +
                                $"Error: {ex.Message}");
                        }
                        catch (Exception)
                        {
                            await stream.DisposeAsync();
                            throw new InvalidDataException($"Exception at line {row}, Column Name: {worksheet.Cells[1, col - 1].Value}, " +
                                $"Error: This Cell has an empty value or a not valid value.");
                        }
                    }
                }
            }
            #endregion

            #region InsertToDb
            //Get Current User Id
            var currentUserId = _claimsService.GetCurrentUserId;

            //Scanning Handle
            var listCreateTrainingProgramArray = await ImportScanningHandle(listFromFile, isScanName, duplicateHandle);

            //List Create Training Program
            var createdList = new List<TrainingProgram>();

            foreach (var trainingProgram in listCreateTrainingProgramArray[0].Keys)
            {
                TrainingProgram trainingPrg = new TrainingProgram
                {
                    Name = trainingProgram,
                    LastModify = _currentTime.GetCurrentTime(),
                    IsActive = true,
                    CreatedOn = _currentTime.GetCurrentTime(),
                    LastModifyBy = currentUserId,
                    CreatedBy = currentUserId,
                    isDeleted = false,
                    Duration = 0,
                    DaysDuration = 0,
                    TimeDuration = 0,
                    Syllabuses = listCreateTrainingProgramArray[0][trainingProgram]
                };
                createdList.Add(trainingPrg);
            }

            await _unitOfWork.TrainingProgramRepo.AddRangeAsync(createdList);

            //List Update After Scanning with 'Replace' Option
            if (listCreateTrainingProgramArray[1].Count != 0)
            {
                var updatedList = new List<TrainingProgram>();
                foreach (var trainingProgram in listCreateTrainingProgramArray[1].Keys)
                {
                    var trainingPrg = await _unitOfWork.TrainingProgramRepo.GetByNameAsync(trainingProgram);
                    trainingPrg!.LastModify = _currentTime.GetCurrentTime();
                    trainingPrg.IsActive = true;
                    trainingPrg.LastModifyBy = currentUserId;
                    trainingPrg.isDeleted = false;
                    trainingPrg.Syllabuses = listCreateTrainingProgramArray[1][trainingProgram];

                    updatedList.Add(trainingPrg);
                };

                _unitOfWork.TrainingProgramRepo.UpdateRange(updatedList);
            }

            if (await _unitOfWork.SaveChangesAsync() >= listFromFile.Keys.Count())
                return _mapper.Map<List<TrainingProgramViewModels>>(createdList);
            #endregion

            return new List<TrainingProgramViewModels>();
        }
        #endregion
    }
}