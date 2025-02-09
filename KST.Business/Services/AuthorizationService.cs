using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using AutoMapper;
using KST.Business.Infrastructure;
using KST.Business.Interfaces;
using KST.Business.ViewModels;
using KST.DataAccess;
using KST.DataAccess.Enums;
using KST.WEB.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SecurityDriven.Inferno;

namespace KST.Business.Services;

public class AuthorizationService(
    IHttpContextAccessor httpContextAccessor, 
    IOptions<AuthOptions> authOptions,
    KSTDbContext context,
    IMapper mapper): IAuthorizationService
{
    private readonly byte[] SecretKeyBytes = Encoding.UTF8.GetBytes(authOptions.Value.SECRETKEY);

    public async Task<JwtTokenResponse?> AuthorizeByLoginAndPassword(string login, string password,
        CancellationToken cancellationToken)
    {
        var user = await context.Users.FirstOrDefaultAsync(x => x.Email == login, cancellationToken);
        if (user is null || !ValidateHashedPassword(user.PasswordHash, password))
        {
            return null;
        }

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(JwtRegisteredClaimNames.GivenName, user.Fullname),
            new(JwtRegisteredClaimNames.Name, user.Username),
            new(JwtRegisteredClaimNames.Email, user.Email),
            new(ClaimTypes.Role, user.Role.ToString())
        };

        var tokenResponse = TokenForAuth(claims);
        var userViewModel = mapper.Map<UserViewModel>(user);
        tokenResponse.User = userViewModel;
        return tokenResponse;
    }
    
    public string HashPassword(string password)
    {
        var passwordBytes = Encoding.UTF8.GetBytes(password);
        var hash = SuiteB.Encrypt(SecretKeyBytes, passwordBytes.AsArraySegment());
        return Convert.ToBase64String(hash);
    }

    public bool ValidateHashedPassword(string hash, string password)
    {
        byte[] hashBytes = Convert.FromBase64String(hash);
        var passwordBytes = SuiteB.Decrypt(SecretKeyBytes, hashBytes.AsArraySegment());
        var translatedPassword = Encoding.UTF8.GetString(passwordBytes);
        return password.Equals(translatedPassword);
    }

    public UserViewModel? GetCurrentUser()
    {
        var claims = httpContextAccessor.HttpContext.User.Claims;
        if (!claims.Any()) return null;
        
        UserRoles.TryParse(claims.First(x => x.Type == ClaimTypes.Role).Value, out UserRoles role); 
        var user = new UserViewModel()
        {
            Id = long.Parse(claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value as string),
            Username = claims.First(x => x.Type == "name").Value,
            Fullname = claims.First(x => x.Type == ClaimTypes.GivenName).Value,
            Email = claims.First(x => x.Type == ClaimTypes.Email).Value,
            Role = role
        };

        return user;
    }
    
    private JwtTokenResponse TokenForAuth(List<Claim> claims)
    {
        var key = new SymmetricSecurityKey(Convert.FromBase64String(authOptions.Value.SECRETKEY));
        var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);
        
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(authOptions.Value.LIFETIME),
            SigningCredentials = cred,
            Issuer = authOptions.Value.ISSUER,
            Audience = authOptions.Value.AUDIENCE 
        };
        
        var tokenHandler = new JwtSecurityTokenHandler();
        var jwtToken = tokenHandler.CreateToken(tokenDescriptor);

        var response = new JwtTokenResponse
        {
            Token = tokenHandler.WriteToken(jwtToken)
        };
        return response;
    }
}