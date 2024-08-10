using AutoMapper;
using CollegeTracker.Business.ViewModels;
using CollegeTracker.DataAccess.Models;

namespace CollegeTracker.Business.Infrastructure.MapperConfigs;

public class UserMapperProfile: Profile
{
    public UserMapperProfile()
    {
        CreateMap<User, UserViewModel>();
        CreateMap<UserViewModel, User>();
    }
}