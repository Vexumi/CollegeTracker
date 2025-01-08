namespace CollegeTracker.Business.ViewModels;

public class BaseViewModel
{
    public long? Id { get; set; }
    public bool IsActive { get; set; } = true;
}