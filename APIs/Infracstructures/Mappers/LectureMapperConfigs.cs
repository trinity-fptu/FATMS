using Application.ViewModels.AddLectureViewModels;
using Application.ViewModels.LectureViewModels;
using Application.ViewModels.SyllabusViewModels;
using Application.ViewModels.TrainingDeliveryPrincipleViewModels;
using AutoMapper;
using Domain.Enums.LectureEnums;
using Domain.Models;

namespace Infracstructures.Mappers
{
    public partial class MapperConfigs : Profile
    {
        partial void AddLectureMapperConfig()
        {
            // Map Lecture class to LectureDetailModel class
            CreateMap<Lecture, LectureDetailModel>().ForMember(dest => dest.OutputStandard,
                opt => opt.MapFrom(s => s.OutputStandard.Code))
                .ReverseMap();

            CreateMap<Lecture, CloneLectureViewModel>().ReverseMap();


            CreateMap<Lecture, AddLectureViewModel>()
                .ForMember(dest => dest.NameLecture, opt => opt.MapFrom(src => src.Name));
            CreateMap<AddLectureViewModel, Lecture>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.NameLecture))
                .ForMember(des => des.LessonType,
                opt => opt.MapFrom(src => src.LessonType ? LectureLessonType.Online : LectureLessonType.Offline)).ReverseMap();
            CreateMap<AddLectureViewModel, LectureViewModel>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.NameLecture))
                .ForMember(des => des.LessonType,
                opt => opt.MapFrom(src => src.LessonType ? LectureLessonType.Online : LectureLessonType.Offline)).ReverseMap();
            CreateMap<Lecture, ImportLectureModel>().ReverseMap();
            CreateMap<LectureViewModel, Lecture>().ReverseMap();
            CreateMap<UpdateLectureViewModel, Lecture>()
                .ForMember(l => l.Id, opt => opt.MapFrom(lvm => lvm.Id))
                .ForMember(l => l.Name, opt => opt.MapFrom(lvm => lvm.Id))
                .ReverseMap();
        }
    }
}
