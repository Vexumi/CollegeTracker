using KST.DataAccess.Models;

namespace KST.Business.ViewModels;

public class MessageDTO: BaseEntity
{
    public string Content { get; set; }
    public long ProjectId { get; set; }
    public long SenderId { get; set; }
}