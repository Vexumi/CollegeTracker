import { AsyncPipe, CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { Observable } from 'rxjs';
import { ProjectService } from '../../entities/project/project.service';
import { ProjectModel } from '../../entities/project/project.model';
import { Router } from '@angular/router';
import { ProjectMainInfoBlockComponent } from './components/main-info/main-info.component';

@Component({
    standalone: true,
    selector: 'app-project-page',
    templateUrl: './project-page.component.html',
    styleUrls: ['./project-page.component.scss'],
    imports: [AsyncPipe, ProjectMainInfoBlockComponent, CommonModule]
})
export class ProjectPageComponent {
    public project$: Observable<ProjectModel>;

    constructor(
        private readonly projectService: ProjectService,
        private readonly router: Router
    ) {
        const lastChar = this.router.url.split('/').reverse()[0];
        this.project$ = this.projectService.getById(Number(lastChar));
    }
}
