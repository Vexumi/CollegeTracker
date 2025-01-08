import { AsyncPipe } from '@angular/common';
import { Component } from '@angular/core';
import { SubjectModel } from '../../entities/subject/subject.model';
import { SubjectService } from '../../entities/subject/subject.service';
import { EMPTY, Observable, switchMap, tap } from 'rxjs';
import { MatDialog } from '@angular/material/dialog';
import { DialogAddEditSubjectComponent } from './components/dialog-add-edit/dialog-add-edit.component';

@Component({
    standalone: true,
    selector: 'app-subjects',
    templateUrl: './subjects.component.html',
    styleUrls: ['./subjects.component.scss'],
    imports: [AsyncPipe]
})
export class SubjectsPageComponent {
    public subjects$: Observable<SubjectModel[]>;

    constructor(
        private readonly subjectService: SubjectService,
        private readonly dialogSerivce: MatDialog
    ) {
        this.subjects$ = subjectService.getAllSubjects();
    }

    public onAddClicked(): void {
        const dialogRef = this.dialogSerivce.open(DialogAddEditSubjectComponent, {
            data: 
            {
                isEdit: false
            }
        });
      
        dialogRef.afterClosed()
            .pipe(
                switchMap((result) => {
                    if (!result) return EMPTY;
                    return this.subjectService.createSubject(result)
                        .pipe(
                            tap(() => this.subjects$ = this.subjectService.getAllSubjects())
                        );
                })
            )
            .subscribe();
    }

    public onEditClicked(subject: SubjectModel): void {
        const dialogRef = this.dialogSerivce.open(DialogAddEditSubjectComponent, {
            data: 
            {
                isEdit: true,
                subject: subject
            }
        });
      
        dialogRef.afterClosed()
            .pipe(
                switchMap((result) => {
                    if (!result) return EMPTY;
                    return this.subjectService.editSubject(result)
                        .pipe(
                            tap(() => this.subjects$ = this.subjectService.getAllSubjects())
                        );
                })
            )
            .subscribe();
    }

    public onChangeActiveClicked(id: number): void {
        this.subjectService.changeActive(id).pipe(
            tap(() => this.subjects$ = this.subjectService.getAllSubjects())
        )
        .subscribe();
    }
}
