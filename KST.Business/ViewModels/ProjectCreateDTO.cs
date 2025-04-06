namespace KST.Business.ViewModels;

public class ProjectCreateDTO: ProjectModificationDTO
{
    public long[] StudentIds { get; set; }
    public long[] GroupIds { get; set; }
}