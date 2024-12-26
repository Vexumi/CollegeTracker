using AutoMapper;
using CollegeTracker.Business.ViewModels;
using CollegeTracker.DataAccess.Models;

namespace CollegeTracker.Business.Infrastructure.MapperConfigs;

public class UserMapperProfile: Profile
{
    public UserMapperProfile()
    {
        CreateMap<User, UserViewModel>().ForMember(x => x.Password, x => x.Ignore());
        CreateMap<UserViewModel, User>().ForMember(x => x.PasswordHash, x => x.MapFrom(s => s.Password));
    }
}