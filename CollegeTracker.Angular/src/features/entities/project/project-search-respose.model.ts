import { ProjectModel } from "./project.model";

export interface ProjectSearchResponseModel {
    projects: ProjectModel[],
    totalPages: number
}