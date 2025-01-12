using KST.Business.ViewModels;

namespace KST.WEB.Infrastructure;

public sealed class JwtTokenResponse
{
    public string? Token  { get; set; }
    public UserViewModel User { get; set; }
}