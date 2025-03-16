using KST.Business.Interfaces;
using KST.Business.ViewModels;
using KST.DataAccess.Models;
using KST.WEB.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace KST.WEB.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class MessageController: BaseController<Message>
{
    private readonly IMessageService _service;
    
    public MessageController(IMessageService service): base(service)
    {
        this._service = service;
    }

    [HttpGet("{projectId}")]
    public IEnumerable<Message> GetAll(long projectId)
    {
        return _service.GetAll(projectId);
    }

    [HttpPost]
    public async Task<long> Create(MessageDTO dto, CancellationToken cancellationToken)
    {
        return await _service.CreateAsync(dto, cancellationToken);
    }
    
    [HttpDelete]
    public async Task Delete(long id, CancellationToken cancellationToken)
    {
        await _service.DeleteAsync(id, cancellationToken);
    }
}