using Application.ViewModels.AuditViewModels;
using Application.ViewModels.OutputStandardViewModels;
using Application.ViewModels.TrainingDeliveryPrincipleViewModels;
using AutoMapper;
using Domain.Enums.AuditDetailsEnums;
using Domain.Models;
using Infracstructures.MappingHelpers;
using System.Diagnostics;
using System.Globalization;

namespace Infracstructures.Mappers
{
    public partial class MapperConfigs : Profile
    {
        partial void AddAuditConfig()
        {
            CreateMap<AuditPlan, AddAuditPlanViewModel>().ReverseMap();

            CreateMap<AuditResult, AddAuditResultViewModel>().ReverseMap()
                .ForMember(dest => dest.Rating, opt => opt.MapFrom((src, dest) => EnumMapper<AuditAnswerRating>.MapType(src.Rating)));

            CreateMap<AuditDetail, AuditResultDetailViewModel>()
                .ForPath(dest => dest.ClassCode, opt => opt.MapFrom(src => src.AuditPlan.Class.Code))
                .ForPath(dest => dest.SyllabusCode, opt => opt.MapFrom(src => src.AuditPlan.Syllabus.Code))
                .ForPath(dest => dest.AuditorName, opt => opt.MapFrom(src => src.Auditor.FullName))
                .ForPath(dest => dest.TraineeName, opt => opt.MapFrom(src => src.Trainee.FullName))
                .ForPath(dest => dest.Location, opt => opt.MapFrom(src => src.AuditPlan.Location))
                .ForPath(dest => dest.Date, opt => opt.MapFrom(src => src.AuditPlan.AuditDate.ToString("dd/MM/yyyy")))
                .ForPath(dest => dest.NumOfQuestion, opt => opt.MapFrom(src => src.Results.Count))
                .ForMember(dest => dest.Status, opt => opt.MapFrom((src, dest) =>
                {
                    string destinationValue = "";

                    if(src.Status != null)
                    {
                        destinationValue = src.Status == true ? "Pass" : "Not pass";
                    }

                    return destinationValue;
                }))
                .ReverseMap()
                .ForMember(dest => dest.Status, opt => opt.MapFrom((src, dest) =>
                {
                    bool? destinationValue = null;

                    if (src.Status.Equals("Pass"))
                    {
                        destinationValue = true;
                    }else if(src.Status.Equals("Not pass"))
                    {
                        destinationValue = false;
                    }

                    return destinationValue;
                }));

            CreateMap<AuditResult, AuditResultViewModel>().ReverseMap()
                .ForMember(dest => dest.Rating, opt => opt.MapFrom(src => EnumMapper<AuditAnswerRating>.MapType(src.Rating)));

            CreateMap<AuditDetail, AddAuditDetailViewModel>().ReverseMap();

        }
    }
}
