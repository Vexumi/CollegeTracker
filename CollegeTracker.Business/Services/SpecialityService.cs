using CollegeTracker.Business.Interfaces;
using CollegeTracker.DataAccess;
using CollegeTracker.DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace CollegeTracker.Business.Services;

public class SpecialityService: ISpecialityService
{
    private readonly CollegeTrackerDbContext dbContext;

    public SpecialityService(
        CollegeTrackerDbContext dbContext
    )
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