using AutoMapper;
using KST.Business.Infrastructure;
using KST.DataAccess;
using KST.Business.Interfaces;
using KST.Business.ViewModels;
using KST.DataAccess;
using KST.DataAccess.Enums;
using KST.DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace KST.Business.Services;

public class TeacherService: BaseService<Teacher>, ITeacherService
{
    
    private readonly KSTDbContext dbContext;
    private readonly IUserService userService;
    private readonly IMapper mapper;

    public TeacherService(
        KSTDbContext dbContext,
        IUserService userService,
        IMapper mapper
    ): base(dbContext)
    {
        this.dbContext = dbContext;
        this.userService = userService;
        this.mapper = mapper;
    }

    public async Task<long> CreateAsync(TeacherModificationDTO viewModel, CancellationToken cancellationToken)
    {
        viewModel.UserInfo.Role = UserRoles.Teacher;
        var userProfileId = await userService.CreateAsync(viewModel.UserInfo, cancellationToken);
        var groups = await dbContext.Groups.Where(x => viewModel.GroupIds.Contains(x.Id))
            .ToListAsync(cancellationToken);
        
        var teacher = new Teacher()
        {
            UserInfoId = userProfileId,
            Groups = groups
        };
        
        await dbContext.Teachers.AddAsync(teacher, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        return teacher.Id;
    }

    public async Task<TeacherViewModel> GetByIdAsync(long id, CancellationToken cancellationToken)
    {
        var entity = await GetAllTeachers().FirstAsync(x => x.Id == id, cancellationToken);
        return mapper.Map<TeacherViewModel>(entity);
    }

    public IEnumerable<TeacherViewModel> GetAll()
        => GetAllTeachers().Select(mapper.Map<TeacherViewModel>);

    public async Task DeleteAsync(long id, CancellationToken cancellationToken)
    {
        var entity = await dbContext.Teachers.FirstAsync(x => x.Id == id, cancellationToken);
        dbContext.Teachers.Remove(entity);

        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<TeacherViewModel> UpdateAsync(TeacherModificationDTO entity, CancellationToken cancellationToken)
    {        
        var teacher = await dbContext.Teachers.Include(x => x.UserInfo).Include(x => x.Groups).FirstAsync(x => x.Id == entity.Id, cancellationToken);
        teacher.UserInfo.Email = entity.UserInfo.Email;
        teacher.UserInfo.Fullname = entity.UserInfo.Fullname;
        teacher.UserInfo.PhoneNumber = entity.UserInfo.PhoneNumber;
        teacher.UserInfo.Username = entity.UserInfo.Username;
        teacher.Groups =
            await dbContext.Groups.Where(x => entity.GroupIds.Contains(x.Id)).ToListAsync(cancellationToken);

        await dbContext.SaveChangesAsync(cancellationToken);
        return mapper.Map<TeacherViewModel>(teacher);
    }
    
    private IQueryable<Teacher> GetAllTeachers()
        => dbContext.Teachers
            .Include(x => x.UserInfo)
            .Include(x => x.Groups)
            .AsQueryable();
}