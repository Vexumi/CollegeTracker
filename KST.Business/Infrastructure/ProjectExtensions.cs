using KST.DataAccess.Enums;

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
}