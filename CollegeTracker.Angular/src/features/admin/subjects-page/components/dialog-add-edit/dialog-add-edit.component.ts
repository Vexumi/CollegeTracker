import { Component, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { SubjectModel } from '../../../../entities/subject/subject.model';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';

interface DialogData {
    isEdit: boolean,
    subject: SubjectModel | null
}

@Component({
    standalone: true,
    selector: 'app-dialog-add-edit',
    templateUrl: './dialog-add-edit.component.html',
    styleUrls: ['./dialog-add-edit.component.scss'],
    imports: [ReactiveFormsModule]
})
export class DialogAddEditSubjectComponent {
    public readonly isEdit = this.data.isEdit;

    public readonly form = new FormGroup({
        title: new FormControl("", [Validators.required, Validators.minLength(3)]),
        description: new FormControl("", [Validators.required, Validators.minLength(3)]),
    });

    constructor(
        private readonly dialogRef: MatDialogRef<DialogAddEditSubjectComponent>,
        @Inject(MAT_DIALOG_DATA) private readonly data: DialogData
    ) {}

    public onSaveClicked(): void {
        const result = this.form.value as SubjectModel;

        if (this.isEdit) {
            result.id = this.data.subject!.id;
        }

        this.dialogRef.close(result);
    }

    public onCancelClicked(): void {
        this.dialogRef.close(null);
    }
}
