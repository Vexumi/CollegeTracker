import { NgClass } from '@angular/common';
import { Component } from '@angular/core';

@Component({
    standalone: true,
    selector: 'app-user-page',
    templateUrl: './user-page.component.html',
    styleUrls: ['./user-page.component.scss'],
    imports: [NgClass]
})
export class UserPageComponent {
    protected isSidebarHidden = false;

    toggleSidebar(): void {
        this.isSidebarHidden = !this.isSidebarHidden;
    }
}
