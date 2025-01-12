using KST.Business.Infrastructure;
using KST.DataAccess;
using KST.Business.Interfaces;
using KST.DataAccess;
using KST.DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace KST.Business.Services;

public class SpecialityService: BaseService<Speciality>, ISpecialityService
{
    private readonly KSTDbContext dbContext;

    public SpecialityService(
        KSTDbContext dbContext
    ): base(dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task<long> CreateAsync(Speciality speciality, CancellationToken cancellationToken)
    {
        var entity = await dbContext.Specialities.AddAsync(speciality, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        return entity.Entity.Id;
    }

    public async Task<Speciality> GetByIdAsync(long id, CancellationToken cancellationToken)
    {
        return await GetAll().FirstAsync(x => x.Id == id, cancellationToken);
    }

    public IQueryable<Speciality> GetAll()
        => dbContext.Specialities.AsQueryable();

    public async Task DeleteAsync(long id, CancellationToken cancellationToken)
    {
        var speciality = await dbContext.Specialities.FirstAsync(x => x.Id == id, cancellationToken);
        dbContext.Specialities.Remove(speciality);

        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<Speciality> UpdateAsync(Speciality speciality, CancellationToken cancellationToken)
    {
        dbContext.Attach(speciality);
        dbContext.Entry(speciality).State = EntityState.Modified;
        await dbContext.SaveChangesAsync(cancellationToken);

        return speciality;
    }
}