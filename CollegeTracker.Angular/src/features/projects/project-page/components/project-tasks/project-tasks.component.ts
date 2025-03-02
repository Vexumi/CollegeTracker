import { AsyncPipe, DatePipe } from '@angular/common';
import { Component, DestroyRef, Input, OnChanges, SimpleChanges } from '@angular/core';
import { ProjectModel } from '../../../../entities/project/project.model';
import { CdkDropListGroup, CdkDropList, CdkDrag, CdkDragDrop, transferArrayItem } from '@angular/cdk/drag-drop';
import { FormControl, FormGroup } from '@angular/forms';
import { ProjectTaskStateEnum } from '../../../../entities/project-task/project-task-state.enum';
import { ProjectTaskModel } from '../../../../entities/project-task/project-task.model';
import { ProjectTaskService } from '../../../../entities/project-task/project-task.service';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';

@Component({
    standalone: true,
    selector: 'app-project-tasks-block',
    templateUrl: './project-tasks.component.html',
    styleUrls: ['./project-tasks.component.scss'],
    imports: [AsyncPipe, DatePipe, CdkDropListGroup, CdkDropList, CdkDrag]
})
export class ProjectTasksBlockComponent implements OnChanges {
    @Input({ required: true })
    public project!: ProjectModel;

    public formGroup: FormGroup = new FormGroup({});
    
    public opened: ProjectTaskModel[] = [];
    public inProgress: ProjectTaskModel[] = [];
    public blocked: ProjectTaskModel[] = [];
    public closed: ProjectTaskModel[] = [];

    constructor(
        private readonly projectTaskService: ProjectTaskService,
        private readonly destroyRef: DestroyRef
    ) {}

    public ngOnChanges(changes: SimpleChanges): void {
        if (changes['project']) {
            this.opened = this.project.tasks.filter(x => x.state == ProjectTaskStateEnum.Opened);
            this.blocked = this.project.tasks.filter(x => x.state == ProjectTaskStateEnum.Blocked);
            this.inProgress = this.project.tasks.filter(x => x.state == ProjectTaskStateEnum.InProgress);
            this.closed = this.project.tasks.filter(x => x.state == ProjectTaskStateEnum.Closed);

            this.formGroup = new FormGroup({
                opened: new FormControl<number[]>(this.opened.map(x => x.id)),
                blocked: new FormControl<number[]>(this.blocked.map(x => x.id)),
                inProgress: new FormControl<number[]>(this.inProgress.map(x => x.id)),
                closed: new FormControl<number[]>(this.closed.map(x => x.id))
            })
        }
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
        this.projectTaskService.changeState(id, newState).pipe(takeUntilDestroyed(this.destroyRef)).subscribe();
    }
}