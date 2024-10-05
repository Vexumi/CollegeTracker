using CollegeTracker.Business.ViewModels;
using CollegeTracker.DataAccess.Models;

namespace CollegeTracker.Business.Interfaces;

public interface IGroupService
{
    Task<long> CreateAsync(GroupModificationDTO dto, CancellationToken cancellationToken);

    Task<Group> GetByIdAsync(long id, CancellationToken cancellationToken);

    IQueryable<Group> GetAll();

    Task DeleteAsync(long id, CancellationToken cancellationToken);

    Task<Group> UpdateAsync(GroupModificationDTO group, CancellationToken cancellationToken);
}