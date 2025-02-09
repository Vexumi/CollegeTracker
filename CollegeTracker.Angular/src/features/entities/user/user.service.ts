import { Injectable } from '@angular/core';
import { ApiEndpoints } from '../../../constants/api-routes';
import { BaseService } from '../base.service';
import { UserModel } from './user.model';
import { Observable } from 'rxjs';

@Injectable({
    providedIn: 'root'
})
export class UserService extends BaseService<UserModel> {
    constructor() {
        super(ApiEndpoints.Users);
    }

    public changePassword(newPassword: string, oldPassword: string): Observable<boolean> {
        return this.http.post<boolean>(`${this.baseControllerUrl}/ChangePassword`, {
            newPassword,
            oldPassword
        });
    }
}
