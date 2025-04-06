namespace KST.Business.Notifications.Services;

public interface IHangfireNotificationService
{
    void SendUserAddedNotification(long userId, string password);
}