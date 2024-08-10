using CollegeTracker.Business.Interfaces;
using CollegeTracker.DataAccess;
using CollegeTracker.DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace CollegeTracker.Business.Services;

public class GroupService: IGroupService
{
    private readonly CollegeTrackerDbContext dbContext;

    public GroupService(
        CollegeTrackerDbContext dbContext
    )
    {
        this.dbContext = dbContext;
    }

    public async Task<long> CreateAsync(Group group, CancellationToken cancellationToken)
    {
        var entity = await dbContext.Groups.AddAsync(group, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        return entity.Entity.Id;
    }

    public async Task<Group> GetByIdAsync(long id, CancellationToken cancellationToken)
    {
        return await GetAll().FirstAsync(x => x.Id == id, cancellationToken);
    }

    public IQueryable<Group> GetAll()
        => dbContext.Groups.AsQueryable();

    public async Task DeleteAsync(long id, CancellationToken cancellationToken)
    {
        var entity = await dbContext.Groups.FirstAsync(x => x.Id == id, cancellationToken);
        dbContext.Groups.Remove(entity);

        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<Group> UpdateAsync(Group group, CancellationToken cancellationToken)
    {
        dbContext.Attach(group);
        dbContext.Entry(group).State = EntityState.Modified;
        await dbContext.SaveChangesAsync(cancellationToken);

        return group;
    }
}