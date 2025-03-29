import { CommonModule } from '@angular/common';
import { Component, Input } from '@angular/core';
import { ProjectStateEnum } from '../../../features/entities/project/project-state.enum';

@Component({
    standalone: true,
    selector: 'app-project-status-indicator',
    templateUrl: './project-status-indicator.component.html',
    styleUrls: ['./project-status-indicator.component.scss'],
    imports: [CommonModule]
})
export class ProjectStatusIndicatorComponent {
    @Input({ required: true })
    public status!: ProjectStateEnum;

    public getCircleColor() {
        switch(this.status) {
            case ProjectStateEnum.Created:
            case ProjectStateEnum.InProgress:
                return 'green';
            case ProjectStateEnum.OnReview:
            case ProjectStateEnum.Reviewed:
                return 'yellow';
            case ProjectStateEnum.Rejected:
                return 'red';
            case ProjectStateEnum.Completed:
                return 'gray';
        }
    }
}
