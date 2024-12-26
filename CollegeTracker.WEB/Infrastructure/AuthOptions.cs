using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace CollegeTracker.WEB.Infrastructure;

public class AuthOptions
{
    public string ISSUER { get; set; }
    public string AUDIENCE { get; set; }
    public string SECRETKEY { get; set; }
    public int LIFETIME { get; set; }
    
    public static SymmetricSecurityKey GetSymmetricSecurityKey(string key) => 
        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
}