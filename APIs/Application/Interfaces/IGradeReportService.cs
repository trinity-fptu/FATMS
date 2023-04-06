using Application.ViewModels.GradeViewModels;

namespace Application.Interfaces
{
    public interface IGradeReportService
    {
        Task CreateReportAsync(GradeViewModel gradeViewModel);
        Task<GradeReportViewModel> ViewGradeReportByIdAsync(int id);
        Task<List<GradeReportViewModel>> ViewAllGradeReportAsync();
    }
}