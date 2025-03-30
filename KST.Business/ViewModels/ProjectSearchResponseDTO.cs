using KST.DataAccess.Models;

namespace KST.Business.ViewModels;

public sealed class ProjectSearchResponseDTO
{
    public IEnumerable<Project> Projects { get; set; }
    public int TotalPages { get; set; }
}