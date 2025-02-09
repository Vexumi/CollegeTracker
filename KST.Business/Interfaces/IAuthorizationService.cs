using KST.Business.ViewModels;
using KST.WEB.Infrastructure;

namespace KST.Business.Interfaces;

public interface IAuthorizationService
{
    Task<JwtTokenResponse?> AuthorizeByLoginAndPassword(string login, string password, CancellationToken cancellationToken);
    string HashPassword(string password);
    bool ValidateHashedPassword(string hash, string password);
    UserViewModel GetCurrentUser();
}