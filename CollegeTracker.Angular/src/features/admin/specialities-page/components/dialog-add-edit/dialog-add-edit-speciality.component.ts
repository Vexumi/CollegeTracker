import { Component, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { SubjectModel } from '../../../../entities/subject/subject.model';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { SpecialityModel } from '../../../../entities/speciality/speciality.model';

interface DialogData {
    isEdit: boolean,
    subject: SpecialityModel | null
}

@Component({
    standalone: true,
    selector: 'app-dialog-add-edit-speciality',
    templateUrl: './dialog-add-edit-speciality.component.html',
    styleUrls: ['./dialog-add-edit-speciality.component.scss'],
    imports: [ReactiveFormsModule]
})
export class DialogAddEditSpecialityComponent {
    public readonly isEdit: boolean;

    public readonly form = new FormGroup({
        title: new FormControl("", [Validators.required, Validators.minLength(3)]),
        description: new FormControl("", [Validators.required, Validators.minLength(3)]),
    });

    constructor(
        private readonly dialogRef: MatDialogRef<DialogAddEditSpecialityComponent>,
        @Inject(MAT_DIALOG_DATA) private readonly data: DialogData
    ) {
        this.isEdit = this.data.isEdit;
        if (this.isEdit) {
            this.form.controls["title"].setValue(data.subject!.title);
            this.form.controls["description"].setValue(data.subject!.description);
        }
    }

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
