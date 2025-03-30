using KST.DataAccess.Enums;

namespace KST.Business.ViewModels;

public class ProjectSearchParamsDTO
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public ProjectState? State { get; set; }
    public long? SpecialityId { get; set; }
    public DateOnly? StartDateFrom { get; set; }
    public DateOnly? StartDateTo { get; set; }
    public DateOnly? ActualEndDateFrom { get; set; }
    public DateOnly? ActualEndDateTo { get; set; }
    public DateOnly? DeadlineFrom { get; set; }
    public DateOnly? DeadlineTo { get; set; }
    
    public long? CurrentUserId { get; set; }

    public int PageSize { get; set; }
    public int Page { get; set; }
}