import { BaseModel } from "../base.model";
import { ProjectTaskModel } from "../project-task/project-task.model";
import { SpecialityModel } from "../speciality/speciality.model";
import { StudentModel } from "../student/student.model";
import { TeacherModel } from "../teacher/teacher.model";
import { ProjectStateEnum } from "./project-state.enum";

export interface ProjectModel extends BaseModel {
    title: string,
    description: string | null,
    state: ProjectStateEnum,
    teacher: TeacherModel,
    speciality: SpecialityModel,
    startDate: Date,
    actualEndDate: Date | null,
    deadline: Date,
    mark: number | null,
    tasks: ProjectTaskModel[],
    students: StudentModel[]
}