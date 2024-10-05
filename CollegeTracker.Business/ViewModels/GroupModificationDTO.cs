using CollegeTracker.DataAccess.Models;

namespace CollegeTracker.Business.ViewModels;

public class GroupModificationDTO: BaseEntity
{
    public string Number { get; set; }
    
    public DateTime LaunchDate { get; set; }
    
    public DateTime StopDate { get; set; }
    
    public long SpecialityId { get; set; }
}