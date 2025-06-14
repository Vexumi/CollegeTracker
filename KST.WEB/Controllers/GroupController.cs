using KST.Business.Interfaces;
using KST.Business.ViewModels;
using KST.DataAccess.Enums;
using KST.DataAccess.Models;
using KST.WEB.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KST.WEB.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
[Authorize(Roles = "Admin")]
public class GroupController: BaseController<Group>
{
    private readonly IGroupService _groupService;
    
    public GroupController(IGroupService groupService): base(groupService)
    {
        this._groupService = groupService;
    }

    [HttpGet]
    public IEnumerable<Group> GetAll()
    {
        return _groupService.GetAll();
    }

    [HttpPost]
    public async Task<long> Create(GroupModificationDTO userViewModel, CancellationToken cancellationToken)
    {
        return await _groupService.CreateAsync(userViewModel, cancellationToken);
    }
    
    [HttpPost]
    public async Task<Group> Update(GroupModificationDTO userViewModel, CancellationToken cancellationToken)
    {
        return await _groupService.UpdateAsync(userViewModel, cancellationToken);
    }
    
    [HttpDelete]
    public async Task Delete(long id, CancellationToken cancellationToken)
    {
        await _groupService.DeleteAsync(id, cancellationToken);
    }
}