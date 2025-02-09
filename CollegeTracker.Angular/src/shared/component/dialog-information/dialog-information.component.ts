import { Component, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { ReactiveFormsModule } from '@angular/forms';

interface DialogData {
    header: string,
    body: string | null
}

@Component({
    standalone: true,
    selector: 'app-dialog-information',
    templateUrl: './dialog-information.component.html',
    styleUrls: ['./dialog-information.component.scss'],
    imports: [ReactiveFormsModule]
})
export class DialogInformationComponent {

    public readonly headerText: string;
    public readonly bodyText: string | null;

    constructor(
        private readonly dialogRef: MatDialogRef<DialogInformationComponent>,
        @Inject(MAT_DIALOG_DATA) private readonly data: DialogData
    ) {
        this.headerText = this.data.header;
        this.bodyText = this.data.body;
    }

    public onOkClicked(): void {
        this.dialogRef.close();
    }
}
