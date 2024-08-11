using CollegeTracker.DataAccess.Models;

namespace CollegeTracker.Business.ViewModels;

public class StudentViewModel: BaseViewModel
{
    public long? UserInfoId { get; set; }
    
    public UserViewModel? UserInfo { get; set; }
    
    public long? GroupId { get; set; }

    public Group? Group { get; set; }
}