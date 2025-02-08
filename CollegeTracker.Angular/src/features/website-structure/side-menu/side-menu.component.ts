import { Component } from '@angular/core';
import { AppRoutes } from '../../../constants/app-routes';
import { AuthService } from '../../../shared/services/auth.service';

@Component({
    standalone: true,
    selector: 'app-side-menu',
    templateUrl: './side-menu.component.html',
    styleUrls: ['./side-menu.component.scss'],
    imports: []
})
export class SideMenuComponent {
    public sideMenuRolledUp = false;
    public readonly routes = AppRoutes;

    public readonly adminPrefix = AppRoutes.Admin + '/';
    public readonly teacherPrefix = AppRoutes.Teacher + '/';
    public readonly studentPrefix = AppRoutes.Student + '/';

    constructor(public readonly authService: AuthService) {}

    public onSideMenuClick(): void {
        this.sideMenuRolledUp = !this.sideMenuRolledUp;
    }
}
