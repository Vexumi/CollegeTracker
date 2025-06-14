using Hangfire;
using KST.DataAccess.Models;

namespace KST.Business.Notifications.Services;

public class HangfireNotificationService(IBackgroundJobClient backgroundJobClient): IHangfireNotificationService
{
    public void SendUserAddedNotification(long userId, string password)
    {
        backgroundJobClient.Enqueue<INotificationService>(x => x.UserAdded(userId, password));
    }

    public void SendProjectStateChangedNotification(long project)
    {
        backgroundJobClient.Enqueue<INotificationService>(x => x.ProjectStateChanged(project));
    }

    public void SendProjectMarkAddedNotification(long project)
    {
        backgroundJobClient.Enqueue<INotificationService>(x => x.ProjectMarkAdded(project));
    }
}