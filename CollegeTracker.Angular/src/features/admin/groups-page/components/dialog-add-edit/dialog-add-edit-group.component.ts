import { Component, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { GroupModel } from '../../../../entities/group/group.model';
import { DropdownSelectIdComponent, DropdownSelectIdItem } from '../../../../../shared/component/dropdown/dropdown.component';
import { SpecialityService } from '../../../../entities/speciality/speciality.service';
import { map, Observable } from 'rxjs';
import { AsyncPipe, DatePipe } from '@angular/common';

interface DialogData {
    isEdit: boolean,
    model: GroupModel | null
}

@Component({
    standalone: true,
    selector: 'app-dialog-add-edit-group',
    templateUrl: './dialog-add-edit-group.component.html',
    styleUrls: ['./dialog-add-edit-group.component.scss'],
    imports: [ReactiveFormsModule, DropdownSelectIdComponent, AsyncPipe],
    providers: [DatePipe]
})
export class DialogAddEditGroupComponent {
    public readonly isEdit: boolean;

    public readonly specialities$: Observable<DropdownSelectIdItem[]>;

    public readonly form = new FormGroup({
        number: new FormControl("", [Validators.required, Validators.minLength(3)]),
        launchDate: new FormControl<string | null>(null, [Validators.required]),
        stopDate: new FormControl<string | null>(null, [Validators.required]),
        specialityId: new FormControl<number | null>(null, [Validators.required]),
    });

    constructor(
        private readonly specialitiesService: SpecialityService,
        private readonly dialogRef: MatDialogRef<DialogAddEditGroupComponent>,
        @Inject(MAT_DIALOG_DATA) private readonly data: DialogData,
        private readonly datePipe: DatePipe
    ) {
        this.specialities$ = this.specialitiesService.getAllActive()
            .pipe(
                map(res => res.map((x) => ({ title: x.title, id: x.id } as DropdownSelectIdItem)))
            );
        this.isEdit = this.data.isEdit;
        if (this.isEdit) {
            this.form.controls.number.setValue(data.model!.number);
            this.form.controls.launchDate.setValue(this.datePipe.transform(data.model!.launchDate, "yyyy-MM-dd"));
            this.form.controls.stopDate.setValue(this.datePipe.transform(data.model!.stopDate, "yyyy-MM-dd"));
            this.form.controls.specialityId.setValue(data.model!.specialityId);
        }
    }

    public formValid(): boolean {
        const formValue = this.form.value;
        const launchDate = formValue.launchDate ? new Date(formValue.launchDate) : new Date();
        const stopDate = formValue.stopDate ? new Date(formValue.stopDate) : new Date();
        return this.form.valid && launchDate <= stopDate;
    }

    public onSpecialitySelected(selected: DropdownSelectIdItem) {
        this.form.controls.specialityId.setValue(selected.id);
    }

    public onSaveClicked(): void {
        const resultValues = this.form.value;

        const result = {
            ...resultValues,
            launchDate: new Date(resultValues.launchDate!),
            stopDate: new Date(resultValues.stopDate!),
        } as GroupModel;

        if (this.isEdit) {
            result.id = this.data.model!.id;
        }

        this.dialogRef.close(result);
    }

    public onCancelClicked(): void {
        this.dialogRef.close(null);
    }

    public getCurrentSpeciality(): DropdownSelectIdItem | null {
        if (this.isEdit) {
            return { title: this.data.model!.speciality.title, id: this.data.model!.speciality.id };
        }
        return null;
    }
}
