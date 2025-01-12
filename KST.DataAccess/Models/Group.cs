using System.Text.Json.Serialization;

namespace KST.DataAccess.Models;

public class Group: BaseEntity
{
    public string Number { get; set; }
    
    public DateTime LaunchDate { get; set; }
    
    public DateTime StopDate { get; set; }
    
    public long SpecialityId { get; set; }
    public Speciality Speciality { get; set; } = null!;
    
    [JsonIgnore]
    // предметы которые проходят у группы
    public virtual ICollection<Subject> Subjects { get; set; } = new List<Subject>();

    [JsonIgnore]
    // учителя у которых группа в избранном
    public virtual ICollection<Teacher> Teachers { get; set; } = new List<Teacher>();
}