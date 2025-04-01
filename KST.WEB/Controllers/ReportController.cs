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
}