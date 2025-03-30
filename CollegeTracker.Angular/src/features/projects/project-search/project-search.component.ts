import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { ProjectService } from '../../entities/project/project.service';
import { BehaviorSubject, combineLatest, map, switchMap } from 'rxjs';
import { ProjectCardComponent } from './components/project-card/project-card.component';
import { ProjectSearchParamsComponent } from './components/project-search-params/project-search-params.component';
import { ProjectSearchParamsModel } from '../../entities/project/project-search-params.model';

@Component({
    standalone: true,
    selector: 'app-project-search',
    templateUrl: './project-search.component.html',
    styleUrls: ['./project-search.component.scss'],
    imports: [CommonModule, ProjectCardComponent, ProjectSearchParamsComponent]
})
export class ProjectSearchComponent {
    public readonly pageSizes = [6, 12, 18];
    public pageSize$ = new BehaviorSubject<number>(this.pageSizes[0]);
    public page$ = new BehaviorSubject<number>(1);
    public searchParams$ = new BehaviorSubject<ProjectSearchParamsModel>({} as ProjectSearchParamsModel);
    public projects$ = combineLatest([this.pageSize$, this.page$, this.searchParams$]).pipe(
        map((request) => {
            const [pageSize, page, params] = request;
            params.pageSize = pageSize;
            params.page = page - 1;
            return params;
        }),
        switchMap((params) => this.projectService.searchProjects(params))
    )

    constructor(
        private readonly projectService: ProjectService
    ) {}

    public onSearchClicked(searchParams: ProjectSearchParamsModel) {
        this.searchParams$.next(searchParams);
    }
}
