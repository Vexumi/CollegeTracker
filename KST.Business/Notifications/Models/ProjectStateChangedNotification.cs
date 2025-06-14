namespace KST.Business.Notifications.Models;

public class ProjectStateChangedNotification: BaseNotification
{
    public string NewStatus { get; set; }
    public long ProjectId { get; set; }
    public string ProjectName { get; set; }
    public override string Subject => "Статус проекта в системе Student Tracker изменен!";
    public override string TemplatePath =>  "/Emails/Templates/ProjectStateChangedNotification.cshtml";
}
