import { Component } from '@angular/core';
import { PhonePipe } from '../../shared/pipes/phone-number/phone.pipe';
import { AuthService } from '../../shared/services/auth.service';
import { UserModel } from '../entities/user/user.model';
import { UserRole } from '../entities/user/user-role.model';

@Component({
    standalone: true,
    selector: 'app-profile',
    templateUrl: './profile.component.html',
    styleUrls: ['./profile.component.scss'],
    imports: [PhonePipe]
})
export class ProfilePageComponent {
    public readonly currentUser: UserModel;

    constructor(private readonly authService: AuthService) {
        this.currentUser = this.authService.getCurrentUser();
    }

    public getUserType(): string {
        switch(this.currentUser.role) {
            case UserRole.Admin: return 'Администратор';
            case UserRole.Student: return 'Студент';
            case UserRole.Teacher: return 'Преподаватель';
            default: return 'Гость'
        }
    }

    public getImageByRole() {
        switch(this.currentUser.role) {
            case UserRole.Admin: return 'profile-admin.png';
            case UserRole.Student: return 'profile-student.png';
            case UserRole.Teacher: return 'profile-teacher.png';
            default: return 'profile-guest.png'
        } 
    }
}
