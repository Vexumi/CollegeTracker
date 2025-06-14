using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using IAuthorizationService = KST.Business.Interfaces.IAuthorizationService;

namespace KST.WEB.Controllers;

public class AuthorizationController(IAuthorizationService authorizationService): Controller
{
    [HttpGet("~/api/authorize")]
    [AllowAnonymous]
    public async Task<IActionResult> AuthorizeUser(string login, string password, CancellationToken cancellationToken)
    {
        var response = await authorizationService.AuthorizeByLoginAndPassword(login, password, cancellationToken);
        return Ok(response);
    }
}