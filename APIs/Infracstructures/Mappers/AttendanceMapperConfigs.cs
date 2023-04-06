
using Application.ViewModels.AttendanceViewModels;
using AutoMapper;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infracstructures.Mappers
{
    public partial class MapperConfigs : Profile
    {
    
        partial void AddAttendanceConfig()
        {
            CreateMap<Attendance,TakeAttendanceModel>().ReverseMap();
            CreateMap<Attendance, EditAttendanceViewModel>().ReverseMap();
            // Map Attendance entity to AttendanceViewModel
            CreateMap<Attendance, AttendanceViewModel>()
                .ForMember(dest => dest.Day, opt => opt.MapFrom(src => src.Day.ToString("dd/MM/yyyy",CultureInfo.InvariantCulture)))
                .ReverseMap();

        }
    }
}
  
        