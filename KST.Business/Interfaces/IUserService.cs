using KST.Business.ViewModels;
using KST.DataAccess.Models;

namespace KST.Business.Interfaces;

public interface IUserService
{
    Task<long> CreateAsync(UserViewModel user, CancellationToken cancellationToken);

    Task<UserViewModel> GetByIdAsync(long id, CancellationToken cancellationToken);
    Task<bool> ChangePassword(string newPassword, string oldPassword, CancellationToken cancellationToken);

    IEnumerable<UserViewModel> GetAll();

    Task DeleteAsync(long id, CancellationToken cancellationToken);

    Task<UserViewModel> UpdateAsync(UserViewModel user, CancellationToken cancellationToken);

    IQueryable<User> GetAllUsers();
}