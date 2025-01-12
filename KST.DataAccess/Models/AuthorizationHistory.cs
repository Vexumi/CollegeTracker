using KST.DataAccess.Enums;

namespace KST.DataAccess.Models;

public class AuthorizationHistory: BaseEntity
{
    public long ChangedById { get; set; }

    public User ChangedBy { get; set; } = null!;
    
    public AuthorizationHistoryType Type { get; set; }
    
    public string? OldValue { get; set; }
    
    public string? NewValue { get; set; }
    
    public long ProjectId { get; set; }

    public Project Project { get; set; } = null!;
}