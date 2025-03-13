import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { ProjectModel } from '../../../../entities/project/project.model';
import { DropdownSelectIdComponent, DropdownSelectIdItem } from '../../../../../shared/component/dropdown/dropdown.component';
import { map, Observable } from 'rxjs';
import { SpecialityService } from '../../../../entities/speciality/speciality.service';
import { TeacherService } from '../../../../entities/teacher/teacher.service';
import { AsyncPipe } from '@angular/common';
import { ProjectDTOModel } from '../../../../entities/project/project-dto.model';

interface DialogData {
    model: ProjectModel
}

@Component({
    standalone: true,
    selector: 'app-dialog-edit-project',
    templateUrl: './dialog-edit-project.component.html',
    styleUrls: ['./dialog-edit-project.component.scss'],
    imports: [ReactiveFormsModule, DropdownSelectIdComponent, AsyncPipe]
})
export class DialogEditProjectComponent {
    public readonly specialities$: Observable<DropdownSelectIdItem[]>;
    public readonly teachers$: Observable<DropdownSelectIdItem[]>;

    public formGroup = new FormGroup({
        title: new FormControl<string>('', [Validators.required]),
        description: new FormControl<string | null>(null),
        teacherId: new FormControl<number>(0, [Validators.required]),
        specialityId: new FormControl<number>(0, [Validators.required]),
        startDate: new FormControl<Date>(new Date(), [Validators.required]),
        deadline: new FormControl<Date>(new Date(), [Validators.required]),
    })

    constructor(
        private readonly dialogRef: MatDialogRef<DialogEditProjectComponent>,
        @Inject(MAT_DIALOG_DATA) private readonly data: DialogData,
        private readonly specialityService: SpecialityService,
        private readonly teacherService: TeacherService
    ) {
        const project = this.data.model;
        this.formGroup.controls.title.setValue(project.title);
        this.formGroup.controls.description.setValue(project.description);
        this.formGroup.controls.teacherId.setValue(project.teacher.id);
        this.formGroup.controls.specialityId.setValue(project.speciality.id);
        this.formGroup.controls.startDate.setValue(project.startDate);
        this.formGroup.controls.deadline.setValue(project.deadline);

        this.specialities$ = this.specialityService.getAllActive()
            .pipe(
                map(res => res.map((x) => ({ title: x.title, id: x.id } as DropdownSelectIdItem)))
            );
        
        this.teachers$ = this.teacherService.getAllActive()
            .pipe(
                map(res => res.map((x) => ({ title: x.userInfo.fullname, id: x.id } as DropdownSelectIdItem)))
            );
    }

    public onChangeClicked(): void {
        const projectDto = this.formGroup.value as ProjectDTOModel;
        projectDto.id = this.data.model.id;
        this.dialogRef.close(projectDto);
    }

    public onCancelClicked(): void {
        this.dialogRef.close(null);
    }

    public getCurrentSpeciality(): DropdownSelectIdItem | null {
        return { title: this.data.model!.speciality.title, id: this.data.model!.speciality.id };
    }

    public onSpecialitySelected(selected: DropdownSelectIdItem) {
        this.formGroup.controls.specialityId.setValue(selected.id);
    }

    public getCurrentTeacher(): DropdownSelectIdItem | null {
        return { title: this.data.model!.teacher.userInfo.fullname, id: this.data.model!.teacher.id };
    }

    public onTeacherSelected(selected: DropdownSelectIdItem) {
        this.formGroup.controls.teacherId.setValue(selected.id);
    }
}
