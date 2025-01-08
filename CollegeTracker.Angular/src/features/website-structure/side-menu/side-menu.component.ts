import { Component } from '@angular/core';
import { AppRoutes } from '../../../constants/app-routes';

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

    public onSideMenuClick(): void {
        this.sideMenuRolledUp = !this.sideMenuRolledUp;
    }
}
