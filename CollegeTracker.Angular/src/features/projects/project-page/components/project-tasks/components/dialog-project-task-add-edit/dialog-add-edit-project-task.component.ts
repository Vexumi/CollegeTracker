import { Component, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { map, Observable } from 'rxjs';
import { AsyncPipe, NgIf } from '@angular/common';
import { ProjectTaskModel } from '../../../../../../entities/project-task/project-task.model';
import { DropdownSelectIdComponent, DropdownSelectIdItem } from '../../../../../../../shared/component/dropdown/dropdown.component';
import { StudentService } from '../../../../../../entities/student/student.service';

interface DialogData {
    isEdit: boolean,
    model: ProjectTaskModel | null
}

@Component({
    standalone: true,
    selector: 'app-dialog-add-edit-project-task',
    templateUrl: './dialog-add-edit-project-task.component.html',
    styleUrls: ['./dialog-add-edit-project-task.component.scss'],
    imports: [ReactiveFormsModule, DropdownSelectIdComponent, AsyncPipe, NgIf],
    providers: []
})
export class DialogAddEditProjectTaskComponent {
    public readonly isEdit: boolean;
    public readonly students$: Observable<DropdownSelectIdItem[]>;

    public readonly form = new FormGroup({
        title: new FormControl<string | null>(null, [Validators.required]),
        description: new FormControl<string | null>(null, [Validators.required]),
        estimatedHours: new FormControl<number | null>(null, [Validators.required]),
        assignedToId: new FormControl<number | null>(null, [Validators.required])
    });

    constructor(
        private readonly studentsService: StudentService,
        private readonly dialogRef: MatDialogRef<DialogAddEditProjectTaskComponent>,
        @Inject(MAT_DIALOG_DATA) private readonly data: DialogData
    ) {
        this.students$ = this.studentsService.getAllActive()
            .pipe(
                map(res => res.map((x) => ({ title: x.userInfo.fullname, id: x.id } as DropdownSelectIdItem)))
            );
        this.isEdit = this.data.isEdit;
        if (this.isEdit) {
            const model = data.model!;
            this.form.controls.title.setValue(model.title);
            this.form.controls.description.setValue(model.description);
            this.form.controls.estimatedHours.setValue(model.estimatedHours);
            this.form.controls.assignedToId.setValue(model.assignedToId);
        }
    }

    public formValid(): boolean {
        return this.form.valid;
    }

    public onStudentSelected(selected: DropdownSelectIdItem) {
        this.form.controls.assignedToId.setValue(selected.id);
    }

    public getCurrentStudent(): DropdownSelectIdItem | null {
        if (this.isEdit) {
            return { title: this.data.model!.assignedTo.userInfo.fullname, id: this.data.model!.assignedTo.id };
        }
        return null;
    }

    public onSaveClicked(): void {
        const result = {
            ...this.data.model,
            title: this.form.controls.title.value,
            description: this.form.controls.description.value,
            assignedToId: this.form.controls.assignedToId.value,
            estimatedHours: this.form.controls.estimatedHours.value
        } as ProjectTaskModel;

        if (this.isEdit) {
            result.id = this.data.model!.id;
        }

        this.dialogRef.close(result);
    }

    public onCancelClicked(): void {
        this.dialogRef.close(null);
    }
}
