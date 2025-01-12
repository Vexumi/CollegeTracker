using System.Text.Json.Serialization;
using KST.DataAccess.Models;

namespace KST.Business.ViewModels;

public class TeacherViewModel: BaseViewModel
{
    public long? UserInfoId { get; set; }
    
    public UserViewModel? UserInfo { get; set; }

    // группы избранные учителем
    public virtual ICollection<Group> Groups { get; set; } = new List<Group>();
    
    // предметы которые ведет учитель
    public virtual ICollection<Subject> Subjects { get; set; } = new List<Subject>(); 
}