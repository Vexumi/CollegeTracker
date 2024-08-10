using CollegeTracker.Business.Interfaces;

namespace CollegeTracker.Business.Services;

public class AuthorizationService: IAuthorizationService
{
    public AuthorizationService()
    {
        
    }
    public string GetPasswordHash(string password)
    {
        return password; //TODO hash generator
    }
}