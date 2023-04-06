using Application.ViewModels.SyllabusViewModels;
using Application.ViewModels.TrainingProgramViewModels;
using Domain.Enums.ResponeModelEnums;
using Microsoft.AspNetCore.Http;

namespace Application.Interfaces
{
    public interface ITrainingProgramService
    {
        Task<TrainingProgramViewModels> GetByIdAsync(int id);
        Task<bool> DeleteTrainingProgramsAsync(int id);
        Task<bool> UpdateTrainingProgramsAsync(int id, UpdateTrainingProgramViewModel updatetrainingProgramViewModels);
        Task<List<TrainingProgramViewModels>> GetAllAsync();
        Task<TrainingProgramViewModels> CreateTrainingProgramAsync(CreateTrainingProgramViewModels trainingProgramViewModels);
        Task<List<TrainingProgramViewModels>> GetAllTrainingProgramIsActiveAsync();
        Task<List<TrainingProgramViewModels>> GetTrainingProgramIsActiveByNameAsync(string name);
        Task<List<TrainingProgramViewModels>> GetTrainingProgramByNameAsync(string name);

        Task<List<TrainingProgramViewModels>> GetTrainingProgramByDateRangeCreateOnAsync(DateTime[] fromToDate);
        Task<List<TrainingProgramViewModels>> GetTrainingProgramByDateRangeLastModifyAsync(DateTime[] fromToDate);
        Task<List<TrainingProgramViewModels>> Filter(TrainingProgramFilterModel trainingProgramFilterModels);
        Task CloneTrainingProgramAsync(int id);
        byte[] ExportCsvFile(string columnSeperator);
        Task<List<TrainingProgramViewModels>> ImportCsvFileAsync(IFormFile formFile, 
            bool isScanName, 
            DuplicateHandle duplicateHandle);
        Task<Stream> ExportExcelFileAsync();
        Task<List<TrainingProgramViewModels>> ImportExcelFileAsync(IFormFile formFile,
            bool isScanName,
            DuplicateHandle duplicateHandle);
    }
}