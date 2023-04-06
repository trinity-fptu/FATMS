using Application.ViewModels.ClassViewModels;
using AutoMapper;
using Domain.Models;
using Domain.Models.Users;

namespace Infracstructures.Mappers
{
    public partial class MapperConfigs : Profile
    {
        partial void AddClassUserConfig()
        {
            CreateMap<User, ClassUserViewModel>().ReverseMap();

            //ClassUser entity class to AddUserToClassViewModel and reverse
            CreateMap<ClassUsers, AddUserToClassViewModel>().ReverseMap();
        }
    }
}
