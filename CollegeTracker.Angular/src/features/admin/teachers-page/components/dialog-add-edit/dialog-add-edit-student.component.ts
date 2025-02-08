import { Component, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { DropdownSelectIdComponent, DropdownSelectIdItem } from '../../../../../shared/component/dropdown/dropdown.component';
import { map, Observable } from 'rxjs';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { AsyncPipe, NgIf } from '@angular/common';
import { StudentModel } from '../../../../entities/student/student.model';
import { GroupService } from '../../../../entities/group/group.service';
import { UserGeneratorService } from '../../../../../shared/services/user-generator.service';

interface DialogData {
    isEdit: boolean,
    model: StudentModel | null
}

@Component({
    standalone: true,
    selector: 'app-dialog-add-edit-student',
    templateUrl: './dialog-add-edit-student.component.html',
    styleUrls: ['./dialog-add-edit-student.component.scss'],
    imports: [ReactiveFormsModule, DropdownSelectIdComponent, AsyncPipe, NgIf],
    providers: []
})
export class DialogAddEditStudentComponent {
    public readonly isEdit: boolean;
    public readonly groups$: Observable<DropdownSelectIdItem[]>;

    public readonly form = new FormGroup({
        email: new FormControl<string | null>(null, [Validators.required]),
        phoneNumber: new FormControl<string | null>(null, [Validators.required]),
        fullname: new FormControl<string | null>(null, [Validators.required]),
        username: new FormControl<string | null>(null, [Validators.required]),
        password: new FormControl<string | null>(null, [Validators.required]),
        groupId: new FormControl<number | null>(null, [Validators.required]),
    });

    constructor(
        private readonly groupsService: GroupService,
        private readonly dialogRef: MatDialogRef<DialogAddEditStudentComponent>,
        private readonly userGenerator: UserGeneratorService,
        @Inject(MAT_DIALOG_DATA) private readonly data: DialogData
    ) {
        this.groups$ = this.groupsService.getAllActive()
            .pipe(
                map(res => res.map((x) => ({ title: x.number, id: x.id } as DropdownSelectIdItem)))
            );
        this.isEdit = this.data.isEdit;
        if (this.isEdit) {
            const userInfo = data.model!.userInfo;
            this.form.controls.email.setValue(userInfo.email);
            this.form.controls.phoneNumber.setValue(userInfo.phoneNumber);
            this.form.controls.fullname.setValue(userInfo.fullname);
            this.form.controls.username.setValue(userInfo.username);
            this.form.controls.groupId.setValue(data.model!.groupId);
            this.form.controls.password.removeValidators(Validators.required);
        }
        else {
            this.form.controls.fullname.valueChanges.pipe(takeUntilDestroyed()).subscribe(() => this.regenerateUsername());
            this.form.controls.password.setValue(this.userGenerator.generatePassword());
        }
    }

    public formValid(): boolean {
        return this.form.valid;
    }

    public onGroupSelected(selected: DropdownSelectIdItem) {
        this.form.controls.groupId.setValue(selected.id);
    }

    public onSaveClicked(): void {
        const result = {
            userInfo: {
                id: this.data.model?.id,
                email: this.form.controls.email.value,
                phoneNumber: this.form.controls.phoneNumber.value,
                fullname: this.form.controls.fullname.value,
                username: this.form.controls.username.value,
                password: this.form.controls.password.value,
            },
            groupId: this.form.controls.groupId.value
        } as StudentModel;

        if (this.isEdit) {
            result.id = this.data.model!.id;
        }

        this.dialogRef.close(result);
    }

    public onCancelClicked(): void {
        this.dialogRef.close(null);
    }

    public regenerateUsername() {
        const fullname = this.form.controls.fullname.value;
        if (this.isEdit || fullname == null) return;

        const username = this.userGenerator.generateUserName(fullname);
        if (username == null) return;
        this.form.controls.username.setValue(username);
    }

    public getCurrentGroup(): DropdownSelectIdItem | null {
        if (this.isEdit) {
            return { title: this.data.model!.group.number, id: this.data.model!.groupId };
        }
        return null;
    }
}
