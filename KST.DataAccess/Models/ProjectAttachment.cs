namespace KST.DataAccess.Models;

public class ProjectAttachment: BaseEntity
{
    public long ProjectId { get; set; }
    public Project Project { get; set; }
    public string Name { get; set; }
    public string? FilePath { get; set; }
    public string? ContentType { get; set; }
    public string? Url { get; set; }
}