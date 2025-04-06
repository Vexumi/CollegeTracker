using KST.Business.Notifications.Models;

namespace KST.Business.Notifications.Services;

public interface INotificationTemplateRendererService
{
    Task<string> RenderTemplateAsync(BaseNotification notification);
}