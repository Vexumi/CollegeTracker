using KST.Business.Interfaces;
using KST.Business.ViewModels;
using KST.DataAccess.Models;
using Microsoft.AspNetCore.Mvc;

namespace KST.WEB.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
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
    public async Task<long> Create(Speciality model, CancellationToken cancellationToken)
    {
        return await specialityService.CreateAsync(model, cancellationToken);
    }
    
    [HttpPost]
    public async Task<Speciality> Update(Speciality model, CancellationToken cancellationToken)
    {
        return await specialityService.UpdateAsync(model, cancellationToken);
    }
    
    [HttpDelete]
    public async Task Delete(long id, CancellationToken cancellationToken)
    {
        await specialityService.DeleteAsync(id, cancellationToken);
    }
}