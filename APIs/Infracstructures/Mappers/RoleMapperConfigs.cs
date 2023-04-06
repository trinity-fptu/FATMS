using Application.ViewModels.RoleViewModels;
using AutoMapper;
using Domain.Enums.RoleEnums;
using Domain.Models;

namespace Infracstructures.Mappers
{
    public partial class MapperConfigs : Profile
    {
        partial void AddRoleConfig()
        {
            //Map from Role to RoleViewModel
            CreateMap<Role, RoleViewModel>()
                .ForMember(des => des.SyllabusPermission, src => src.MapFrom(src => src.SyllabusPermission.ToString()))
                .ForMember(des => des.TrainingProgramPermission, src => src.MapFrom(src => src.TrainingProgramPermission.ToString()))
                .ForMember(des => des.ClassPermission, src => src.MapFrom(src => src.ClassPermission.ToString()))
                .ForMember(des => des.LearningMaterialPermission, src => src.MapFrom(src => src.LearningMaterialPermission.ToString()))
                .ForMember(des => des.UserPermission, src => src.MapFrom(src => src.UserPermission.ToString()));

            //Map from RoleUpdateModel to Role
            CreateMap<RoleUpdateModel, Role>()
                .ForMember(des => des.Name, act => act.Ignore())
                .ForMember(des => des.SyllabusPermission, src => src.MapFrom(src => (UserPermission)Enum.Parse(typeof(UserPermission), src.SyllabusPermission)))
                .ForMember(des => des.TrainingProgramPermission, src => src.MapFrom(src => (UserPermission)Enum.Parse(typeof(UserPermission), src.TrainingProgramPermission)))
                .ForMember(des => des.ClassPermission, src => src.MapFrom(src => (UserPermission)Enum.Parse(typeof(UserPermission), src.ClassPermission)))
                .ForMember(des => des.LearningMaterialPermission, src => src.MapFrom(src => (UserPermission)Enum.Parse(typeof(UserPermission), src.LearningMaterialPermission)))
                .ForMember(des => des.UserPermission, src => src.MapFrom(src => (UserPermission)Enum.Parse(typeof(UserPermission), src.UserPermission)));

            //Map from RoleCreateModel to Role
            CreateMap<RoleCreateModel, Role>()
                .ForMember(des => des.SyllabusPermission, src => src.MapFrom(src => (UserPermission)Enum.Parse(typeof(UserPermission), !string.IsNullOrEmpty(src.SyllabusPermission) ? src.SyllabusPermission : "AccessDenied")))
                .ForMember(des => des.TrainingProgramPermission, src => src.MapFrom(src => (UserPermission)Enum.Parse(typeof(UserPermission), !string.IsNullOrEmpty(src.TrainingProgramPermission) ? src.TrainingProgramPermission : "AccessDenied")))
                .ForMember(des => des.ClassPermission, src => src.MapFrom(src => (UserPermission)Enum.Parse(typeof(UserPermission), !string.IsNullOrEmpty(src.ClassPermission) ? src.ClassPermission : "AccessDenied")))
                .ForMember(des => des.LearningMaterialPermission, src => src.MapFrom(src => (UserPermission)Enum.Parse(typeof(UserPermission), !string.IsNullOrEmpty(src.LearningMaterialPermission) ? src.LearningMaterialPermission : "AccessDenied")))
                .ForMember(des => des.UserPermission, src => src.MapFrom(src => (UserPermission)Enum.Parse(typeof(UserPermission), !string.IsNullOrEmpty(src.UserPermission) ? src.UserPermission : "AccessDenied")));
        }
    }
}
