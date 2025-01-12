using AutoMapper;
using KST.Business.ViewModels;
using KST.DataAccess.Models;

namespace KST.Business.Infrastructure.MapperConfigs;

public class UserMapperProfile: Profile
{
    public UserMapperProfile()
    {
        CreateMap<User, UserViewModel>().ForMember(x => x.Password, x => x.Ignore());
        CreateMap<UserViewModel, User>().ForMember(x => x.PasswordHash, x => x.MapFrom(s => s.Password));
    }
}