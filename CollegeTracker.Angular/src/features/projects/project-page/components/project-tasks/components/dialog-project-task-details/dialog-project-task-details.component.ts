import { Component, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { ReactiveFormsModule } from '@angular/forms';
import { DatePipe } from '@angular/common';
import { ProjectTaskModel } from '../../../../../../entities/project-task/project-task.model';

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
        @Inject(MAT_DIALOG_DATA) private readonly data: DialogData
    ) {
        this.model = this.data.model;
    }

    public onCancelClicked(): void {
        this.dialogRef.close(null);
    }
}
