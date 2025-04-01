using KST.Business.ViewModels;

namespace KST.Business.Interfaces;

public interface IReportService
{
    Task<Stream> ExportProjects(ProjectSearchParamsDTO exportParams, CancellationToken cancellationToken);
    Task<Stream> ExportSpecialitiesWithProjects(CancellationToken cancellationToken);
    Task<Stream> ExportProjectTasks(long projectId, CancellationToken cancellationToken);
    Task<Stream> ExportLateProjects(CancellationToken cancellationToken);
}