using CollegeTracker.DataAccess.Models;

namespace CollegeTracker.Business.Interfaces;

public interface ISubjectService
{
    Task<long> CreateAsync(Subject entity, CancellationToken cancellationToken);

    Task<Subject> GetByIdAsync(long id, CancellationToken cancellationToken);

    IQueryable<Subject> GetAll();

    Task DeleteAsync(long id, CancellationToken cancellationToken);

    Task<Subject> UpdateAsync(Subject entity, CancellationToken cancellationToken);
}