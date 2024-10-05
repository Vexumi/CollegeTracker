using CollegeTracker.Business.Interfaces;
using CollegeTracker.Business.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CollegeTracker.WEB.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class UserController: ControllerBase
{
    private readonly IUserService userService;
    
    public UserController(IUserService userService)
    {
        this.userService = userService;
    }

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
    
    // TODO
    [HttpPost]
    public async Task ChangePassword(long id, string newPassword, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
    
    [HttpDelete]
    public async Task Delete(long id, CancellationToken cancellationToken)
    {
        await userService.DeleteAsync(id, cancellationToken);
    }
}