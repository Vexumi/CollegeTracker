import { Component } from '@angular/core';
import { PhonePipe } from '../../shared/pipes/phone-number/phone.pipe';
import { AuthService } from '../../shared/services/auth.service';
import { UserModel } from '../entities/user/user.model';
import { UserRole } from '../entities/user/user-role.model';
import { Router } from '@angular/router';
import { AppRoutes } from '../../constants/app-routes';
import { MatDialog } from '@angular/material/dialog';
import { DialogLogoutConfirmationComponent } from './components/dialog-logout-confirmation/dialog-logout-confirmation.component';
import { tap } from 'rxjs';

@Component({
    standalone: true,
    selector: 'app-profile',
    templateUrl: './profile.component.html',
    styleUrls: ['./profile.component.scss'],
    imports: [PhonePipe]
})
export class ProfilePageComponent {
    public readonly currentUser: UserModel;

    constructor(
        private readonly authService: AuthService,
        private readonly router: Router,
        private readonly dialogSerivce: MatDialog
    ) {
        this.currentUser = this.authService.getCurrentUser();
    }

    public onLogoutClicked() {
        const dialogRef = this.dialogSerivce.open(DialogLogoutConfirmationComponent);
        
        dialogRef.afterClosed()
            .pipe(
                tap((result) => {
                    if (!result) return;
                    this.router.navigate([AppRoutes.Logout]);
                })
            )
            .subscribe();
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
