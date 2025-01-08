namespace CollegeTracker.Business.ViewModels;

public interface IActivityChangeable
{
    Task ChangeActivityState(long entityId, CancellationToken cancellationToken);
}