using Application.Repositories;

namespace Application
{
    public interface IUnitOfWork
    {
        public IUserRepository UserRepo { get; }
        public IAttendancesRepository AttendancesRepo { get; }
        public IAuditResultRepository AuditResultRepo { get; }
        public IAuditPlanRepository AuditPlanRepo { get; }
        public IAuditDetailRepository AuditDetailRepo { get; }
        public IClassRepository ClassRepo { get; }
        public IClassUserRepository ClassUserRepo { get; }
        public IClassUnitDetailRepository ClassUnitRepo { get; }
        public IFeedbackFormsRepository FeedbackFormRepo { get; }
        public IGradeReportsRepository GradeReportRepo { get; }
        public ILecturesRepository LectureRepo { get; }
        public IOutputStandardRepository OutputStandardRepo { get; }
        public ISyllabusRepository SyllabusRepo { get; }
        public ITimeMngSystemsRepository TimeMngSystemRepo { get; }
        public ITrainingMaterialRepository TrainingMaterialRepo { get; }
        public ITrainingProgramRepository TrainingProgramRepo { get; }
        public IUnitRepository UnitRepo { get; }
        public IRoleRepository RoleRepo { get; }

        public Task<int> SaveChangesAsync();
    }
}