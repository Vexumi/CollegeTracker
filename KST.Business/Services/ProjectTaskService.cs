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

public class ProjectTaskService: BaseService<ProjectTask>, IProjectTaskService
{
    private readonly KSTDbContext dbContext;
    private readonly IMapper mapper;

    public ProjectTaskService(
        KSTDbContext dbContext,
        IMapper mapper
    ): base(dbContext)
    {
        this.dbContext = dbContext;
        this.mapper = mapper;
    }

    public async Task<long> CreateAsync(ProjectTaskModificationDTO dto, CancellationToken cancellationToken)
    {
        var task = mapper.Map<ProjectTask>(dto);
        var entity = await dbContext.ProjectTasks.AddAsync(task, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
        return entity.Entity.Id;
    }

    public async Task<ProjectTask> GetByIdAsync(long id, CancellationToken cancellationToken)
    {
        return await GetAll().FirstAsync(x => x.Id == id, cancellationToken);
    }

    public IQueryable<ProjectTask> GetAll()
        => dbContext.ProjectTasks
            .AsNoTracking()
            .Include(x => x.AssignedTo).ThenInclude(x => x.UserInfo)
            .Include(x => x.Project)
            .AsQueryable();

    public async Task DeleteAsync(long id, CancellationToken cancellationToken)
    {
        var entity = await dbContext.ProjectTasks.FirstAsync(x => x.Id == id, cancellationToken);
        dbContext.ProjectTasks.Remove(entity);

        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<ProjectTask> UpdateAsync(ProjectTaskModificationDTO dto, CancellationToken cancellationToken)
    {
        var group = mapper.Map<ProjectTask>(dto);
        dbContext.Attach(group);
        dbContext.Entry(group).State = EntityState.Modified;
        await dbContext.SaveChangesAsync(cancellationToken);
        return group;
    }

    public async Task<long> ChangeState(long id, TaskState state, CancellationToken cancellationToken)
    {
        var task = await dbContext.Set<ProjectTask>().AsTracking().FirstAsync(x => x.Id == id, cancellationToken);
        task.State = state;
        await dbContext.SaveChangesAsync(cancellationToken);
        return id;
    }
}