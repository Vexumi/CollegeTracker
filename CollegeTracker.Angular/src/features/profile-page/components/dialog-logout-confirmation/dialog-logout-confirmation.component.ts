import { Component } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';

@Component({
    standalone: true,
    selector: 'app-dialog-logout-confirmation',
    templateUrl: './dialog-logout-confirmation.component.html',
    styleUrls: ['./dialog-logout-confirmation.component.scss'],
    imports: [],
    providers: []
})
export class DialogLogoutConfirmationComponent {    
    constructor(private readonly dialogRef: MatDialogRef<DialogLogoutConfirmationComponent>) {}

    public onOkClicked(): void {
        this.dialogRef.close(true);
    }
    public onCancelClicked(): void {
        this.dialogRef.close(false);
    }
}
