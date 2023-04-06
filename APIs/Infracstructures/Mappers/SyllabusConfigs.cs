using Application.ViewModels.SyllabusViewModels;
using AutoMapper;
using Domain.Enums.AuditDetailsEnums;
using Domain.Models;
using Domain.Models.Syllabuses;
using Infracstructures.MappingHelpers;
using System.Globalization;
using Application.ViewModels.LectureViewModels;
using Application.ViewModels.UnitViewModels;

namespace Infracstructures.Mappers
{
    public partial class MapperConfigs : Profile
    {
        partial void AddSyllabusMapperConfig()
        {
            // Map Syllabus class to SyllabusDetailModel class
            CreateMap<Syllabus, SyllabusDetailModel>()
                .ForMember(des => des.LastModifiedOn, src => src.MapFrom(x => x.LastModifiedOn.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture)))
                .ForMember(des => des.CreatedOn, src => src.MapFrom(x => x.CreatedOn.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture)))
                .ForPath(des => des.LastModifiedBy, src => src.MapFrom(x => x.ModifiedAdmin.FullName))
                .ForPath(des => des.CreatedBy, src => src.MapFrom(x => x.CreatedAdmin.FullName))
                .ForPath(des => des.Duration.Days, src => src.MapFrom(x => x.DaysDuration))
                .ForPath(des => des.Duration.Hours, src => src.MapFrom(x => x.TimeDuration))
                .ReverseMap()
                .ForMember(des => des.LastModifiedOn, src => src.MapFrom(x => DateTime.ParseExact(x.LastModifiedOn, "dd/MM/yyyy", CultureInfo.InvariantCulture)))
                .ForMember(des => des.CreatedOn, src => src.MapFrom(x => DateTime.ParseExact(x.CreatedOn, "dd/MM/yyyy", CultureInfo.InvariantCulture)))
                .ForMember(dest => dest.Level, opt => opt.MapFrom(src => EnumMapper<AuditAnswerRating>.MapType(src.Level)))
                .ForPath(des => des.DaysDuration, src => src.MapFrom(x => x.Duration.Days))
                .ForPath(des => des.TimeDuration, src => src.MapFrom(x => x.Duration.Hours));

            CreateMap<Syllabus, CloneSyllabusViewModel>().ReverseMap();

            // Map Syllabus class to ImportSyllabusModel class
            CreateMap<Syllabus, ImportSyllabusModel>().ReverseMap();


            #region Update Syllabus

            // Map from UpdateSyllabusViewModel to SyllabusViewModel
            CreateMap<UpdateSyllabusViewModel, Syllabus>().ReverseMap();
            
            CreateMap<UpdateUnitInSyllabuViewModel, Unit>()
                .ForMember(des => des.Id, opt => opt.MapFrom(src => src.UnitId))
                .ReverseMap();
            
            CreateMap<UnitUpdateViewModel, Unit>()
                .ReverseMap();
            
            CreateMap<UpdateLectureInSyllabusViewModel, Lecture>()
                .ForMember(des => des.Id, opt => opt.MapFrom(src => src.LectureId))
                .ReverseMap();

            CreateMap<UpdateLectureViewModel, Lecture>()
                .ReverseMap();
            

            #endregion

            CreateMap<Syllabus, SyllabusViewModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(des => des.CreatedBy, opt => opt.MapFrom(src => src.CreatedAdmin.FullName))
                .ForMember(des => des.LastModifiedBy, opt => opt.MapFrom(src => src.ModifiedAdmin.FullName))
                .ForMember(des => des.CreatedOn, src => src.MapFrom(x => x.CreatedOn.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture)))
                .ForMember(des => des.LastModifiedOn, src => src.MapFrom(x => x.LastModifiedOn.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture)))
                .ReverseMap();
                            


            CreateMap<Syllabus, AddSyllabusViewModel>()
                .ReverseMap();


            

        }
    }
}
