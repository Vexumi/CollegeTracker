import { AsyncPipe } from '@angular/common';
import { Component } from '@angular/core';
import { SubjectModel } from '../../entities/subject/subject.model';
import { EMPTY, Observable, switchMap, tap } from 'rxjs';
import { MatDialog } from '@angular/material/dialog';
import { DialogAddEditSpecialityComponent } from './components/dialog-add-edit/dialog-add-edit-speciality.component';
import { SpecialityService } from '../../entities/speciality/speciality.service';
import { SpecialityModel } from '../../entities/speciality/speciality.model';

@Component({
    standalone: true,
    selector: 'app-specialities',
    templateUrl: './specialities.component.html',
    styleUrls: ['./specialities.component.scss'],
    imports: [AsyncPipe]
})
export class SpecialitiesPageComponent {
    public subjects$: Observable<SpecialityModel[]>;

    constructor(
        private readonly specialitiesService: SpecialityService,
        private readonly dialogSerivce: MatDialog
    ) {
        this.subjects$ = specialitiesService.getAll();
    }

    public onAddClicked(): void {
        const dialogRef = this.dialogSerivce.open(DialogAddEditSpecialityComponent, {
            data: 
            {
                isEdit: false
            }
        });
      
        dialogRef.afterClosed()
            .pipe(
                switchMap((result) => {
                    if (!result) return EMPTY;
                    return this.specialitiesService.create(result)
                        .pipe(
                            tap(() => this.subjects$ = this.specialitiesService.getAll())
                        );
                })
            )
            .subscribe();
    }

    public onEditClicked(subject: SubjectModel): void {
        const dialogRef = this.dialogSerivce.open(DialogAddEditSpecialityComponent, {
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
                    return this.specialitiesService.edit(result)
                        .pipe(
                            tap(() => this.subjects$ = this.specialitiesService.getAll())
                        );
                })
            )
            .subscribe();
    }

    public onChangeActiveClicked(id: number): void {
        this.specialitiesService.changeActive(id).pipe(
            tap(() => this.subjects$ = this.specialitiesService.getAll())
        )
        .subscribe();
    }
}
