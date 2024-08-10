using CollegeTracker.Business.Interfaces;
using CollegeTracker.Business.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CollegeTracker.WEB.Controllers;

[ApiController]
[Route("[controller]/[action]")]
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
    
    [HttpDelete]
    public async Task Delete(long id, CancellationToken cancellationToken)
    {
        await userService.DeleteAsync(id, cancellationToken);
    }
}