using KST.Business.Interfaces;
using KST.Business.ViewModels;
using KST.DataAccess.Models;
using KST.WEB.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace KST.WEB.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class ProjectController: BaseController<Project>
{
    private readonly IProjectService _service;
    
    public ProjectController(IProjectService service): base(service)
    {
        this._service = service;
    }

    [HttpGet]
    public IEnumerable<Project> GetAll()
    {
        return _service.GetAll();
    }

    [HttpPost]
    public async Task<long> Create(ProjectModificationDTO viewModel, CancellationToken cancellationToken)
    {
        return await _service.CreateAsync(viewModel, cancellationToken);
    }
    
    [HttpPost]
    public async Task<Project> Update(ProjectModificationDTO viewModel, CancellationToken cancellationToken)
    {
        return await _service.UpdateAsync(viewModel, cancellationToken);
    }
    
    [HttpDelete]
    public async Task Delete(long id, CancellationToken cancellationToken)
    {
        await _service.DeleteAsync(id, cancellationToken);
    }
}