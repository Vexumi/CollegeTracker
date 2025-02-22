using KST.DataAccess.Enums;

namespace KST.Business.ViewModels;

public class ProjectModificationDTO: BaseViewModel
{
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public ProjectState State { get; set; } = ProjectState.Created;
    public long TeacherId { get; set; }
    public long SubjectId { get; set; }
    public DateTime StartDate { get; set; } = DateTime.Now;
    public DateTime? Deadline { get; set; }
}