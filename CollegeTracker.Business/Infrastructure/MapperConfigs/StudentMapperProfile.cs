using AutoMapper;
using CollegeTracker.Business.ViewModels;
using CollegeTracker.DataAccess.Models;

namespace CollegeTracker.Business.Infrastructure.MapperConfigs;

public class StudentMapperProfile: Profile
{
    public StudentMapperProfile()
    {
        CreateMap<Student, StudentViewModel>();
        CreateMap<StudentViewModel, Student>();
        CreateMap<StudentModificationDTO, Student>();
    }
}