import { CommonModule } from '@angular/common';
import { Component, DestroyRef, EventEmitter, Input, OnChanges, Output, SimpleChanges } from '@angular/core';
import { ProjectModel } from '../../../../entities/project/project.model';
import { CdkDropListGroup, CdkDropList, CdkDrag, CdkDragDrop, transferArrayItem } from '@angular/cdk/drag-drop';
import { ProjectTaskStateEnum } from '../../../../entities/project-task/project-task-state.enum';
import { ProjectTaskModel } from '../../../../entities/project-task/project-task.model';
import { ProjectTaskService } from '../../../../entities/project-task/project-task.service';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { MatDialog } from '@angular/material/dialog';
import { DialogAddEditProjectTaskComponent } from './components/dialog-project-task-add-edit/dialog-add-edit-project-task.component';
import { EMPTY, switchMap, tap } from 'rxjs';
import { DialogProjectTaskDetailsComponent } from './components/dialog-project-task-details/dialog-project-task-details.component';

@Component({
    standalone: true,
    selector: 'app-project-tasks-block',
    templateUrl: './project-tasks.component.html',
    styleUrls: ['./project-tasks.component.scss'],
    imports: [CdkDropListGroup, CdkDropList, CdkDrag, CommonModule]
})
export class ProjectTasksBlockComponent implements OnChanges {
    @Input({ required: true })
    public project!: ProjectModel;

    @Output()
    public tasksChanged = new EventEmitter();
    
    public opened: ProjectTaskModel[] = [];
    public inProgress: ProjectTaskModel[] = [];
    public blocked: ProjectTaskModel[] = [];
    public closed: ProjectTaskModel[] = [];

    constructor(
        private readonly projectTaskService: ProjectTaskService,
        private readonly destroyRef: DestroyRef,
        private readonly dialogSerivce: MatDialog
    ) {}

    public ngOnChanges(changes: SimpleChanges): void {
        if (changes['project']) {
            this.opened = this.project.tasks.filter(x => x.state == ProjectTaskStateEnum.Opened);
            this.blocked = this.project.tasks.filter(x => x.state == ProjectTaskStateEnum.Blocked);
            this.inProgress = this.project.tasks.filter(x => x.state == ProjectTaskStateEnum.InProgress);
            this.closed = this.project.tasks.filter(x => x.state == ProjectTaskStateEnum.Closed);
        }
    }

    public onAddTaskClicked() {
        const dialogRef = this.dialogSerivce.open(DialogAddEditProjectTaskComponent, {
            data: 
            {
                isEdit: false,
                model: { projectId: this.project.id } as ProjectTaskModel
            }
        });
        
        dialogRef.afterClosed()
            .pipe(
                takeUntilDestroyed(this.destroyRef),
                switchMap((result) => {
                    if (!result) return EMPTY;
                    return this.projectTaskService.create(result)
                })
            )
            .subscribe(() => this.tasksChanged.emit());
    }

    public onDetailsClicked(model: ProjectTaskModel) {
        this.dialogSerivce.open(DialogProjectTaskDetailsComponent, {
            data: 
            {
                model
            }
        })
        .afterClosed()
        .pipe(
            takeUntilDestroyed(this.destroyRef),
            tap((result) => {
                if (result) this.tasksChanged.emit();
            })
        )
        .subscribe();
    }

    public onItemDrop(event: CdkDragDrop<ProjectTaskModel[]>) {
        if (event.previousContainer === event.container) {
          return;
        }

        transferArrayItem(
            event.previousContainer.data,
            event.container.data,
            event.previousIndex,
            event.currentIndex,
        );

        const id = event.item.data.id;
        const newState = ProjectTaskStateEnum[event.container.id as keyof typeof ProjectTaskStateEnum]
        this.projectTaskService.changeState(id, newState).pipe(takeUntilDestroyed(this.destroyRef)).subscribe(() => this.tasksChanged.emit());
    }
}