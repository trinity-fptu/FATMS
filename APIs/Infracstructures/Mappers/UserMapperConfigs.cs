using Application.ViewModels.UserViewModels;
using AutoMapper;
using Domain.Enums.UserEnums;
using Domain.Models.Users;
using System.Globalization;

namespace Infracstructures.Mappers
{
    public partial class MapperConfigs : Profile
    {
        partial void AddUserMapperConfig()
        {
            // Map User entity class to User detail model
            CreateMap<User, UserDetailModel>()
                .ForMember(des => des.Role, src => src.MapFrom(x => x.Role.Name))
                .ForMember(des => des.DateOfBirth, src => src.MapFrom(x => x.DateOfBirth.ToString("dd/MM/yyyy")))
                .ForMember(des => des.Role, src => src.MapFrom(x => x.Role.Name));

            // Map User entity class to User list model
            CreateMap<User, UserListModel>()
                .ForMember(des => des.Role, act => act.MapFrom(src => src.Role.Name))
                .ForMember(des => des.DateOfBirth, src => src.MapFrom(x => x.DateOfBirth.ToString("dd/MM/yyyy")));

            // Map User entity class to User list model
            CreateMap<UserDetailModel, User>()
                .ForMember(des => des.DateOfBirth, src => src.MapFrom(x => DateTime.Parse(x.DateOfBirth)));

            //create map from UserCreateModel to User
            CreateMap<UserCreateModel, User>()
                .ForMember(des => des.Role, act => act.Ignore())
                .ForMember(des => des.Status,
                    act => act.MapFrom(src => (UserStatus)Enum.Parse(typeof(UserStatus), src.Status)))
                .ForMember(des => des.Level,
                    act => act.MapFrom(src => (UserLevel)Enum.Parse(typeof(UserLevel), src.Level)));

            // Map from User to UserLoginModel
            CreateMap<User, UserLoginModel>();

            // Map User entity class to User update model
            CreateMap<UserUpdateModel, User>()
                .ForMember(des => des.DateOfBirth,
                    src => src.MapFrom(x =>
                        DateTime.ParseExact(x.DateOfBirth, "dd/MM/yyyy", CultureInfo.InvariantCulture)))
                .ForMember(des => des.IsMale, src => src.MapFrom(x => bool.Parse(x.IsMale)))
                .ForMember(des => des.Level, src => src.MapFrom(x => Enum.Parse(typeof(UserLevel), x.Level, true)))
                .ForMember(des => des.Status, src => src.MapFrom(x => Enum.Parse(typeof(UserStatus), x.Status, true)));

            CreateMap<UserFilterModel, UserListModel>().ReverseMap();
        }
    }
}
