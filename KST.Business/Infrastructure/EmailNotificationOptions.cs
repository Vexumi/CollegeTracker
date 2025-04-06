using MailKit.Security;
using MimeKit;

namespace KST.Business.Infrastructure;

public class EmailNotificationOptions
{
    public string Host { get; set; } = null!;
    public int Port { get; set; }
    public int Timeout { get; set; }
    public string Sender { get; set; } = null!;
    public MailboxAddress SenderMailbox => MailboxAddress.Parse(Sender);
    public string[] From { get; set; } = [];
    public IEnumerable<MailboxAddress> FromAddresses => From.Select(MailboxAddress.Parse);
    public string UserName { get; set; } = null!;
    public string Password { get; set; } = null!;
    public SecureSocketOptions ConnectionOptions { get; set; } = SecureSocketOptions.Auto;
}