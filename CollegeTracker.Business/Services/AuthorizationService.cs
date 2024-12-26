using System.Security.Claims;
using CollegeTracker.Business.Interfaces;
using CollegeTracker.Business.ViewModels;
using CollegeTracker.DataAccess.Enums;
using CollegeTracker.DataAccess.Models;
using Microsoft.AspNetCore.Http;

namespace CollegeTracker.Business.Services;

public class AuthorizationService(IHttpContextAccessor httpContextAccessor): IAuthorizationService
{
    public string GetPasswordHash(string password)
    {
        return password; //TODO hash generator
    }
    
    public UserViewModel GetCurrentUser()
    {
        var claims = httpContextAccessor.HttpContext.User.Claims;
        UserRoles.TryParse(claims.First(x => x.Type == ClaimTypes.Role).Value, out UserRoles role); 
        var user = new UserViewModel()
        {
            Id = long.Parse(claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value as string),
            Username = claims.First(x => x.Type == "name").Value,
            PhoneNumber = claims.First(x => x.Type == "phone").Value,
            Fullname = claims.First(x => x.Type == "given_name").Value,
            Email = claims.First(x => x.Type == ClaimTypes.Email).Value,
            Role = role
        };

        return user;
    }
}