using System.Text.Json.Serialization;

namespace CollegeTracker.DataAccess.Models;

public class Subject: BaseEntity
{
    public string Title { get; set; } = null!;

    public string Description { get; set; } = null!;

    [JsonIgnore]
    // учителя которые ведут предмет
    public virtual ICollection<Teacher> Teachers { get; set; } = new List<Teacher>();
}