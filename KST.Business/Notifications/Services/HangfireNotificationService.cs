using Hangfire;

namespace KST.Business.Notifications.Services;

public class HangfireNotificationService(IBackgroundJobClient backgroundJobClient): IHangfireNotificationService
{
    public void SendUserAddedNotification(long userId, string password)
    {
        backgroundJobClient.Enqueue<INotificationService>(x => x.SendUserAddedNotification(userId, password));
    }
}