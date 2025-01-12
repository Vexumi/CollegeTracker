import { AsyncPipe, DatePipe } from '@angular/common';
import { Component } from '@angular/core';
import { EMPTY, Observable, switchMap, tap } from 'rxjs';
import { MatDialog } from '@angular/material/dialog';
import { GroupService } from '../../entities/group/group.service';
import { GroupModel } from '../../entities/group/group.model';
import { DialogAddEditGroupComponent } from './components/dialog-add-edit/dialog-add-edit-group.component';

@Component({
    standalone: true,
    selector: 'app-groups',
    templateUrl: './groups.component.html',
    styleUrls: ['./groups.component.scss'],
    imports: [AsyncPipe, DatePipe]
})
export class GroupsPageComponent {
    public subjects$: Observable<GroupModel[]>;

    constructor(
        private readonly groupsService: GroupService,
        private readonly dialogSerivce: MatDialog
    ) {
        this.subjects$ = groupsService.getAll();
    }

    public onAddClicked(): void {
        const dialogRef = this.dialogSerivce.open(DialogAddEditGroupComponent, {
            data: 
            {
                isEdit: false
            }
        });
      
        dialogRef.afterClosed()
            .pipe(
                switchMap((result) => {
                    if (!result) return EMPTY;
                    return this.groupsService.create(result)
                        .pipe(
                            tap(() => this.subjects$ = this.groupsService.getAll())
                        );
                })
            )
            .subscribe();
    }

    public onEditClicked(model: GroupModel): void {
        const dialogRef = this.dialogSerivce.open(DialogAddEditGroupComponent, {
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
                    return this.groupsService.edit(result)
                        .pipe(
                            tap(() => this.subjects$ = this.groupsService.getAll())
                        );
                })
            )
            .subscribe();
    }

    public onChangeActiveClicked(id: number): void {
        this.groupsService.changeActive(id).pipe(
            tap(() => this.subjects$ = this.groupsService.getAll())
        )
        .subscribe();
    }
}
