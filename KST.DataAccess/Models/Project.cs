using KST.DataAccess.Enums;

namespace KST.DataAccess.Models;

public class Project : BaseEntity
{
    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public ProjectTypes Type { get; set; }

    public ProjectState State { get; set; }

    public long StudentId { get; set; }

    public Student Student { get; set; } = null!;

    public long TeacherId { get; set; }

    public Teacher Teacher { get; set; } = null!;

    public long SubjectId { get; set; }

    public Subject Subject { get; set; } = null!;

    public DateTime? Deadline { get; set; }
}