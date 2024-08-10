using CollegeTracker.DataAccess.Models;

namespace CollegeTracker.Business.Interfaces;

public interface ISpecialityService
{
    Task<long> CreateAsync(Speciality speciality, CancellationToken cancellationToken);

    Task<Speciality> GetByIdAsync(long id, CancellationToken cancellationToken);

    IQueryable<Speciality> GetAll();

    Task DeleteAsync(long id, CancellationToken cancellationToken);

    Task<Speciality> UpdateAsync(Speciality speciality, CancellationToken cancellationToken);
}