using CollegeTracker.Business.Interfaces;
using CollegeTracker.DataAccess.Models;
using Microsoft.AspNetCore.Mvc;

namespace CollegeTracker.WEB.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class SubjectController: ControllerBase
{
    private readonly ISubjectService subjectService;
    
    public SubjectController(ISubjectService subjectService)
    {
        this.subjectService = subjectService;
    }

    [HttpGet]
    public IEnumerable<Subject> GetAll()
    {
        return subjectService.GetAll();
    }

    [HttpPost]
    public async Task<long> Create(Subject model, CancellationToken cancellationToken)
    {
        return await subjectService.CreateAsync(model, cancellationToken);
    }
    
    [HttpPost]
    public async Task<Subject> Update(Subject model, CancellationToken cancellationToken)
    {
        return await subjectService.UpdateAsync(model, cancellationToken);
    }
    
    [HttpDelete]
    public async Task Delete(long id, CancellationToken cancellationToken)
    {
        await subjectService.DeleteAsync(id, cancellationToken);
    }
}