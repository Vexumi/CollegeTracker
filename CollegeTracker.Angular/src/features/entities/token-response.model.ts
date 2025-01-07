import { UserModel } from './user/user.model';

export interface JwtTokenResponse {
    token: string;
    user: UserModel;
}
