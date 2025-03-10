import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { FormControl, ReactiveFormsModule, Validators } from '@angular/forms';
import { ProjectModel } from '../../../../entities/project/project.model';
import { ProjectService } from '../../../../entities/project/project.service';
import { ProjectStateEnum } from '../../../../entities/project/project-state.enum';
import { getProjectStateString } from '../../../../../shared/utils/project-state.utils';

interface DialogData {
    model: ProjectModel
}

@Component({
    standalone: true,
    selector: 'app-dialog-change-status',
    templateUrl: './dialog-change-status.component.html',
    styleUrls: ['./dialog-change-status.component.scss'],
    imports: [ReactiveFormsModule]
})
export class DialogChangeStatusComponent {
    public readonly suitableStates: ProjectStateEnum[];
    public getProjectStateString = getProjectStateString;

    public formControl = new FormControl<ProjectStateEnum | null>(null, [Validators.required]);

    constructor(
        private readonly dialogRef: MatDialogRef<DialogChangeStatusComponent>,
        @Inject(MAT_DIALOG_DATA) private readonly data: DialogData,
        private readonly projectService: ProjectService
    ) {
        this.suitableStates = this.projectService.getSuitableStates(this.data.model);
    }

    public stateSelected(state: ProjectStateEnum) {
        return this.formControl.value == state;
    }

    public onSelectStateClicked(state: ProjectStateEnum) {
        if (this.formControl.value == state) {
            this.formControl.setValue(null);
            return;
        }
        this.formControl.setValue(state);
    }

    public onChangeClicked(): void {
        this.dialogRef.close(this.formControl.value);
    }

    public onCancelClicked(): void {
        this.dialogRef.close(null);
    }
}
