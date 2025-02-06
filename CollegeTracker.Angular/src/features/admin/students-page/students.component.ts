import { AsyncPipe } from '@angular/common';
import { Component } from '@angular/core';
import { EMPTY, Observable, switchMap, tap } from 'rxjs';
import { MatDialog } from '@angular/material/dialog';
import { StudentService } from '../../entities/student/student.service';
import { StudentModel } from '../../entities/student/student.model';
import { DialogAddEditStudentComponent } from './components/dialog-add-edit/dialog-add-edit-student.component';
import { PhonePipe } from '../../../shared/pipes/phone-number/phone.pipe';

@Component({
    standalone: true,
    selector: 'app-groups',
    templateUrl: './students.component.html',
    styleUrls: ['./students.component.scss'],
    imports: [AsyncPipe, PhonePipe]
})
export class StudentsPageComponent {
    public subjects$: Observable<StudentModel[]>;

    constructor(
        private readonly studentsService: StudentService,
        private readonly dialogSerivce: MatDialog
    ) {
        this.subjects$ = studentsService.getAll();
    }

    public onAddClicked(): void {
        const dialogRef = this.dialogSerivce.open(DialogAddEditStudentComponent, {
            data: 
            {
                isEdit: false
            }
        });
      
        dialogRef.afterClosed()
            .pipe(
                switchMap((result) => {
                    if (!result) return EMPTY;
                    return this.studentsService.create(result)
                        .pipe(
                            tap(() => this.subjects$ = this.studentsService.getAll())
                        );
                })
            )
            .subscribe();
    }

    public onEditClicked(model: StudentModel): void {
        const dialogRef = this.dialogSerivce.open(DialogAddEditStudentComponent, {
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
                    return this.studentsService.edit(result)
                        .pipe(
                            tap(() => this.subjects$ = this.studentsService.getAll())
                        );
                })
            )
            .subscribe();
    }

    public onChangeActiveClicked(id: number): void {
        this.studentsService.changeActive(id).pipe(
            tap(() => this.subjects$ = this.studentsService.getAll())
        )
        .subscribe();
    }
}
