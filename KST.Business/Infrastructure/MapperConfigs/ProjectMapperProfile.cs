using AutoMapper;
using KST.Business.ViewModels;
using KST.DataAccess.Models;

namespace KST.Business.Infrastructure.MapperConfigs;

public class ProjectMapperProfile: Profile
{
    public ProjectMapperProfile()
    {
        CreateMap<ProjectModificationDTO, Project>();
        CreateMap<ProjectCreateDTO, Project>();
    }
}