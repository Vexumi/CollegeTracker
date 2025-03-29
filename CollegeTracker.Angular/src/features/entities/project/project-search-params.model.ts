import { ProjectStateEnum } from "./project-state.enum";

export interface ProjectSearchParamsModel {
    title: string | null,
    description: string | null,
    state: ProjectStateEnum | null,
    specialityId: number | null,
    startDateFrom: Date | null,
    startDateTo: Date | null,
    actualEndDateFrom: Date | null,
    actualEndDateTo: Date | null,
    deadlineFrom: Date | null,
    deadlineTo: Date | null,

    pageSize: number,
    currentPage: number
}