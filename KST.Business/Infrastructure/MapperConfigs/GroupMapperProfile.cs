using AutoMapper;
using KST.Business.ViewModels;
using KST.DataAccess.Models;

namespace KST.Business.Infrastructure.MapperConfigs;

public class GroupMapperProfile: Profile
{
    public GroupMapperProfile()
    {
        CreateMap<GroupModificationDTO, Group>();
    }
}