using KST.Business.Infrastructure;
using KST.Business.Notifications.Models;
using KST.DataAccess;
using KST.DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace KST.Business.Notifications.Services;

public class NotificationService(
    INotificationTemplateRendererService templateService, 
    ISmtpService smtpService, 
    KSTDbContext context,
    IOptions<AuthOptions> authOptions) : INotificationService
{
    public async Task UserAdded(long userId, string password)
    {
        var user = await context.Set<User>().FirstAsync(x => x.Id == userId);
        var userAddedNotification = new UserAddedNotification()
        {
            Fullname = user.Fullname,
            UserName = user.Username,
            Email = user.Email,
            Password = password,
            FrontendLink = authOptions.Value.AUDIENCE
        };

        await SendNotificationAsync(userAddedNotification, new List<string> { user.Email });
    }
    
    public async Task ProjectStateChanged(long projectId)
    {
        var project = await context.Set<Project>()
            .Include(x => x.Students).ThenInclude(x => x.UserInfo)
            .Include(x => x.Teacher).ThenInclude(x => x.UserInfo)
            .FirstAsync(x => x.Id == projectId);
        var notification = new ProjectStateChangedNotification()
        {
            NewStatus = project.State.ToString(),
            ProjectId = project.Id,
            ProjectName = project.Title,
            FrontendLink = authOptions.Value.AUDIENCE
        };

        var emails = project.Students.Select(x => x.UserInfo.Email).ToList();
        emails.Add(project.Teacher.UserInfo.Email);

        await SendNotificationAsync(notification, emails);
    }
    
    public async Task ProjectMarkAdded(long projectId)
    {
        var project = await context.Set<Project>()
            .Include(x => x.Students).ThenInclude(x => x.UserInfo)
            .Include(x => x.Teacher).ThenInclude(x => x.UserInfo)
            .FirstAsync(x => x.Id == projectId);
        var notification = new ProjectMarkAddedNotification()
        {
            ProjectName = project.Title,
            Mark = project.Mark.ToString() ?? "0",
            ProjectId = project.Id,
            FrontendLink = authOptions.Value.AUDIENCE
        };

        var emails = project.Students.Select(x => x.UserInfo.Email).ToList();
        emails.Add(project.Teacher.UserInfo.Email);

        await SendNotificationAsync(notification, emails);
    }
    
    private async Task SendNotificationAsync(BaseNotification notification, IList<string> addresses)
    {
        var message = await templateService.RenderTemplateAsync(notification);
        var emailMessage = new EmailMessage
        {
            Subject = notification.Subject,
            Message = message,
            Recipients = addresses,
        };
        await smtpService.SendEmailAsync(emailMessage);
    }

    private async Task SendNotificationsAsync(List<(BaseNotification, IList<string>)> notifications)
    {
        foreach (var (notification, recipients) in notifications)
        {
            await SendNotificationAsync(notification, recipients);
        }
    }
}