using KST.Business.ViewModels;

namespace KST.Business.Interfaces;

public interface IReportService
{
    Task<Stream> ExportProjects(ProjectSearchParamsDTO exportParams, CancellationToken cancellationToken);
}