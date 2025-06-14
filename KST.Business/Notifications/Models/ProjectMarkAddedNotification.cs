namespace KST.Business.Notifications.Models;

public class ProjectMarkAddedNotification: BaseNotification
{
    public string Mark { get; set; }
    public long ProjectId { get; set; }
    public string ProjectName { get; set; }
    public override string Subject => "Добавлена оценка для проекта в системе Student Tracker!";
    public override string TemplatePath =>  "/Emails/Templates/ProjectMarkAddedNotification.cshtml";
}
