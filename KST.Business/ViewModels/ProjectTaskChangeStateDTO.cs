using KST.DataAccess.Enums;

namespace KST.Business.ViewModels;

public class ProjectTaskChangeStateDTO
{
    public long Id { get; set; }
    public TaskState State { get; set; }
}