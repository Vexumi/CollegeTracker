import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { AppRoutes } from '../../../constants/app-routes';
import { RouterModule } from '@angular/router';

@Component({
    standalone: true,
    selector: 'app-reports',
    templateUrl: './reports.component.html',
    styleUrls: ['./reports.component.scss'],
    imports: [CommonModule, RouterModule]
})
export class ReportsComponent {
    public readonly appRoutes = AppRoutes;
}
