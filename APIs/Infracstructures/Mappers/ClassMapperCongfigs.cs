using Application.ViewModels.ClassViewModels;
using AutoMapper;
using Domain.Models;
using Domain.Models.Users;

namespace Infracstructures.Mappers
{
    public partial class MapperConfigs : Profile
    {
        partial void AddClassConfig()
        {
            // Map Class entity to ClassViewModel
            CreateMap<Class, ClassViewModel>()
                .ForMember(des => des.CreatedOn, src => src.MapFrom(x => x.CreatedOn.ToString("dd/MM/yyyy")))
                .ForMember(des => des.StartedOn, src => src.MapFrom(x => x.StartedOn.ToString("dd/MM/yyyy")))
                .ForMember(des => des.FinishedOn, src => src.MapFrom(x => x.FinishedOn.ToString("dd/MM/yyyy")))
                .ForMember(des => des.CreatedBy, src => src.MapFrom(x => x.CreatedAdmin.FullName))
                .ReverseMap();

            //Map Class entity class to CreateViewModel and reverse
            CreateMap<Class, CreateClassViewModel>().ReverseMap();

            CreateMap<Class, UpdateClassViewModel>().ReverseMap();

            CreateMap<Class, ClassDetailViewModels>()
                .ForMember(des => des.CreatedAdmin, src => src.MapFrom(x => x.CreatedAdmin))
                .ForMember(des => des.Admin, src => src.MapFrom(x => x.ClassUsers))
                .ForMember(des => des.Trainer, src => src.MapFrom(x => x.ClassUnitDetails))
                .ForMember(des => des.AttendeeType, src => src.MapFrom(x => x.AttendeeType))
            .ReverseMap();

            CreateMap<User, Admin>()
                .ForMember(des => des.AdminId, src => src.MapFrom(x => x.Id))
                .ForMember(des => des.FullName, src => src.MapFrom(x => x.FullName))
                .ForMember(des => des.Email, src => src.MapFrom(x => x.Email))
                .ForMember(des => des.Phone, src => src.MapFrom(x => x.Phone))
                .ReverseMap();

            CreateMap<ClassUsers, Admin>()
                .ForPath(des => des.AdminId, src => src.MapFrom(x => x.User.Id))
                .ForPath(des => des.FullName, src => src.MapFrom(x => x.User.FullName))
                .ForPath(des => des.Email, src => src.MapFrom(x => x.User.Email))
                .ForPath(des => des.Phone, src => src.MapFrom(x => x.User.Phone))
                .ReverseMap();

            CreateMap<ClassUnitDetail, Trainer>()
                .ForPath(des => des.TrainerId, src => src.MapFrom(x => x.Trainer.Id))
                .ForPath(des => des.FullName, src => src.MapFrom(x => x.Trainer.FullName))
                .ForPath(des => des.Email, src => src.MapFrom(x => x.Trainer.Email))
                .ForPath(des => des.Phone, src => src.MapFrom(x => x.Trainer.Phone))
                .ForPath(des => des.AvatarUrl, src => src.MapFrom(x => x.Trainer.AvatarURL))
                .ReverseMap();

            CreateMap<ClassUnitDetail, UnitDetail>()
                .ForPath(des => des.UnitId, src => src.MapFrom(x => x.UnitId))
                .ForPath(des => des.TrainerId, src => src.MapFrom(x => x.TrainerId))
                .ForPath(des => des.Location, src => src.MapFrom(x => x.Location))
                .ReverseMap();
        }
    }
}
