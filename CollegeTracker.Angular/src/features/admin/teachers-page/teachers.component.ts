import { AsyncPipe } from '@angular/common';
import { Component } from '@angular/core';
import { EMPTY, Observable, switchMap, tap } from 'rxjs';
import { MatDialog } from '@angular/material/dialog';
import { PhonePipe } from '../../../shared/pipes/phone-number/phone.pipe';
import { TeacherModel } from '../../entities/teacher/teacher.model';
import { TeacherService } from '../../entities/teacher/teacher.service';
import { GroupModel } from '../../entities/group/group.model';
import { DialogAddEditTeacherComponent } from './components/dialog-add-edit/dialog-add-edit-teacher.component';

@Component({
    standalone: true,
    selector: 'app-teachers',
    templateUrl: './teachers.component.html',
    styleUrls: ['./teachers.component.scss'],
    imports: [AsyncPipe, PhonePipe]
})
export class TeachersPageComponent {
    public subjects$: Observable<TeacherModel[]>;

    constructor(
        private readonly baseService: TeacherService,
        private readonly dialogSerivce: MatDialog
    ) {
        this.subjects$ = baseService.getAll();
    }

    public getGroups(groups: GroupModel[]): string {
        return groups.map(x => x.number).join(', ');
    }

    public onAddClicked(): void {
        const dialogRef = this.dialogSerivce.open(DialogAddEditTeacherComponent, {
            data: 
            {
                isEdit: false
            }
        });
      
        dialogRef.afterClosed()
            .pipe(
                switchMap((result) => {
                    if (!result) return EMPTY;
                    return this.baseService.create(result)
                        .pipe(
                            tap(() => this.subjects$ = this.baseService.getAll())
                        );
                })
            )
            .subscribe();
    }

    public onEditClicked(model: TeacherModel): void {
        const dialogRef = this.dialogSerivce.open(DialogAddEditTeacherComponent, {
            data: 
            {
                isEdit: true,
                model: model
            }
        });
      
        dialogRef.afterClosed()
            .pipe(
                switchMap((result) => {
                    if (!result) return EMPTY;
                    return this.baseService.edit(result)
                        .pipe(
                            tap(() => this.subjects$ = this.baseService.getAll())
                        );
                })
            )
            .subscribe();
    }

    public onChangeActiveClicked(id: number): void {
        this.baseService.changeActive(id).pipe(
            tap(() => this.subjects$ = this.baseService.getAll())
        )
        .subscribe();
    }
}
