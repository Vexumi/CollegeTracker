using KST.Business.Notifications.Models;
using KST.DataAccess.Models;

namespace KST.Business.Notifications.Services;

public interface INotificationService
{
    Task UserAdded(long userId, string password);

    Task ProjectStateChanged(long projectId);
    
    Task ProjectMarkAdded(long projectId);
}