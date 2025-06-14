using KST.DataAccess.Models;

namespace KST.Business.Notifications.Services;

public interface IHangfireNotificationService
{
    void SendUserAddedNotification(long userId, string password);
    void SendProjectStateChangedNotification(long project);
    void SendProjectMarkAddedNotification(long project);
}