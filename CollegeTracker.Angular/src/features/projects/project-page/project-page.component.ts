import { AsyncPipe, CommonModule, NgIf } from '@angular/common';
import { Component, DestroyRef } from '@angular/core';
import { BehaviorSubject, EMPTY, map, switchMap, tap } from 'rxjs';
import { ProjectService } from '../../entities/project/project.service';
import { ProjectModel } from '../../entities/project/project.model';
import { Router } from '@angular/router';
import { ProjectMainInfoBlockComponent } from './components/main-info/main-info.component';
import { ProjectTasksBlockComponent } from "./components/project-tasks/project-tasks.component";
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { ProjectAttachmentsBlockComponent } from './components/attachments/attachments.component';
import { MatDialog } from '@angular/material/dialog';
import { DialogChangeStatusComponent } from './components/dialog-change-status/dialog-change-status.component';
import { DialogEditProjectComponent } from './components/dialog-edit-project/dialog-edit-project.component';
import { MessagesBlockComponent } from './components/messages/messages.component';
import { ProjectStateEnum } from '../../entities/project/project-state.enum';
import { DialogEvaluateProjectComponent } from './components/dialog-evaluate-project/dialog-evaluate-project.component';
import { AuthService } from '../../../shared/services/auth.service';
import { ReportsService } from '../../entities/reports/reports.service';

@Component({
    standalone: true,
    selector: 'app-project-page',
    templateUrl: './project-page.component.html',
    styleUrls: ['./project-page.component.scss'],
    imports: [AsyncPipe, ProjectMainInfoBlockComponent, CommonModule, ProjectTasksBlockComponent, ProjectAttachmentsBlockComponent, MessagesBlockComponent, NgIf]
})
export class ProjectPageComponent {
    public project$ = new BehaviorSubject<ProjectModel | null>(null);
    public readOnlyMode = true;
    private readonly projectId: number;

    constructor(
        private readonly projectService: ProjectService,
        private readonly router: Router,
        private readonly destroyRef: DestroyRef,
        private readonly dialogService: MatDialog,
        private readonly authService: AuthService,
        private readonly reportService: ReportsService
    ) {
        const lastChar = this.router.url.split('/').reverse()[0];
        this.projectId = Number(lastChar);
        this.loadProject();
    }

    public onReload() {
        this.loadProject();
    }

    public onChangeStatusClicked() {
        const dialogRef = this.dialogService.open(DialogChangeStatusComponent, {
            data: {
                model: this.project$.value
            }
        });
        
        dialogRef.afterClosed()
            .pipe(
                takeUntilDestroyed(this.destroyRef),
                switchMap((result) => {
                    if (!result) return EMPTY;
                    return this.projectService.changeState(this.projectId, result);
                })
            )
            .subscribe(() => this.onReload());
    }

    public onEditClicked() {
        const dialogRef = this.dialogService.open(DialogEditProjectComponent, {
            data: {
                model: this.project$.value
            }
        });
        
        dialogRef.afterClosed()
            .pipe(
                takeUntilDestroyed(this.destroyRef),
                switchMap((result) => {
                    if (!result) return EMPTY;
                    return this.projectService.edit(result);
                })
            )
            .subscribe(() => this.onReload());
    }

    public buttonEvaluateProjectVisible(project: ProjectModel) {
        return project.state == ProjectStateEnum.OnReview && this.authService.isTeacher();
    }

    public buttonExportProjectTasksVisible() {
        return this.authService.isAdmin();
    }

    public onEvaluateProjectClicked() {
        const dialogRef = this.dialogService.open(DialogEvaluateProjectComponent);
        
        dialogRef.afterClosed()
            .pipe(
                takeUntilDestroyed(this.destroyRef),
                switchMap((result) => {
                    if (!result) return EMPTY;
                    return this.projectService.evaluateProject(this.projectId, result);
                })
            )
            .subscribe(() => this.onReload());
    }

    public onExportProjectTasksClicked(projectId: number) {
        this.reportService.exportProjectTasks(projectId);
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
            tap((project) => {
                this.project$.next(project);
                this.readOnlyMode = !this.projectService.userIsParticipantOfProject(project);
            })
        ).subscribe();
    }
}
