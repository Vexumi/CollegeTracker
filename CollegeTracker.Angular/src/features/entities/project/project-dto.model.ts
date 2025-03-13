import { BaseModel } from "../base.model";

export interface ProjectDTOModel extends BaseModel {
    title: string,
    description: string | null,
    teacherId: number,
    specialityId: number,
    startDate: Date,
    deadline: Date
}