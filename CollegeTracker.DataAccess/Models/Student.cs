namespace CollegeTracker.DataAccess.Models;

public class Student: BaseEntity
{
    public long UserId { get; set; }
    
    public User User { get; set; } = null!;
    
    public long GroupId { get; set; }

    public Group Group { get; set; } = null!;
}