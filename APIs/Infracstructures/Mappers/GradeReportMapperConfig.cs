using Application.ViewModels.GradeViewModels;
using AutoMapper;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infracstructures.Mappers
{
    public partial class MapperConfigs : Profile
    {
        partial void AddGradeReportMapperConfig()
        {
            // Map GradeReport entity to GradeViewModel and reverse
            CreateMap<GradeReport, GradeViewModel>()
                .ForMember(des => des.Grade, src => src.MapFrom(x => x.Grade.ToString()))
                .ReverseMap();
            CreateMap<GradeReport, GradeReportViewModel>()
                .ForMember(des => des.Grade, src => src.MapFrom(x => x.Grade.ToString()))
                .ForMember(des => des.Trainee, src => src.MapFrom(x => x.User.FullName))
                .ForMember(des => des.Lecture, src => src.MapFrom(x => x.Lecture.Name))
                .ReverseMap();
        }
    }
}
