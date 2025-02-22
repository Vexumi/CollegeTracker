using KST.DataAccess.Enums;

namespace KST.DataAccess.Models;

public class ProjectTask: BaseEntity
{
    public string Title { get; set; }
    public string? Description { get; set; }
    public int EstimatedHours { get; set; }
    public int? ActualHours { get; set; }
    public DateTime? InProgressSince { get; set; }
    public long ProjectId { get; set; }
    public Project Project { get; set; }
    public long AssignedToId { get; set; }
    public Student AssignedTo { get; set; }
    public TaskState State { get; set; }
}