namespace KST.DataAccess.Models;

public class Message: BaseEntity
{
    public string Content { get; set; } = string.Empty;
    public DateTime SendDate { get; set; }
    public long? SenderId { get; set; }
    public User? Sender { get; set; }
    public long ProjectId { get; set; }
    public Project Project { get; set; }
    public bool SystemEvent { get; set; } = false;
}