using KST.Business.Infrastructure;
using KST.Business.Interfaces;
using KST.Business.ViewModels;
using KST.DataAccess;
using KST.DataAccess.Enums;
using KST.DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace KST.Business.Services;

public class ReportService(KSTDbContext context, IExcelExportService excelExportService): IReportService
{
    public async Task<Stream> ExportProjects(ProjectSearchParamsDTO exportParams, CancellationToken cancellationToken)
    {
        var projects = await context.Set<Project>()
            .AsNoTracking()
            .ApplySearchFilter(exportParams)
            .Skip(exportParams.Page * exportParams.PageSize).Take(exportParams.PageSize)
            .Include(x => x.Teacher).ThenInclude(y => y.UserInfo)
            .Include(x => x.Students).ThenInclude(y => y.UserInfo)
            .Include(x => x.Speciality)
            .Include(x => x.Tasks)
            .AsSplitQuery()
            .ToListAsync(cancellationToken);

        var data = projects.Select(x => new
        {
            Title = x.Title,
            Description = x.Description,
            State = ProjectExtensions.GetLocalizedProjectState(x.State),
            Speciality = x.Speciality.Title,
            Mark = x.Mark,
            
            StartDate = x.StartDate,
            ProposedEndDate = x.Deadline,
            ActualEndDate = x.ActualEndDate,
            
            TasksTotal = x.Tasks.Count,
            TasksEnded = x.Tasks.Count(x => x.State == TaskState.Closed),
            TasksInProgress = x.Tasks.Count(x => x.State == TaskState.InProgress),
            EstimatedHumanHours = x.Tasks.Sum(x => x.EstimatedHours),
            ActualHumanHours = x.Tasks.Sum(x => x.ActualHours),
            
            Teacher = x.Teacher.UserInfo.Fullname,
            TeachersPhone = x.Teacher.UserInfo.PhoneNumber,
            TeachersEmail = x.Teacher.UserInfo.Email,
            
            StudentNames = string.Join(", ", x.Students.Select(x => x.UserInfo.Fullname))
        });
        return excelExportService.ExportToExcel(data, "Projects");
    }
}