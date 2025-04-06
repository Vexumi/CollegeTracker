import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { ProjectService } from '../../entities/project/project.service';
import { BehaviorSubject, combineLatest, EMPTY, map, switchMap, tap } from 'rxjs';
import { ProjectCardComponent } from './components/project-card/project-card.component';
import { ProjectSearchParamsComponent } from './components/project-search-params/project-search-params.component';
import { ProjectSearchParamsModel } from '../../entities/project/project-search-params.model';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatSelectModule } from '@angular/material/select';
import { FormControl, ReactiveFormsModule } from '@angular/forms';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { ActivatedRoute } from '@angular/router';
import { AppRoutes } from '../../../constants/app-routes';
import { AuthService } from '../../../shared/services/auth.service';
import { ReportsService } from '../../entities/reports/reports.service';
import { DialogCreateProjectComponent } from './components/dialog-create-project/dialog-create-project.component';
import { MatDialog } from '@angular/material/dialog';

@Component({
    standalone: true,
    selector: 'app-project-search',
    templateUrl: './project-search.component.html',
    styleUrls: ['./project-search.component.scss'],
    //encapsulation: ViewEncapsulation.None, TODO: page size selector style
    imports: [CommonModule, ProjectCardComponent, ProjectSearchParamsComponent, MatFormFieldModule, MatSelectModule, ReactiveFormsModule]
})
export class ProjectSearchComponent {
    private readonly userOnlyProjects: boolean;
    private readonly currentUserId: number;

    public readonly pageSizes = [6, 12, 18];

    public readonly buttonExportToExcelVisible = this.authService.isAdmin();

    public pageSize$ = new BehaviorSubject<number>(this.pageSizes[0]);
    public page$ = new BehaviorSubject<number>(1);
    public totalPages$ = new BehaviorSubject<number>(0);
    public searchParams$ = new BehaviorSubject<ProjectSearchParamsModel>({} as ProjectSearchParamsModel);
    public projects$ = combineLatest([this.pageSize$, this.page$, this.searchParams$]).pipe(
        map((request) => {
            const [pageSize, page, params] = request;
            params.pageSize = pageSize;
            params.page = page - 1;

            params.currentUserId = this.userOnlyProjects ? this.currentUserId : null;

            return params;
        }),
        switchMap((params) => this.projectService.searchProjects(params).pipe(tap((x) => this.totalPages$.next(x.totalPages)), map((x) => x.projects)))
    )

    public pageSizeForm = new FormControl<number>(this.pageSizes[0]);

    constructor(
        private readonly projectService: ProjectService,
        private readonly route: ActivatedRoute,
        private readonly authService: AuthService,
        private readonly reportsService: ReportsService,
        private readonly dialogService: MatDialog
    ) {
        this.userOnlyProjects = this.route.snapshot.url[0].path === AppRoutes.MyProjects;
        this.currentUserId = this.authService.getCurrentUser().id;
        this.pageSizeForm.valueChanges.pipe(takeUntilDestroyed()).subscribe((() => this.pageSize$.next(this.pageSizeForm.value!)))
    }

    public onSearchClicked(searchParams: ProjectSearchParamsModel) {
        this.searchParams$.next(searchParams);
    }

    public buttonPrevPageVisible(): boolean {
        return this.page$.value > 1;
    }
    
    public buttonNextPageVisible(): boolean {
        return this.totalPages$.value > this.page$.value;
    }

    public prevPage() {
        this.page$.next(this.page$.value - 1);
    }

    public nextPage() {
        this.page$.next(this.page$.value + 1);
    }

    public getCurrentPage() {
        return this.totalPages$.value != 0 ? this.page$.value : 0;
    }

    public exportToExcel() {
        this.reportsService.exportProjects(this.searchParams$.value);
    }

    public createProject() {
        const dialogRef = this.dialogService.open(DialogCreateProjectComponent);
        dialogRef.afterClosed()
            .pipe(
                switchMap((result) => {
                    if (!result) return EMPTY;
                    return this.projectService.create(result)
                        .pipe(
                            tap(() => this.page$.next(1))
                        );
                })
            )
            .subscribe();
    }
}
