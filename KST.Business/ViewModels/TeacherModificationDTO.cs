namespace KST.Business.ViewModels;

public class TeacherModificationDTO: BaseViewModel
{
    public UserViewModel UserInfo { get; set; }
    
    public List<long> GroupIds { get; set; }
}