import { BaseModel } from '../base.model';
import { UserRole as UserRoleModel } from './user-role.model';

export interface UserModel extends BaseModel {
    email: string;
    phoneNumber: string;
    fullname: string;
    username: string;
    role: UserRoleModel;
    password: string | null;
}
