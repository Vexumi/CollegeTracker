import { AsyncPipe, DatePipe } from '@angular/common';
import { Component, Input } from '@angular/core';
import { ProjectModel } from '../../../../entities/project/project.model';

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
}
