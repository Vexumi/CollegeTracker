using KST.DataAccess.Models;

namespace KST.Business.ViewModels;

public class StudentViewModel: BaseViewModel
{
    public long? UserInfoId { get; set; }
    
    public UserViewModel? UserInfo { get; set; }
    
    public long? GroupId { get; set; }

    public Group? Group { get; set; }
}