import { BaseModel } from "../base.model";
import { UserModel } from "../user/user.model";

export interface TeacherDtoModel extends BaseModel {
    userInfo: UserModel,
    groupIds: number[]
}