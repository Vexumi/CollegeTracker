using AutoMapper;
using KST.Business.ViewModels;
using KST.DataAccess.Models;

namespace KST.Business.Infrastructure.MapperConfigs;

public class ProjectTaskMapperProfile: Profile
{
    public ProjectTaskMapperProfile()
    {
        CreateMap<ProjectTaskModificationDTO, ProjectTask>();
    }
}