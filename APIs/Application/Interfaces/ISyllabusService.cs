using Application.ViewModels.AddLectureViewModels;
using Application.ViewModels.AttendanceViewModels;
using Application.ViewModels.LectureViewModels;
using Application.ViewModels.OutputStandardViewModels;
using Application.ViewModels.SyllabusViewModels;
using Application.ViewModels.UnitViewModels;
using Domain.Enums.ResponeModelEnums;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;

namespace Application.Interfaces
{
    public interface ISyllabusService
    {
        //your action is here
        Task CloneSyllabusAsync(int syllabusId);
        Task<SyllabusDetailModel> GetSyllabusDetailByIdAsync(int syllabusId);

        public Task<SyllabusViewModel> DeleteSyllabusAsync(int id);
        Task<List<SyllabusViewModel>> GetSyllabusByNameAsync(string searchString);

        Task<List<SyllabusViewModel>> GetSyllabusByDateRangeAsync(DateTime[] fromToDate);

        public Task<LectureViewModel> AddLectureAsync(AddLectureViewModel model);

        public Task<LectureViewModel> AddLectureToUnitAsync(int Lectureid, int UnitId);

        #region Update

        public Task<SyllabusViewModel> UpdateSyllabusAsync(UpdateSyllabusViewModel updateSyllabusViewModel, int syllabusId);

        public Task<UpdateUnitInSyllabuViewModel> UpdateUnitInSyllabusAsync(UnitUpdateViewModel unitUpdateViewModel,
            int syllabusId);

        public Task<UpdateLectureInSyllabusViewModel> UpdateLectureInSyllabusAsync(UpdateLectureViewModel updateLectureViewModel,
            int syllabusId);

        #endregion

        public Task<List<SyllabusViewModel>> GetSyllabusAsync();

        public Task<List<OutputStandardViewModel>> GetOutputStandardsAsync();
        Task<ValidationResult> ValidateAddLectureAsync(AddLectureViewModel model);

        public Task<SyllabusViewModel> AddSyllabusAsync(AddSyllabusViewModel syllabus);

        byte[] ExportCsvFile(string columnSeperator);
        Task<List<SyllabusViewModel>> ImportCsvFileAsync(IFormFile formfile,
            bool isScanCode, bool isScanName,
            DuplicateHandle duplicateHandle);

        Task<Stream> ExportExcelFileAsync();
        Task<List<SyllabusViewModel>> ImportExcelFileAsync(IFormFile formfile,
            bool isScanCode, bool isScanName,
            DuplicateHandle duplicateHandle);
    }
}