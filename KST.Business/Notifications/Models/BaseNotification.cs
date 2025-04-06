namespace KST.Business.Notifications.Models;

public abstract class BaseNotification
{
    public abstract string Subject { get; }
    public string UserEmail { get; set; } = string.Empty;
    public abstract string TemplatePath { get; }
}