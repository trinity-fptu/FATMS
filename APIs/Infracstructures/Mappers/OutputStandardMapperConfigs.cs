using Application.ViewModels.OutputStandardViewModels;
using Application.ViewModels.TrainingDeliveryPrincipleViewModels;
using AutoMapper;
using Domain.Models;

namespace Infracstructures.Mappers
{
    public partial class MapperConfigs : Profile
    {
        partial void AddOutputStandardConfig()
        {
            CreateMap<OutputStandard, OutputStandardViewModel>().ReverseMap();

        }
    }
}
