using CollegeTracker.DataAccess.Enums;

namespace CollegeTracker.DataAccess.Models;

public class User: BaseEntity
{
    public string? Email { get; set; }
    
    public string? PhoneNumber { get; set; }
    
    public string Fullname { get; set; }
    
    public string Username { get; set; }
    
    public string PasswordHash { get; set; }
    
    public UserRoles Role { get; set; }
}