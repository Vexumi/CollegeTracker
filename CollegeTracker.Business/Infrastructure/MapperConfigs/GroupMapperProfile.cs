using AutoMapper;
using CollegeTracker.Business.ViewModels;
using CollegeTracker.DataAccess.Models;

namespace CollegeTracker.Business.Infrastructure.MapperConfigs;

public class GroupMapperProfile: Profile
{
    public GroupMapperProfile()
    {
        CreateMap<GroupModificationDTO, Group>();
    }
}