using KST.DataAccess.Enums;

namespace KST.Business.ViewModels;

public class UserViewModel: BaseViewModel
{
    public string? Email { get; set; }
    
    public string? PhoneNumber { get; set; }

    public string? Fullname { get; set; }

    public string? Username { get; set; }
    
    public string? Password { get; set; }
    
    public UserRoles? Role { get; set; }
}