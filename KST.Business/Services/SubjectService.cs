using KST.Business.ViewModels;
using KST.DataAccess;
using KST.Business.Interfaces;
using KST.DataAccess;
using KST.DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace KST.Business.Services;

public class SubjectService: ISubjectService
{
    private readonly KSTDbContext dbContext;

    public SubjectService(
        KSTDbContext dbContext
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

    public async Task ChangeActivityState(long entityId, CancellationToken cancellationToken)
    {
        var entity = await dbContext.Subjects.FirstAsync(x => x.Id == entityId, cancellationToken);
        entity.IsActive = !entity.IsActive;
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}