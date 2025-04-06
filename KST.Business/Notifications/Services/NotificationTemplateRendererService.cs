using KST.Business.Notifications.Models;

namespace KST.Business.Notifications.Services;

public class NotificationTemplateRendererService(RazorTemplateRenderer razorViewRenderer) : INotificationTemplateRendererService
{
    public async Task<string> RenderTemplateAsync(BaseNotification notification)
    {
        return await razorViewRenderer.RenderAsync(notification.TemplatePath, notification);
    }
}