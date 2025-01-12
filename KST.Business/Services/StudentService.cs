using AutoMapper;
using KST.DataAccess;
using KST.Business.Interfaces;
using KST.Business.ViewModels;
using KST.DataAccess;
using KST.DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace KST.Business.Services;

public class StudentService: IStudentService
{
    
    private readonly KSTDbContext dbContext;
    private readonly IUserService userService;
    private readonly IMapper mapper;

    public StudentService(
        KSTDbContext dbContext,
        IUserService userService,
        IMapper mapper
    )
    {
        this.dbContext = dbContext;
        this.userService = userService;
        this.mapper = mapper;
    }

    public async Task<long> CreateAsync(StudentModificationDTO studentViewModel, CancellationToken cancellationToken)
    {
        var userProfileId = await userService.CreateAsync(studentViewModel.UserInfo, cancellationToken);
        var student = new Student()
        {
            UserInfoId = userProfileId,
            GroupId = studentViewModel.GroupId
        };
        await dbContext.Students.AddAsync(student, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        return student.Id;
    }

    public async Task<StudentViewModel> GetByIdAsync(long id, CancellationToken cancellationToken)
    {
        var student = await GetAllStudents().FirstAsync(x => x.Id == id, cancellationToken);
        return mapper.Map<StudentViewModel>(student);
    }

    public IEnumerable<StudentViewModel> GetAll()
        => GetAllStudents().Select(mapper.Map<StudentViewModel>);

    public async Task DeleteAsync(long id, CancellationToken cancellationToken)
    {
        var entity = await dbContext.Students.FirstAsync(x => x.Id == id, cancellationToken);
        dbContext.Students.Remove(entity);

        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<StudentViewModel> UpdateAsync(StudentModificationDTO entity, CancellationToken cancellationToken)
    {
        var student = mapper.Map<Student>(entity);
        dbContext.Attach(student);
        dbContext.Entry(student).State = EntityState.Modified;
        await dbContext.SaveChangesAsync(cancellationToken);

        return await GetByIdAsync(student.Id, cancellationToken);
    }

    public async Task ChangeStudentGroup(long studentId, long groupId, CancellationToken cancellationToken)
    {
        var student = await dbContext.Students.Include(x => x.Group).FirstAsync(x => x.Id == studentId, cancellationToken);
        var group = await dbContext.Groups.AsNoTracking().FirstAsync(x => x.Id == groupId, cancellationToken);

        student.GroupId = groupId;

        await dbContext.SaveChangesAsync(cancellationToken);
    }
    
    private IQueryable<Student> GetAllStudents()
        => dbContext.Students
            .Include(x => x.UserInfo)
            .Include(x => x.Group)
            .AsQueryable();
}