using KST.DataAccess.Enums;

namespace KST.DataAccess.Models;

public class User: BaseEntity
{
    public string? Email { get; set; }
    
    public string? PhoneNumber { get; set; }

    public string Fullname { get; set; } = null!;

    public string Username { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;
    
    public UserRoles Role { get; set; }
}