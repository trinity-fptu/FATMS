using Application;
using Application.Repositories;

namespace Infracstructures
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private readonly IUserRepository _userRepository;
        private readonly IAttendancesRepository _attendancesRepository;
        private readonly IAuditResultRepository _auditResultRepository;
        private readonly IAuditPlanRepository _auditPlanRepository;
        private readonly IAuditDetailRepository _auditDetailRepository;
        private readonly IClassRepository _classRepository;
        private readonly IClassUserRepository _classUserRepository;
        private readonly IFeedbackFormsRepository _feedbackFormsRepository;
        private readonly IClassUnitDetailRepository _classUnitDetailRepository;
        private readonly IGradeReportsRepository _gradeReportsRepository;
        private readonly ILecturesRepository _lecturesRepository;
        private readonly IOutputStandardRepository _outputStandardRepository;
        private readonly ISyllabusRepository _syllabusRepository;
        private readonly ITimeMngSystemsRepository _timeMngSystemsRepository;
        private readonly ITrainingMaterialRepository _trainingMaterialRepository;
        private readonly ITrainingProgramRepository _trainingProgramRepository;
        private readonly IUnitRepository _unitRepository;
        private readonly IRoleRepository _roleRepository;

        public UnitOfWork(AppDbContext context, IUserRepository userRepository,
            IAttendancesRepository attendancesRepository,
            IAuditResultRepository auditResultRepository,
            IAuditPlanRepository auditPlanRepository,
            IAuditDetailRepository auditDetailRepository,
            IClassRepository classRepository,
            IClassUserRepository classUserRepository,
            IFeedbackFormsRepository feedbackFormsRepository,
            IGradeReportsRepository gradeReportsRepository,
            ILecturesRepository lecturesRepository,
            IClassUnitDetailRepository classUnitDetailRepository,
            IOutputStandardRepository outputStandardRepository,
            ISyllabusRepository syllabusRepository,
            ITimeMngSystemsRepository timeMngSystemsRepository,
            ITrainingMaterialRepository trainingMaterialRepository,
            ITrainingProgramRepository trainingProgramRepository,
            IUnitRepository unitRepository,
            IRoleRepository roleRepository)
        {
            _context = context;
            _userRepository = userRepository;
            _attendancesRepository = attendancesRepository;
            _auditResultRepository = auditResultRepository;
            _auditPlanRepository = auditPlanRepository;
            _auditDetailRepository = auditDetailRepository;
            _classRepository = classRepository;
            _classUserRepository = classUserRepository;
            _classUnitDetailRepository= classUnitDetailRepository;
            _feedbackFormsRepository = feedbackFormsRepository;
            _gradeReportsRepository = gradeReportsRepository;
            _lecturesRepository = lecturesRepository;
            _outputStandardRepository = outputStandardRepository;
            _syllabusRepository = syllabusRepository;
            _timeMngSystemsRepository = timeMngSystemsRepository;
            _classUnitDetailRepository = classUnitDetailRepository;
            _trainingMaterialRepository = trainingMaterialRepository;
            _trainingProgramRepository = trainingProgramRepository;
            _unitRepository = unitRepository;
            _roleRepository = roleRepository;
        }

        public IUserRepository UserRepo => _userRepository;
        public IAttendancesRepository AttendancesRepo => _attendancesRepository;
        public IAuditResultRepository AuditResultRepo => _auditResultRepository;
        public IAuditPlanRepository AuditPlanRepo => _auditPlanRepository;
        public IAuditDetailRepository AuditDetailRepo => _auditDetailRepository;
        public IClassRepository ClassRepo => _classRepository;
        public IClassUserRepository ClassUserRepo => _classUserRepository;
        public IFeedbackFormsRepository FeedbackFormRepo => _feedbackFormsRepository;
        public IGradeReportsRepository GradeReportRepo => _gradeReportsRepository;
        public ILecturesRepository LectureRepo => _lecturesRepository;
        public IOutputStandardRepository OutputStandardRepo => _outputStandardRepository;
        public ISyllabusRepository SyllabusRepo => _syllabusRepository;
        public IClassUnitDetailRepository ClassUnitRepo => _classUnitDetailRepository;
        public ITimeMngSystemsRepository TimeMngSystemRepo => _timeMngSystemsRepository;
        public ITrainingMaterialRepository TrainingMaterialRepo => _trainingMaterialRepository;
        public ITrainingProgramRepository TrainingProgramRepo => _trainingProgramRepository;
        public IUnitRepository UnitRepo => _unitRepository;
        public IRoleRepository RoleRepo => _roleRepository;

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}