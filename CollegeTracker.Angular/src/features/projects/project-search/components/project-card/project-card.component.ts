import { CommonModule } from '@angular/common';
import { Component, Input } from '@angular/core';
import { ProjectModel } from '../../../../entities/project/project.model';
import { getProjectStateString } from '../../../../../shared/utils/project-state.utils';
import { AppRoutes } from '../../../../../constants/app-routes';
import { ProjectStatusIndicatorComponent } from '../../../../../shared/component/project-status-indicator/project-status-indicator.component';

@Component({
    standalone: true,
    selector: 'app-project-card',
    templateUrl: './project-card.component.html',
    styleUrls: ['./project-card.component.scss'],
    imports: [CommonModule, ProjectStatusIndicatorComponent]
})
export class ProjectCardComponent {
    @Input({ required: true })
    public project!: ProjectModel;

    public getProjectStateString = getProjectStateString;
    public projectRoute = AppRoutes.Projects;
}
