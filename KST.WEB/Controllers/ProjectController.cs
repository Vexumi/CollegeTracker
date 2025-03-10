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
        _service = service;
    }

    [HttpGet]
    public IEnumerable<Project> GetAll()
    {
        return _service.GetAll();
    }
    
    [HttpGet("{id}")]
    public async Task<Project> GetById([FromRoute] long id, CancellationToken cancellationToken)
    {
        return await _service.GetByIdAsync(id, cancellationToken);
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
    
    [HttpGet("{projectId}")]
    public IQueryable<ProjectAttachment> GetAttachments(long projectId, CancellationToken cancellationToken)
    {
        return _service.GetAttachments(projectId);
    }
    
    [HttpPost]
    public async Task<long> AddLink(ProjectAttachment attachment, CancellationToken cancellationToken)
    {
        return await _service.AddLink(attachment, cancellationToken);
    }

    [HttpPost("{projectId}")]
    public async Task<bool> UploadFile(long projectId, [FromForm] IFormFile file, CancellationToken cancellationToken)
    {
        await _service.UploadFile(projectId, file, cancellationToken);
        return true;
    }
    
    [HttpDelete("{attachmentId}")]
    public async Task<bool> DeleteFile(long attachmentId, CancellationToken cancellationToken)
    {
        await _service.DeleteFile(attachmentId, cancellationToken);
        return true;
    }

    [HttpGet("{attachmentId}")]
    public async Task<FileStreamResult> DownloadFile(long attachmentId, CancellationToken cancellationToken)
    {
        return await _service.DownloadFile(attachmentId, cancellationToken);
    }
}