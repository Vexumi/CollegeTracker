import { Component } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';
import { FormControl, ReactiveFormsModule, Validators } from '@angular/forms';

@Component({
    standalone: true,
    selector: 'app-dialog-evaluate-project',
    templateUrl: './dialog-evaluate-project.component.html',
    styleUrls: ['./dialog-evaluate-project.component.scss'],
    imports: [ReactiveFormsModule]
})
export class DialogEvaluateProjectComponent {
    public readonly form = new FormControl<number | null>(null, [Validators.required, Validators.min(0), Validators.max(100)]);

    constructor(
        private readonly dialogRef: MatDialogRef<DialogEvaluateProjectComponent>,
    ) {}

    public onSaveClicked(): void {
        this.dialogRef.close(this.form.value);
    }

    public onCancelClicked(): void {
        this.dialogRef.close(null);
    }
}
