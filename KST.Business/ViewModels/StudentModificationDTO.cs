namespace KST.Business.ViewModels;

public class StudentModificationDTO: BaseViewModel
{
    public UserViewModel UserInfo { get; set; }
    
    public long GroupId { get; set; }
}