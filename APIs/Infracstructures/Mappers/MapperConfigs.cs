using Application.Commons;
using AutoMapper;

namespace Infracstructures.Mappers
{
    public partial class MapperConfigs : Profile
    {
        public MapperConfigs()
        {
            //add map here
            //CreateMap<SourceModel, DestinationModel>();

            // Create mapping between Pagination
            CreateMap(typeof(Pagination<>), typeof(Pagination<>));

            //Add User Mapper
            AddUserMapperConfig();

            //Add Syllabus Mapper
            AddSyllabusMapperConfig();

            //Add Audit Plan Mapper
            AddAuditConfig();

            //Add Unit Mapper
            AddUnitMapperConfig();

            //Add Lecture Mapper
            AddLectureMapperConfig();

            //Add Training Material Mapper
            AddTrainingMaterialConfig();

            //Add Training Delivery Principle Mapper
            AddTrainingDeliveryPrincipleConfig();

            //Add Training Program Mapper
            AddTrainingProgramConfig();

            //Add Class Mapper
            AddClassConfig();

            //Add Class Unit Mapper
            AddClassUnitConfig();

            //Add Class User Mapper
            AddClassUserConfig();

            //Add Role Mapper
            AddRoleConfig();

            //Add GradeReprt Mapper
            AddGradeReportMapperConfig();

            //Add TMS Mapper
            AddTimeManagementSystemMapperConfig();

            //Add Attendace Mapper
            AddAttendanceConfig();

            //Add OutputStandard Mapper
            AddOutputStandardConfig();

            //Add Attendance Mapper
            AddAttendanceConfig();
        }

        partial void AddUserMapperConfig();
        partial void AddSyllabusMapperConfig();
        partial void AddAuditConfig();
        partial void AddUnitMapperConfig();
        partial void AddLectureMapperConfig();
        partial void AddTrainingMaterialConfig();
        partial void AddTrainingDeliveryPrincipleConfig();
        partial void AddTrainingProgramConfig();
        partial void AddClassConfig();
        partial void AddClassUnitConfig();
        partial void AddClassUserConfig();
        partial void AddRoleConfig();
        partial void AddGradeReportMapperConfig();
        partial void AddTimeManagementSystemMapperConfig();
        partial void AddAttendanceConfig();
        partial void AddOutputStandardConfig();
    }
}
