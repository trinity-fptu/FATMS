using Application.ViewModels.TrainingMaterialViewModels;
using AutoMapper;
using Domain.Models;
using System.Globalization;

namespace Infracstructures.Mappers
{
    public partial class MapperConfigs : Profile
    {
        partial void AddTrainingMaterialConfig()
        {
            // Map TrainingMaterial class to TrainingMaterialDetailModel class
            CreateMap<TrainingMaterial, TrainingMaterialDetailModel>()
                .ForMember(dest => dest.MaterialId, opt => opt.MapFrom(s => s.Id))
                .ForMember(des => des.CreatedOn, src => src.MapFrom(x => x.CreatedOn.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture)))
                .ReverseMap().ForMember(dest => dest.Id, opt => opt.MapFrom(s => s.MaterialId));
            
        }
    }
}
