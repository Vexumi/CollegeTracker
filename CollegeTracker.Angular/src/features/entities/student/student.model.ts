import { BaseModel } from "../base.model";
import { GroupModel } from "../group/group.model";
import { UserModel } from "../user/user.model";

export interface StudentModel extends BaseModel {
    userInfoId: number,
    userInfo: UserModel,
    groupId: number,
    group: GroupModel
}