import { BaseModel } from "../base.model";
import { UserModel } from "../user/user.model";

export interface MessageModel extends BaseModel {
    content: string;
    sender: UserModel;
    sendDate: Date;
    systemEvent: boolean;
    senderId: number;
    projectId: number;
}