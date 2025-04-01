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
            .Include(x => x.Teacher).ThenInclude(y => y.UserInfo)
            .Include(x => x.Students).ThenInclude(y => y.UserInfo)
            .Include(x => x.Speciality)
            .Include(x => x.Tasks)
            .AsSplitQuery()
            .ToListAsync(cancellationToken);

        var data = projects.Select(GetProjectInfo);
        return excelExportService.ExportToExcel(data, "Информация о проектах");
    }
    
    public async Task<Stream> ExportSpecialitiesWithProjects(CancellationToken cancellationToken)
    {
        var activeProjectStates = new [] { ProjectState.Created, ProjectState.InProgress };
        var reviewProjectStates = new [] { ProjectState.OnReview, ProjectState.Reviewed };

        var specialities = await context.Set<Speciality>()
            .AsNoTracking()
            .Include(x => x.Projects)
            .Select(x => new
            {
                Title = x.Title,
                Description = x.Description,
                TotalProjects = x.Projects.Count,
                ActiveStateProjects = x.Projects.Count(p => activeProjectStates.Contains(p.State)),
                ReviewStateProjects = x.Projects.Count(p => reviewProjectStates.Contains(p.State)),
                RejectedStateProjects = x.Projects.Count(p => p.State == ProjectState.Rejected),
                CompletedStateProjects = x.Projects.Count(p => p.State == ProjectState.Completed)
            })
            .ToListAsync(cancellationToken);
        return excelExportService.ExportToExcel(specialities, "Информация о специальностях и проектах");
    }
    
    public async Task<Stream> ExportProjectTasks(long projectId, CancellationToken cancellationToken)
    {
        var project = await context.Set<Project>()
            .AsNoTracking()
            .Include(x => x.Teacher).ThenInclude(y => y.UserInfo)
            .Include(x => x.Students).ThenInclude(y => y.UserInfo)
            .Include(x => x.Speciality)
            .Include(x => x.Tasks).ThenInclude(y => y.AssignedTo).ThenInclude(z => z.UserInfo)
            .AsSplitQuery()
            .FirstAsync(x => x.Id == projectId, cancellationToken);

        var projectInfo = GetProjectInfo(project);
        var projectTasksInfo = project.Tasks.Select(x => new
        {
            Title = x.Title,
            Description = x.Description,
            EstimatedHours = x.EstimatedHours,
            ActualHours = x.ActualHours,
            Assign = x.AssignedTo.UserInfo.Fullname,
            State = ProjectExtensions.GetLocalizedProjectTaskState(x.State),
            InProgressSince = x.State == TaskState.Opened ? x?.InProgressSince: null
        });

        var data = new Dictionary<string, IEnumerable<object>>()
        {
            { "Информация о проекте", [projectInfo] },
            { "Информация о задачах", projectTasksInfo },
        };
        
        return excelExportService.ExportToExcel(data);
    }
    
    public async Task<Stream> ExportLateProjects(CancellationToken cancellationToken)
    {
        var projects = await context.Set<Project>()
            .AsNoTracking()
            .Where(x => 
                x.Tasks.Sum(t => t.ActualHours ?? t.EstimatedHours) > x.Tasks.Sum(t => t.EstimatedHours) || 
                x.Deadline >= DateOnly.FromDateTime(DateTime.UtcNow))
            .Include(x => x.Teacher).ThenInclude(y => y.UserInfo)
            .Include(x => x.Students).ThenInclude(y => y.UserInfo)
            .Include(x => x.Speciality)
            .Include(x => x.Tasks)
            .AsSplitQuery()
            .ToListAsync(cancellationToken);

        var data = projects.Select(GetProjectLateInfo);
        return excelExportService.ExportToExcel(data, "Информация о проектах отстающих от срока выполнения");
    }

    private object GetProjectInfo(Project x)
    {
        return new
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
        };
    }
    private object GetProjectLateInfo(Project x)
    {
        return new
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
            LateFor = x.Tasks.Sum(t => t.ActualHours ?? t.EstimatedHours) - x.Tasks.Sum(t => t.EstimatedHours),

            Teacher = x.Teacher.UserInfo.Fullname,
            TeachersPhone = x.Teacher.UserInfo.PhoneNumber,
            TeachersEmail = x.Teacher.UserInfo.Email,

            StudentNames = string.Join(", ", x.Students.Select(x => x.UserInfo.Fullname))
        };
    }
}