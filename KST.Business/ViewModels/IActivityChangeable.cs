namespace KST.Business.ViewModels;

public interface IActivityChangeable
{
    Task ChangeActivityState(long entityId, CancellationToken cancellationToken);
}