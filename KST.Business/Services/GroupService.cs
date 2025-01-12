using AutoMapper;
using KST.Business.Infrastructure;
using KST.DataAccess;
using KST.Business.Interfaces;
using KST.Business.ViewModels;
using KST.DataAccess;
using KST.DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace KST.Business.Services;

public class GroupService: BaseService<Group>, IGroupService
{
    private readonly KSTDbContext dbContext;
    private readonly IMapper mapper;

    public GroupService(
        KSTDbContext dbContext,
        IMapper mapper
    ): base(dbContext)
    {
        this.dbContext = dbContext;
        this.mapper = mapper;
    }

    public async Task<long> CreateAsync(GroupModificationDTO dto, CancellationToken cancellationToken)
    {
        var group = mapper.Map<Group>(dto);
        var entity = await dbContext.Groups.AddAsync(group, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        return entity.Entity.Id;
    }

    public async Task<Group> GetByIdAsync(long id, CancellationToken cancellationToken)
    {
        return await GetAll().FirstAsync(x => x.Id == id, cancellationToken);
    }

    public IQueryable<Group> GetAll()
        => dbContext.Groups
            .AsNoTracking()
            .Include(x => x.Speciality)
            .Include(x => x.Subjects)
            .Include(x => x.Teachers)
            .AsSplitQuery()
            .AsQueryable();

    public async Task DeleteAsync(long id, CancellationToken cancellationToken)
    {
        var entity = await dbContext.Groups.FirstAsync(x => x.Id == id, cancellationToken);
        dbContext.Groups.Remove(entity);

        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<Group> UpdateAsync(GroupModificationDTO dto, CancellationToken cancellationToken)
    {
        var group = mapper.Map<Group>(dto);
        dbContext.Attach(group);
        dbContext.Entry(group).State = EntityState.Modified;
        
        await dbContext.SaveChangesAsync(cancellationToken);

        return group;
    }
}