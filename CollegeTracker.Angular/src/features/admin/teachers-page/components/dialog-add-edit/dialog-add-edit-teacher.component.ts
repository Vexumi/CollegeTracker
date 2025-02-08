import { Component, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { DropdownSelectIdItem } from '../../../../../shared/component/dropdown/dropdown.component';
import { map, Observable } from 'rxjs';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { AsyncPipe, NgIf } from '@angular/common';
import { GroupService } from '../../../../entities/group/group.service';
import { UserGeneratorService } from '../../../../../shared/services/user-generator.service';
import { TeacherModel } from '../../../../entities/teacher/teacher.model';
import { TeacherDtoModel } from '../../../../entities/teacher/teacher-dto.model';
import { DropdownMultiSelectIdsComponent } from '../../../../../shared/component/dropdown-multiselect/dropdown-multiselect.component';

interface DialogData {
    isEdit: boolean,
    model: TeacherModel | null
}

@Component({
    standalone: true,
    selector: 'app-dialog-add-edit-teacher',
    templateUrl: './dialog-add-edit-teacher.component.html',
    styleUrls: ['./dialog-add-edit-teacher.component.scss'],
    imports: [ReactiveFormsModule, DropdownMultiSelectIdsComponent, AsyncPipe, NgIf],
    providers: []
})
export class DialogAddEditTeacherComponent {
    public readonly isEdit: boolean;
    public readonly groups$: Observable<DropdownSelectIdItem[]>;

    public readonly form = new FormGroup({
        email: new FormControl<string | null>(null, [Validators.required]),
        phoneNumber: new FormControl<string | null>(null, [Validators.required]),
        fullname: new FormControl<string | null>(null, [Validators.required]),
        username: new FormControl<string | null>(null, [Validators.required]),
        password: new FormControl<string | null>(null, [Validators.required]),
        groupIds: new FormControl<number[]>([], [Validators.required]),
    });

    constructor(
        private readonly groupsService: GroupService,
        private readonly dialogRef: MatDialogRef<DialogAddEditTeacherComponent>,
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
            this.form.controls.groupIds.setValue(data.model!.groups.map(x => x.id));
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

    public onGroupSelected(selected: DropdownSelectIdItem[]) {
        this.form.controls.groupIds.setValue(selected.map(x => x.id));
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
            groupIds: this.form.controls.groupIds.value
        } as TeacherDtoModel;

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

    public getCurrentGroups(): DropdownSelectIdItem[] {
        if (this.isEdit) {
            return this.data.model!.groups.map((x) => ({ title: x.number, id: x.id }));
        }
        return [];
    }
}
