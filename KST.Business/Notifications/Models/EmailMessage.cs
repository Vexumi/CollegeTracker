using MimeKit;
using MimeKit.Text;

namespace KST.Business.Notifications.Models;

public class EmailMessage
{
    public string Subject { get; set; } = null!;

    public string Message { get; set; } = null!;

    public TextPart Body => new(TextFormat.Html) { Text = Message };

    public IList<string> Recipients { get; set; } = new List<string>(0);
}
