import { AsyncPipe, DatePipe } from '@angular/common';
import { Component, Input } from '@angular/core';
import { ProjectModel } from '../../../../entities/project/project.model';
import { getProjectStateString } from '../../../../../shared/utils/project-state.utils';
import { ProjectTaskStateEnum } from '../../../../entities/project-task/project-task-state.enum';

@Component({
    standalone: true,
    selector: 'app-project-main-info-block',
    templateUrl: './main-info.component.html',
    styleUrls: ['./main-info.component.scss'],
    imports: [AsyncPipe, DatePipe]
})
export class ProjectMainInfoBlockComponent {
    @Input({ required: true })
    public project!: ProjectModel;

    public getProjectStateString = getProjectStateString;

    public getCompletedTasks() {
        const completedTasks = this.project.tasks.filter((t) => t.state == ProjectTaskStateEnum.Closed).length;
        const totalTasks = this.project.tasks.length;
        return completedTasks * 100 / totalTasks;
    }

    public getEstimatedHumanHours() {
        return this.project.tasks.reduce((acc, val) => acc += val.estimatedHours, 0);
    }

    public getActualHumanHours() {
        return this.project.tasks.reduce((acc, val) => {
            if (val.actualHours != null) acc += val.actualHours;
            else acc += val.estimatedHours;
            return acc;
        }, 0);
    }
}
