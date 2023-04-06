using Application.ViewModels.ClassViewModels;

namespace Application.Interfaces
{
    public interface IClassService
    {
        //your action is here
        Task<String> CreateClassAsync(CreateClassViewModel createClassViewModel);
        Task<String> AddUserToClassAsync(AddUserToClassViewModel addUserToClassViewModel);
//        Task<ClassViewModel> GetClassByIDAsync(int id);
        Task<List<ClassViewModel>> GetClassListAsync();
        Task<List<ClassViewModel>> GetOpeningClassListAsync();
        Task<bool> UpdateClassAsync(int classId,UpdateClassViewModel updateClassView);
        Task<List<int>> GetUnitIdList(int trainingProgramId);
        Task<bool> AddClassUnit(int trainingProgramId, int classId);
        Task CloneClassAsync(int id);
        Task<ClassDetailViewModels> GetClassDetail(int id);
    }
}