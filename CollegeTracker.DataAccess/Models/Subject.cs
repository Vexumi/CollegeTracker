namespace CollegeTracker.DataAccess.Models;

public class Subject: BaseEntity
{
    public string Title { get; set; } = null!;

    public string Description { get; set; } = null!;
}