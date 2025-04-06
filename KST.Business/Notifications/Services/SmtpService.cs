using KST.Business.Infrastructure;
using KST.Business.Notifications.Models;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;

namespace KST.Business.Notifications.Services;

public class SmtpService(IOptions<EmailNotificationOptions> smtpSettings) : ISmtpService
{
    private readonly EmailNotificationOptions options = smtpSettings.Value;

    public async Task SendEmailAsync(EmailMessage emailMessage)
    {
        using var resultMessage = new MimeMessage
        {
            Sender = options.SenderMailbox,
            Subject = emailMessage.Subject,
            Body = emailMessage.Body
        };

        if (options.From.Any())
        {
            resultMessage.From.AddRange(options.FromAddresses);
        }

        var emails = emailMessage.Recipients.Where(recipient => !string.IsNullOrWhiteSpace(recipient));
        /*foreach (var email in emails)
        {
            resultMessage.To.Add(MailboxAddress.Parse(email));
        }*/
        resultMessage.To.AddRange(emails.Select(MailboxAddress.Parse));
        await SendWithSmtpClientAsync(resultMessage);
    }
    
    private async Task SendWithSmtpClientAsync(MimeMessage resultMessage)
    {
        if (!resultMessage.To.Any())
        {
            return;
        }
        using var smtpClient = new SmtpClient
        {
            Timeout = options.Timeout,
        };

        await smtpClient.ConnectAsync(options.Host, options.Port, options.ConnectionOptions);

        if (!string.IsNullOrWhiteSpace(options.UserName) && !string.IsNullOrWhiteSpace(options.Password))
        {
            await smtpClient.AuthenticateAsync(options.UserName, options.Password);
        }

        await smtpClient.SendAsync(resultMessage);
        await smtpClient.DisconnectAsync(true);
    }
}