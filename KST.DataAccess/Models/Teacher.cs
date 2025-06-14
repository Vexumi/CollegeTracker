namespace KST.DataAccess.Models;

public class Teacher: BaseEntity
{
    public long UserInfoId { get; set; }
    
    public User UserInfo { get; set; } = null!;
    
    // группы избранные учителем
    public virtual ICollection<Group> Groups { get; set; } = new List<Group>();
}