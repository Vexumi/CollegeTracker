using KST.Business.ViewModels;
using KST.DataAccess.Enums;
using KST.DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace KST.Business.Infrastructure;

public static class ProjectExtensions
{
    public static string GetLocalizedProjectState(ProjectState state)
    {
        switch (state) {
            case ProjectState.Created: return "Создан";
            case ProjectState.InProgress: return "В процессе";
            case ProjectState.OnReview: return "На проверке";
            case ProjectState.Rejected: return "Отправлен на доработку";
            case ProjectState.Reviewed: return "Проверен";
            case ProjectState.Completed: return "Закончен";
        }
        return "Неизвестно";
    }
    
    public static string GetLocalizedProjectTaskState(TaskState state)
    {
        switch (state) {
            case TaskState.Opened: return "Открыт";
            case TaskState.Blocked: return "Отложен";
            case TaskState.InProgress: return "В процессе";
            case TaskState.Closed: return "Закрыт";
        }
        return "Неизвестно";
    }

    public static IQueryable<Project> ApplySearchFilter(this IQueryable<Project> request,
        ProjectSearchParamsDTO searchParams)
    {
        request = request.WhereIf(!string.IsNullOrEmpty(searchParams.Title),
            project => EF.Functions.ILike(project.Title, searchParams.Title));
        request = request.WhereIf(!string.IsNullOrEmpty(searchParams.Description),
            project => EF.Functions.ILike(project.Description, searchParams.Description));
        
        request = request.WhereIf(searchParams.State != null,
            project => project.State == searchParams.State);
        request = request.WhereIf(searchParams.SpecialityId != null,
            project => project.SpecialityId == searchParams.SpecialityId);
        
        request = request.WhereIf(searchParams.StartDateFrom != null,
            project => project.StartDate >= searchParams.StartDateFrom);
        request = request.WhereIf(searchParams.StartDateTo != null,
            project => project.StartDate <= searchParams.StartDateTo);
        
        request = request.WhereIf(searchParams.ActualEndDateFrom != null,
            project => project.ActualEndDate >= searchParams.ActualEndDateFrom);
        request = request.WhereIf(searchParams.ActualEndDateTo != null,
            project => project.ActualEndDate <= searchParams.ActualEndDateTo);
        
        request = request.WhereIf(searchParams.DeadlineFrom != null,
            project => project.Deadline >= searchParams.DeadlineFrom);
        request = request.WhereIf(searchParams.DeadlineTo != null,
            project => project.Deadline <= searchParams.DeadlineTo);

        request = request.WhereIf(searchParams.CurrentUserId != null,
            project => project.Teacher.UserInfoId == searchParams.CurrentUserId ||
                       project.Students.Select(x => x.UserInfoId).Contains(searchParams.CurrentUserId.Value));
        return request;
    }
}