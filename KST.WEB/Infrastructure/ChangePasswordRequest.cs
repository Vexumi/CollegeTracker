namespace KST.WEB.Infrastructure;

public class ChangePasswordRequest
{
    public string NewPassword { get; set; }
    public string OldPassword { get; set; }
}