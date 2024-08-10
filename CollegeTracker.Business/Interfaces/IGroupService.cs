using CollegeTracker.DataAccess.Models;

namespace CollegeTracker.Business.Interfaces;

public interface IGroupService
{
    Task<long> CreateAsync(Group group, CancellationToken cancellationToken);

    Task<Group> GetByIdAsync(long id, CancellationToken cancellationToken);

    IQueryable<Group> GetAll();

    Task DeleteAsync(long id, CancellationToken cancellationToken);

    Task<Group> UpdateAsync(Group group, CancellationToken cancellationToken);
}