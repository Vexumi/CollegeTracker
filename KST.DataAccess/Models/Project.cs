using System.Collections;
using KST.DataAccess.Enums;

namespace KST.DataAccess.Models;

public class Project : BaseEntity
{
    public string Title { get; set; } = null!;

    public string? Description { get; set; }
    
    public ProjectState State { get; set; } = ProjectState.Created;

    public long TeacherId { get; set; }

    public Teacher Teacher { get; set; } = null!;

    public long SpecialityId { get; set; }
    
    public Speciality Speciality { get; set; }
    
    public DateTime StartDate { get; set; } = DateTime.Now;

    public DateTime? Deadline { get; set; }
    
    public virtual ICollection<ProjectTask> Tasks { get; set; } = new List<ProjectTask>();

    public virtual ICollection<Student> Students { get; set; } = new List<Student>();
}