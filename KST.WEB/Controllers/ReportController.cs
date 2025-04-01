using KST.Business.Interfaces;
using KST.Business.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace KST.WEB.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class ReportController(IReportService reportService): ControllerBase
{
    [HttpPost]
    public async Task<FileResult> ExportProjects([FromBody]ProjectSearchParamsDTO exportParams, CancellationToken cancellationToken)
    {
        var stream = await reportService.ExportProjects(exportParams, cancellationToken);
        return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"Projects-Export_{DateTime.UtcNow:yyyy-MM-dd}.xlsx");
    }
    
    [HttpGet]
    public async Task<FileResult> ExportSpecialitiesWithProjects(CancellationToken cancellationToken)
    {
        var stream = await reportService.ExportSpecialitiesWithProjects(cancellationToken);
        return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"Specialities-Projects-Export_{DateTime.UtcNow:yyyy-MM-dd}.xlsx");
    }
    
    [HttpGet("{projectId}")]
    public async Task<FileResult> ExportProjectTasks(long projectId, CancellationToken cancellationToken)
    {
        var stream = await reportService.ExportProjectTasks(projectId, cancellationToken);
        return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"Project-Tasks-Export_{DateTime.UtcNow:yyyy-MM-dd}.xlsx");
    }
    
    [HttpGet]
    public async Task<FileResult> ExportLateProjects(CancellationToken cancellationToken)
    {
        var stream = await reportService.ExportLateProjects(cancellationToken);
        return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"Project-Late-Export_{DateTime.UtcNow:yyyy-MM-dd}.xlsx");
    }
}