export interface ProjectCreateDTOModel{
    title: string,
    description: string | null,
    teacherId: number,
    specialityId: number,
    startDate: Date,
    deadline: Date,
    studentIds: number[],
    groupIds: number[]
}