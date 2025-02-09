using KST.Business.Interfaces;
using KST.Business.ViewModels;
using KST.WEB.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KST.WEB.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class UserController(IUserService userService): ControllerBase
{
    [HttpGet]
    public IEnumerable<UserViewModel> GetAll()
    {
        return userService.GetAll();
    }

    [HttpPost]
    public async Task<long> Create(UserViewModel userViewModel, CancellationToken cancellationToken)
    {
        return await userService.CreateAsync(userViewModel, cancellationToken);
    }
    
    [HttpPost]
    public async Task<UserViewModel> Update(UserViewModel userViewModel, CancellationToken cancellationToken)
    {
        return await userService.UpdateAsync(userViewModel, cancellationToken);
    }
    
    [HttpPost]
    public async Task<IActionResult> ChangePassword(ChangePasswordRequest request, CancellationToken cancellationToken)
    { 
        var result = await userService.ChangePassword(request.NewPassword, request.OldPassword, cancellationToken);
        if (!result) return Ok(false);
        return Ok(true);
    }
    
    [HttpDelete]
    public async Task Delete(long id, CancellationToken cancellationToken)
    {
        await userService.DeleteAsync(id, cancellationToken);
    }
}