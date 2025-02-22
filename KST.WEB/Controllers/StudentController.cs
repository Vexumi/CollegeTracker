using KST.Business.Interfaces;
using KST.Business.ViewModels;
using KST.DataAccess.Models;
using KST.WEB.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace KST.WEB.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class StudentController: BaseController<Student>
{
    private readonly IStudentService studentService;
    
    public StudentController(IStudentService studentService): base(studentService)
    {
        this.studentService = studentService;
    }

    [HttpGet]
    public IEnumerable<StudentViewModel> GetAll()
    {
        return studentService.GetAll();
    }

    [HttpPost]
    public async Task<long> Create(StudentModificationDTO userViewModel, CancellationToken cancellationToken)
    {
        return await studentService.CreateAsync(userViewModel, cancellationToken);
    }
    
    [HttpPost]
    public async Task<StudentViewModel> Update(StudentModificationDTO userViewModel, CancellationToken cancellationToken)
    {
        return await studentService.UpdateAsync(userViewModel, cancellationToken);
    }
    
    [HttpDelete]
    public async Task Delete(long id, CancellationToken cancellationToken)
    {
        await studentService.DeleteAsync(id, cancellationToken);
    }
}