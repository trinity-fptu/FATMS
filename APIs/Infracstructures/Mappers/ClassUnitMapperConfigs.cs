using Application.ViewModels.ClassUnitViewModel;
using Application.ViewModels.ClassViewModels;
using AutoMapper;
using Domain.Models;

namespace Infracstructures.Mappers
{
    public partial class MapperConfigs : Profile
    {
        partial void AddClassUnitConfig()
        {
            //Map ClassUnitDetail entity class to EditClassUnitViewModel and reverse
            CreateMap<ClassUnitDetail, EditClassUnitViewModel>().ReverseMap();

            //Map ClassUnitDetail entity class to AddClassUnitViewModel and reverse
            CreateMap<ClassUnitDetail, AddClassUnitViewModel>().ReverseMap();
        }
    }
}
