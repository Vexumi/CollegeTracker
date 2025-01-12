namespace KST.DataAccess.Models;

public class Student: BaseEntity
{
    public long UserInfoId { get; set; }
    
    public User UserInfo { get; set; } = null!;
    
    public long GroupId { get; set; }

    public Group Group { get; set; } = null!;
}