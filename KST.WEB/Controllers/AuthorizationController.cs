using KST.Business.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace KST.WEB.Controllers;

public class AuthorizationController(IAuthorizationService authorizationService): Controller
{
    [HttpGet("~/api/authorize")]
    public async Task<IActionResult> AuthorizeUser(string login, string password, CancellationToken cancellationToken)
    {
        var response = await authorizationService.AuthorizeByLoginAndPassword(login, password, cancellationToken);
        return Ok(response);
    }
}