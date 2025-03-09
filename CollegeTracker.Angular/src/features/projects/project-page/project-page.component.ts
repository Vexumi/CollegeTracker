import { AsyncPipe, CommonModule } from '@angular/common';
import { Component, DestroyRef } from '@angular/core';
import { BehaviorSubject, map, tap } from 'rxjs';
import { ProjectService } from '../../entities/project/project.service';
import { ProjectModel } from '../../entities/project/project.model';
import { Router } from '@angular/router';
import { ProjectMainInfoBlockComponent } from './components/main-info/main-info.component';
import { ProjectTasksBlockComponent } from "./components/project-tasks/project-tasks.component";
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';

@Component({
    standalone: true,
    selector: 'app-project-page',
    templateUrl: './project-page.component.html',
    styleUrls: ['./project-page.component.scss'],
    imports: [AsyncPipe, ProjectMainInfoBlockComponent, CommonModule, ProjectTasksBlockComponent]
})
export class ProjectPageComponent {
    public project$ = new BehaviorSubject<ProjectModel | null>(null);
    private readonly projectId: number;

    constructor(
        private readonly projectService: ProjectService,
        private readonly router: Router,
        private readonly destroyRef: DestroyRef
    ) {
        const lastChar = this.router.url.split('/').reverse()[0];
        this.projectId = Number(lastChar);
        this.loadProject();
    }

    public onReload() {
        this.loadProject();
    }

    private loadProject() {
        this.projectService.getById(this.projectId)
        .pipe(
            takeUntilDestroyed(this.destroyRef),
            map((project) => (
                {
                    ...project, 
                    tasks: project.tasks.map((task) => ({...task, projectId: project.id}))
                }
            )),
            tap((project) => this.project$.next(project))
        ).subscribe();
    }
}
