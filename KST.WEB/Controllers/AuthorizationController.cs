using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using AutoMapper;
using KST.Business.Interfaces;
using KST.Business.ViewModels;
using KST.WEB.Infrastructure.Exceptions;
using KST.WEB.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace KST.WEB.Controllers;

public class AuthorizationController(IUserService userService, IOptions<AuthOptions> authOptions, IMapper mapper): Controller
{
    [HttpGet("~/api/authorize")]
    public async Task<IActionResult> AuthorizeUser(string login, string password, CancellationToken cancellationToken)
    {
        var user = await userService.GetAllUsers().FirstOrDefaultAsync(x => x.Email == login && x.PasswordHash == password, cancellationToken); // TODO Comparison of passwords
        if (user is null)
        {
            return Ok(null);
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
        return Ok(tokenResponse);
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