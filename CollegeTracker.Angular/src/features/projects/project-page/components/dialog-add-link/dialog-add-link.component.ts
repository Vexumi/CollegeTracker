import { Component } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';

@Component({
    standalone: true,
    selector: 'app-dialog-add-link',
    templateUrl: './dialog-add-link.component.html',
    styleUrls: ['./dialog-add-link.component.scss'],
    imports: [ReactiveFormsModule]
})
export class DialogAddLinkComponent {
    public readonly form = new FormGroup({
        name: new FormControl("", [Validators.required, Validators.minLength(3)]),
        url: new FormControl("", [Validators.required, Validators.minLength(3)]),
    });

    constructor(
        private readonly dialogRef: MatDialogRef<DialogAddLinkComponent>,
    ) {}

    public onSaveClicked(): void {
        const result = this.form.value;
        this.dialogRef.close(result);
    }

    public onCancelClicked(): void {
        this.dialogRef.close(null);
    }
}
