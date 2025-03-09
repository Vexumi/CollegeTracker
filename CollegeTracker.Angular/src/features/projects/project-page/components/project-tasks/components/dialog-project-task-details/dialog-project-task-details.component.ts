import { Component, DestroyRef, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA, MatDialog } from '@angular/material/dialog';
import { ReactiveFormsModule } from '@angular/forms';
import { DatePipe } from '@angular/common';
import { ProjectTaskModel } from '../../../../../../entities/project-task/project-task.model';
import { ProjectTaskService } from '../../../../../../entities/project-task/project-task.service';
import { DialogAddEditProjectTaskComponent } from '../dialog-project-task-add-edit/dialog-add-edit-project-task.component';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { switchMap, EMPTY, tap } from 'rxjs';

interface DialogData {
    model: ProjectTaskModel
}

@Component({
    standalone: true,
    selector: 'app-dialog-project-task-details',
    templateUrl: './dialog-project-task-details.component.html',
    styleUrls: ['./dialog-project-task-details.component.scss'],
    imports: [ReactiveFormsModule, DatePipe],
    providers: []
})
export class DialogProjectTaskDetailsComponent {
    public model: ProjectTaskModel;

    constructor(
        private readonly dialogRef: MatDialogRef<DialogProjectTaskDetailsComponent>,
        @Inject(MAT_DIALOG_DATA) private readonly data: DialogData,
        private readonly projectTaskService: ProjectTaskService,
        private readonly dialogService: MatDialog,
        private readonly destroyRef: DestroyRef
    ) {
        this.model = this.data.model;
    }

    public onEditClicked(): void {
        let editted = false;
        const dialogRef = this.dialogService.open(DialogAddEditProjectTaskComponent, {
            data: 
            {
                isEdit: true,
                model: this.model
            }
        });
        
        dialogRef.afterClosed()
            .pipe(
                takeUntilDestroyed(this.destroyRef),
                tap((result) => editted = !!result),
                switchMap((result) => {
                    if (!result) return EMPTY;
                    return this.projectTaskService.edit(result)
                })
            )
            .subscribe(() => {
                if(editted) {
                    this.dialogRef.close(true);
                }
            });
    }

    public onCancelClicked(): void {
        this.dialogRef.close(null);
    }
}
