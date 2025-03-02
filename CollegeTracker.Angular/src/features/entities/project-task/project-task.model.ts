import { BaseModel } from "../base.model";
import { StudentModel } from "../student/student.model";
import { ProjectTaskStateEnum } from "./project-task-state.enum";

export interface ProjectTaskModel extends BaseModel {
    title: string,
    description: string | null,
    estimatedHours: number,
    actualHours: number | null,
    inProgressSince: Date,
    assignedTo: StudentModel,
    state: ProjectTaskStateEnum
}