using AutoMapper;
using KST.Business.ViewModels;
using KST.DataAccess.Models;

namespace KST.Business.Infrastructure.MapperConfigs;

public class TeacherMapperProfile: Profile
{
    public TeacherMapperProfile()
    {
        CreateMap<Teacher, TeacherViewModel>();
        CreateMap<TeacherViewModel, Teacher>();
    }
}