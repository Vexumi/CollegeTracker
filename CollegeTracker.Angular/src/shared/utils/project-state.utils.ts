import { ProjectStateEnum } from "../../features/entities/project/project-state.enum"

export const getProjectStateString = (state: ProjectStateEnum): string => {
    switch (state) {
        case ProjectStateEnum.Created: return 'Создан';
        case ProjectStateEnum.InProgress: return 'В процессе';
        case ProjectStateEnum.OnReview: return 'На проверке';
        case ProjectStateEnum.Rejected: return 'Отправлен на доработку';
        case ProjectStateEnum.Reviewed: return 'Проверен';
        case ProjectStateEnum.Completed: return 'Закончен';
    }
    return 'Неизвестно';
}