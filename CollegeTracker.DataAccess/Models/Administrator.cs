namespace CollegeTracker.DataAccess.Models;

public class Administrator: BaseEntity
{
    public long UserInfoId { get; set; }
    
    public User UserInfo { get; set; }
}