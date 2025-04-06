using KST.Business.Notifications.Models;

namespace KST.Business.Notifications.Services;

public interface INotificationService
{
    Task SendUserAddedNotification(long userId, string password);
}