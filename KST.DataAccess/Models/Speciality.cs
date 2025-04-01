namespace KST.DataAccess.Models;

public class Speciality: BaseEntity
{
    public string Title { get; set; } = null!;
    
    public string Description { get; set; } = null!;

    public ICollection<Project> Projects = new List<Project>();
}