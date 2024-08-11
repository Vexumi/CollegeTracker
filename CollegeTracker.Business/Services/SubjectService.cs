using CollegeTracker.Business.Interfaces;
using CollegeTracker.DataAccess;
using CollegeTracker.DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace CollegeTracker.Business.Services;

public class SubjectService: ISubjectService
{
    private readonly CollegeTrackerDbContext dbContext;

    public SubjectService(
        CollegeTrackerDbContext dbContext
    )
    {
        this.dbContext = dbContext;
    }

    public async Task<long> CreateAsync(Subject subject, CancellationToken cancellationToken)
    {
        var entity = await dbContext.Subjects.AddAsync(subject, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        return entity.Entity.Id;
    }

    public async Task<Subject> GetByIdAsync(long id, CancellationToken cancellationToken)
    {
        return await GetAll().FirstAsync(x => x.Id == id, cancellationToken);
    }

    public IQueryable<Subject> GetAll()
        => dbContext.Subjects.AsQueryable();

    public async Task DeleteAsync(long id, CancellationToken cancellationToken)
    {
        var entity = await dbContext.Subjects.FirstAsync(x => x.Id == id, cancellationToken);
        dbContext.Subjects.Remove(entity);

        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<Subject> UpdateAsync(Subject entity, CancellationToken cancellationToken)
    {
        dbContext.Attach(entity);
        dbContext.Entry(entity).State = EntityState.Modified;
        await dbContext.SaveChangesAsync(cancellationToken);

        return entity;
    }
}