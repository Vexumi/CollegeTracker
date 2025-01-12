using KST.Business.ViewModels;

namespace KST.Business.Interfaces;

public interface IAuthorizationService
{
    string GetPasswordHash(string password);

    UserViewModel GetCurrentUser();
}