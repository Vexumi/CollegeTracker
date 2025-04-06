namespace KST.Business.Notifications.Models;

public class UserAddedNotification: BaseNotification
{
    public string Fullname { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string FrontendLink { get; set; }
    public override string Subject => "Вы добавлены в систему StudentTracker";
    public override string TemplatePath => "KST.Business/Notifications/Templates/UserAddedNotification.cshtml";
}