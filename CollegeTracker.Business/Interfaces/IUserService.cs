using CollegeTracker.Business.ViewModels;

namespace CollegeTracker.Business.Interfaces;

public interface IUserService
{
    Task<long> CreateAsync(UserViewModel user, CancellationToken cancellationToken);

    Task<UserViewModel> GetByIdAsync(long id, CancellationToken cancellationToken);

    IEnumerable<UserViewModel> GetAll();

    Task DeleteAsync(long id, CancellationToken cancellationToken);

    Task<UserViewModel> UpdateAsync(UserViewModel user, CancellationToken cancellationToken);
}