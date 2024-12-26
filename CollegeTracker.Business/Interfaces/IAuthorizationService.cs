using CollegeTracker.Business.ViewModels;

namespace CollegeTracker.Business.Interfaces;

public interface IAuthorizationService
{
    string GetPasswordHash(string password);

    UserViewModel GetCurrentUser();
}