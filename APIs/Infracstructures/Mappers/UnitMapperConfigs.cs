using Application.ViewModels.SyllabusViewModels;
using Application.ViewModels.UnitViewModels;
using AutoMapper;
using Domain.Models;

namespace Infracstructures.Mappers
{
    public partial class MapperConfigs : Profile
    {
        partial void AddUnitMapperConfig()
        {
            // Map Unit class to UnitDetailModel class
            CreateMap<Unit, UnitDetailModel>().ReverseMap();

            CreateMap<Unit, CloneUnitViewModel>().ReverseMap();

            // Map Unit to UnitAddViewModel class

            CreateMap<UnitAddViewModel, Unit>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.NameUnit)).ReverseMap();

            // Map Unit class to ImportUnitModel class
            CreateMap<Unit, ImportUnitModel>().ReverseMap();

            CreateMap<UnitViewModel, Unit>().ReverseMap();
            
            CreateMap<UnitUpdateViewModel, Unit>()
                .ForMember(u => u.Id, opt => opt.MapFrom(uvm => uvm.Id))
                .ForMember(u => u.Name, opt => opt.MapFrom(uvm => uvm.Name))
                .ForMember(u => u.Session, opt => opt.MapFrom(uvm => uvm.Session))
                .ReverseMap();
            
        }
    }
}
