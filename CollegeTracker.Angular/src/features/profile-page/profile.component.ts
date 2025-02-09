import { Component } from '@angular/core';
import { PhonePipe } from '../../shared/pipes/phone-number/phone.pipe';
import { AuthService } from '../../shared/services/auth.service';
import { UserModel } from '../entities/user/user.model';
import { UserRole } from '../entities/user/user-role.model';
import { Router } from '@angular/router';
import { AppRoutes } from '../../constants/app-routes';
import { MatDialog } from '@angular/material/dialog';
import { DialogLogoutConfirmationComponent } from './components/dialog-logout-confirmation/dialog-logout-confirmation.component';
import { filter, map, of, switchMap, tap } from 'rxjs';
import { DialogChangePasswordComponent } from './components/dialog-change-password/dialog-change-password.component';
import { UserService } from '../entities/user/user.service';
import { DialogInformationComponent } from '../../shared/component/dialog-information/dialog-information.component';

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
        private readonly dialogSerivce: MatDialog,
        private readonly userService: UserService
    ) {
        this.currentUser = this.authService.getCurrentUser();
    }

    public onChangePasswordClicked() {
        const dialogRef = this.dialogSerivce.open(DialogChangePasswordComponent);
        
        dialogRef.afterClosed()
            .pipe(
                filter((res) => res != null),
                switchMap((dRes) => this.userService.changePassword(dRes.newPassword, dRes.oldPassword)
                    .pipe(
                        map((resp) => ({response: resp, password: dRes.newPassword}))
                    )
                ),

                switchMap((resp) => {
                    if (!resp.response) {
                        this.openInvalidPasswordDialog();
                        return of();
                    }
                    this.openPasswordSuccessfulyChanged();
                    return this.authService.refreshToken(resp.password);
                })
            )
            .subscribe();
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

    private openPasswordSuccessfulyChanged() {
        const dialogRef = this.dialogSerivce.open(DialogInformationComponent, {
            data: {
                header: "Пароль успешно изменен!",
                body: "Теперь для входа в аккаунт пользуйтесь новым паролем."
            }
        });
        dialogRef.afterClosed().subscribe();
    }

    private openInvalidPasswordDialog() {
        const dialogRef = this.dialogSerivce.open(DialogInformationComponent, {
            data: {
                header: "Неправильный пароль!",
                body: "Введен неправильный пароль от текущего аккаунта, пожалуйста попробуйте еще раз!"
            }
        });
        dialogRef.afterClosed().subscribe();
    }
}
