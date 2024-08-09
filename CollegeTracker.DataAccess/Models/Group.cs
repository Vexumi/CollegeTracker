namespace CollegeTracker.DataAccess.Models;

public class Group: BaseEntity
{
    public string Number { get; set; }
    
    public DateTime LaunchDate { get; set; }
    
    public DateTime StopDate { get; set; }
    
    public long SpecialityId { get; set; }

    public Speciality Speciality { get; set; } = null!;
}