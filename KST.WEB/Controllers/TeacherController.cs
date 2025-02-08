using KST.Business.Interfaces;
using KST.Business.ViewModels;
using KST.DataAccess.Models;
using KST.WEB.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace KST.WEB.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class TeacherController: BaseController<Teacher>
{
    private readonly ITeacherService baseService;
    
    public TeacherController(ITeacherService baseService): base(baseService)
    {
        this.baseService = baseService;
    }

    [HttpGet]
    public IEnumerable<TeacherViewModel> GetAll()
    {
        return baseService.GetAll();
    }

    [HttpPost]
    public async Task<long> Create(TeacherModificationDTO userViewModel, CancellationToken cancellationToken)
    {
        return await baseService.CreateAsync(userViewModel, cancellationToken);
    }
    
    [HttpPost]
    public async Task<TeacherViewModel> Update(TeacherModificationDTO userViewModel, CancellationToken cancellationToken)
    {
        return await baseService.UpdateAsync(userViewModel, cancellationToken);
    }
    
    [HttpDelete]
    public async Task Delete(long id, CancellationToken cancellationToken)
    {
        await baseService.DeleteAsync(id, cancellationToken);
    }
}