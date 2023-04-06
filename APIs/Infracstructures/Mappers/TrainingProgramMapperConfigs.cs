using Application.ViewModels.TrainingProgramViewModels;
using AutoMapper;
using Domain.Models;

namespace Infracstructures.Mappers
{
    public partial class MapperConfigs : Profile
    {
        partial void AddTrainingProgramConfig()
        {
            CreateMap<TrainingProgram, TrainingProgramViewModels>()
                .ForMember(des => des.CreatedOn, src => src.MapFrom(x => x.CreatedOn.ToString("dd/MM/yyyy")))
                .ForMember(des => des.LastModify, src => src.MapFrom(x => x.LastModify.ToString("dd/MM/yyyy")))
                .ForMember(des => des.CreatedBy, src => src.MapFrom(x => x.CreatedAdmin.FullName))
                .ForMember(des => des.LastModifyBy, src => src.MapFrom(x => x.ModifiedAdmin.FullName))
                .ForMember(des => des.syllabusDetailIds, src => src.MapFrom(x => x.Syllabuses.Select(x => x.Id)))
                .ReverseMap();

            //Map TrainingProgram entity class to UpdateTrainingProgramViewModel and reverse
            CreateMap<TrainingProgram, UpdateTrainingProgramViewModel>()
                .ReverseMap();

            CreateMap<TrainingProgram, CreateTrainingProgramViewModels>().ReverseMap();

            CreateMap<TrainingProgram, TrainingProgramFilterModel>()
                .ForMember(des => des.CreatedOn, src => src.MapFrom(x => x.CreatedOn.ToString("dd/MM/yyyy")))
                .ForMember(des => des.CreatedBy, src => src.MapFrom(x => x.CreatedBy))
                .ForMember(des => des.CreatedBy, src => src.MapFrom(x => x.IsActive))
                .ForMember(des => des.CreatedBy, src => src.MapFrom(x => x.Duration))
                .ReverseMap();

            CreateMap<TrainingProgramViewModels, TrainingProgramFilterModel>().ReverseMap();
        }
    }
}
