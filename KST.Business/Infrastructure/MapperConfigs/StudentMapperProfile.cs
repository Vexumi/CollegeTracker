using AutoMapper;
using KST.Business.ViewModels;
using KST.DataAccess.Models;

namespace KST.Business.Infrastructure.MapperConfigs;

public class StudentMapperProfile: Profile
{
    public StudentMapperProfile()
    {
        CreateMap<Student, StudentViewModel>();
        CreateMap<StudentViewModel, Student>();
        CreateMap<StudentModificationDTO, Student>();
    }
}