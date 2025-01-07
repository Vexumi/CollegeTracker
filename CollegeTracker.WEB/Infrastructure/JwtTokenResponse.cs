using CollegeTracker.Business.ViewModels;

public sealed class JwtTokenResponse
{
    public string? Token  { get; set; }
    public UserViewModel User { get; set; }
}