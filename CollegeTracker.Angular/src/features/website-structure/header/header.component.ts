import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AppRoutes } from '../../../constants/app-routes';
@Component({
    standalone: true,
    selector: 'app-header',
    templateUrl: './header.component.html',
    styleUrls: ['./header.component.scss'],
    imports: []
})
export class HeaderComponent {
    public readonly currentPageName: string;

    constructor(private readonly router: Router) {
        const currentUrl = this.router.url.replace("/", "");
        this.currentPageName = this.getPageName(currentUrl);
    }

    private getPageName(url: string): string {
        switch (url) {
            case AppRoutes.Profile: return "Профиль пользователя"; 
            case AppRoutes.AdminRoutes.Subjects: return "Предметы"; 
            case AppRoutes.AdminRoutes.Specialities: return "Направления"; 
            case AppRoutes.AdminRoutes.Groups: return "Студенческие группы";
            case AppRoutes.AdminRoutes.Students: return "Студенты"; 
            case AppRoutes.AdminRoutes.Teachers: return "Наставники"; 
            default: return "";
        }
    }
}
