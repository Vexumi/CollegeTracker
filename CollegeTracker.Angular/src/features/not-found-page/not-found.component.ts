import { Component } from '@angular/core';
import { AppRoutes } from '../../constants/app-routes';

@Component({
    standalone: true,
    selector: 'app-not-found',
    templateUrl: './not-found.component.html',
    styleUrls: ['./not-found.component.scss'],
    imports: []
})
export class NotFoundPageComponent {
    public readonly basePage = AppRoutes.Profile;
}
