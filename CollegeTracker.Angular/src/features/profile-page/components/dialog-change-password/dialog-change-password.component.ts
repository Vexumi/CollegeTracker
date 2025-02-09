import { Component } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';

interface DataResult {
    oldPassword: string;
    newPassword: string;
}

@Component({
    standalone: true,
    selector: 'app-dialog-change-password',
    templateUrl: './dialog-change-password.component.html',
    styleUrls: ['./dialog-change-password.component.scss'],
    imports: [ReactiveFormsModule],
    providers: []
})
export class DialogChangePasswordComponent {
    private readonly minPasswordLength = 6;
    private readonly maxPasswordLength = 12;
    
    public form = new FormGroup({
        oldPassword: new FormControl<string | null>(null, [Validators.required]),
        newPassword: new FormControl<string>('', [Validators.required, Validators.minLength(this.minPasswordLength), Validators.maxLength(this.maxPasswordLength)]),
        newPasswordConfirmation: new FormControl<string>('', [Validators.required, Validators.minLength(this.minPasswordLength), Validators.maxLength(this.maxPasswordLength)]),
    });

    constructor(private readonly dialogRef: MatDialogRef<DialogChangePasswordComponent>) {}

    public onOkClicked(): void {
        const result: DataResult = {
            newPassword: this.form.controls.newPassword.value!,
            oldPassword: this.form.controls.oldPassword.value!
        }
        this.dialogRef.close(result);
    }

    public onCancelClicked(): void {
        this.dialogRef.close(null);
    }

    public passwordLengthInvalid(): boolean {
        const newPwd = this.form.controls.newPassword.value ?? '';
        const newPwdConf = this.form.controls.newPasswordConfirmation.value ?? '';

        return (newPwd.length < 6 || newPwd.length > 12 || newPwdConf.length < 6 || newPwdConf.length > 12) && newPwd.length != 0 && newPwdConf.length != 0;
    }

    public passwordsAreEqual(): boolean {
        const newPwd = this.form.controls.newPassword.value ?? '';
        const newPwdConf = this.form.controls.newPasswordConfirmation.value ?? '';
        return newPwd == newPwdConf;
    }

    public buttonSaveDisabled(): boolean {
        return !this.form.valid || !this.passwordsAreEqual();
    }
}
