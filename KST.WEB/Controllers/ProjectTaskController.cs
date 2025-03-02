using KST.Business.Interfaces;
using KST.Business.ViewModels;
using KST.DataAccess.Models;
using KST.WEB.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace KST.WEB.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class ProjectTaskController: BaseController<ProjectTask>
{
    private readonly IProjectTaskService _service;
    
    public ProjectTaskController(IProjectTaskService service): base(service)
    {
        this._service = service;
    }

    [HttpGet]
    public IEnumerable<ProjectTask> GetAll()
    {
        return _service.GetAll();
    }

    [HttpPost]
    public async Task<long> Create(ProjectTaskModificationDTO viewModel, CancellationToken cancellationToken)
    {
        return await _service.CreateAsync(viewModel, cancellationToken);
    }
    
    [HttpPost]
    public async Task<ProjectTask> Update(ProjectTaskModificationDTO viewModel, CancellationToken cancellationToken)
    {
        return await _service.UpdateAsync(viewModel, cancellationToken);
    }
    
    [HttpDelete]
    public async Task Delete(long id, CancellationToken cancellationToken)
    {
        await _service.DeleteAsync(id, cancellationToken);
    }
    
    [HttpPost]
    public async Task<long> ChangeState(ProjectTaskChangeStateDTO viewModel, CancellationToken cancellationToken)
    {
        return await _service.ChangeState(viewModel.Id, viewModel.State, cancellationToken);
    }
}