using KST.DataAccess.Enums;

namespace KST.Business.ViewModels;

public class ProjectModificationDTO: BaseViewModel
{
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public long TeacherId { get; set; }
    public long SpecialityId { get; set; }
    public DateOnly StartDate { get; set; } = DateOnly.FromDateTime(DateTime.Now);
    public DateOnly? Deadline { get; set; }
}