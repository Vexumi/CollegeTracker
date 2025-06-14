using KST.Business.Interfaces;
using KST.Business.ViewModels;
using KST.DataAccess.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KST.WEB.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
[Authorize]
public class SpecialityController: ControllerBase
{
    private readonly ISpecialityService specialityService;
    
    public SpecialityController(ISpecialityService specialityService)
    {
        this.specialityService = specialityService;
    }

    [HttpGet]
    public IEnumerable<Speciality> GetAll()
    {
        return specialityService.GetAll();
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<long> Create(Speciality model, CancellationToken cancellationToken)
    {
        return await specialityService.CreateAsync(model, cancellationToken);
    }
    
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<Speciality> Update(Speciality model, CancellationToken cancellationToken)
    {
        return await specialityService.UpdateAsync(model, cancellationToken);
    }
    
    [HttpDelete]
    [Authorize(Roles = "Admin")]
    public async Task Delete(long id, CancellationToken cancellationToken)
    {
        await specialityService.DeleteAsync(id, cancellationToken);
    }
    
    [HttpPost("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task ChangeActivityState([FromRoute]long id, CancellationToken cancellationToken)
    {
        await specialityService.ChangeActivityState(id, cancellationToken);
    }
}