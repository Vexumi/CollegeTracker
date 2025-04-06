using KST.Business.Notifications.Models;

namespace KST.Business.Notifications.Services;

public interface ISmtpService
{
    Task SendEmailAsync(EmailMessage emailMessage);
}