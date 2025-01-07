import { Component } from '@angular/core';

@Component({
    standalone: true,
    selector: 'app-side-menu',
    templateUrl: './side-menu.component.html',
    styleUrls: ['./side-menu.component.scss'],
    imports: []
})
export class SideMenuComponent {
    public sideMenuRolledUp = false;

    public onSideMenuClick(): void {
        this.sideMenuRolledUp = !this.sideMenuRolledUp;
    }
}
