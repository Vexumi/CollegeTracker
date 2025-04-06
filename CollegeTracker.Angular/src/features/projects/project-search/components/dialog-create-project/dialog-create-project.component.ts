import { Component } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { DropdownSelectIdComponent, DropdownSelectIdItem } from '../../../../../shared/component/dropdown/dropdown.component';
import { map, Observable, of } from 'rxjs';
import { SpecialityService } from '../../../../entities/speciality/speciality.service';
import { TeacherService } from '../../../../entities/teacher/teacher.service';
import { AsyncPipe, NgIf } from '@angular/common';
import { GroupService } from '../../../../entities/group/group.service';
import { DropdownMultiSelectIdsComponent } from '../../../../../shared/component/dropdown-multiselect/dropdown-multiselect.component';
import { StudentService } from '../../../../entities/student/student.service';
import { ProjectCreateDTOModel } from '../../../../entities/project/project-create-dto.model';

@Component({
    standalone: true,
    selector: 'app-dialog-create-project',
    templateUrl: './dialog-create-project.component.html',
    styleUrls: ['./dialog-create-project.component.scss'],
    imports: [ReactiveFormsModule, DropdownSelectIdComponent, AsyncPipe, DropdownMultiSelectIdsComponent, NgIf]
})
export class DialogCreateProjectComponent {
    public readonly specialities$: Observable<DropdownSelectIdItem[]> = of([]);
    public readonly teachers$: Observable<DropdownSelectIdItem[]> = of([]);
    public readonly groups$: Observable<DropdownSelectIdItem[]> = of([]);
    public readonly students$: Observable<DropdownSelectIdItem[]> = of([]);

    public formGroup = new FormGroup({
        title: new FormControl<string>('', [Validators.required]),
        description: new FormControl<string | null>(null),
        teacherId: new FormControl<number | null>(null, [Validators.required]),
        specialityId: new FormControl<number | null>(null, [Validators.required]),
        startDate: new FormControl<Date>(new Date(), [Validators.required]),
        deadline: new FormControl<Date>(new Date(), [Validators.required]),
        studentIds: new FormControl<number[]>([], []),
        groupIds: new FormControl<number[]>([], [])
    })

    constructor(
        private readonly dialogRef: MatDialogRef<DialogCreateProjectComponent>,
        private readonly specialityService: SpecialityService,
        private readonly teacherService: TeacherService,
        private readonly groupsService: GroupService,
        private readonly studentService: StudentService
    ) {
        this.specialities$ = this.specialityService.getAllActive()
            .pipe(
                map(res => res.map((x) => ({ title: x.title, id: x.id } as DropdownSelectIdItem)))
            );
        
        this.teachers$ = this.teacherService.getAllActive()
            .pipe(
                map(res => res.map((x) => ({ title: x.userInfo.fullname, id: x.id } as DropdownSelectIdItem)))
            );

        this.groups$ = this.groupsService.getAllActive()
            .pipe(
                map(res => res.map((x) => ({ title: x.number, id: x.id } as DropdownSelectIdItem)))
            );

        this.students$ = this.studentService.getAllActive()
            .pipe(
                map(res => res.map((x) => ({ title: x.userInfo.fullname, id: x.id } as DropdownSelectIdItem)))
            );
    }

    public onChangeClicked(): void {
        const projectDto = this.formGroup.value as ProjectCreateDTOModel;
        this.dialogRef.close(projectDto);
    }

    public onCancelClicked(): void {
        this.dialogRef.close(null);
    }

    public onSpecialitySelected(selected: DropdownSelectIdItem) {
        this.formGroup.controls.specialityId.setValue(selected.id);
    }

    public onTeacherSelected(selected: DropdownSelectIdItem) {
        this.formGroup.controls.teacherId.setValue(selected.id);
    }

    public onGroupsSelected(selected: DropdownSelectIdItem[]) {
        this.formGroup.controls.groupIds.setValue(selected.map(x => x.id!));
    }

    public onStudentsSelected(selected: DropdownSelectIdItem[]) {
        this.formGroup.controls.studentIds.setValue(selected.map(x => x.id!));
    }

    public buttonSaveDisabled() {
        return !this.formGroup.valid || (this.formGroup.controls.groupIds.value?.length == 0 && this.formGroup.controls.studentIds.value?.length == 0);
    }

    public getGroupsBySpeciality(groups: DropdownSelectIdItem[]) {
        const specialityId = this.formGroup.value.specialityId;
        return groups.filter((x) => specialityId != null ? x.id == specialityId : true);
    }
}
