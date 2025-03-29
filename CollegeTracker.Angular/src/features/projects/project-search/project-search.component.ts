import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { ProjectService } from '../../entities/project/project.service';
import { ProjectModel } from '../../entities/project/project.model';
import { Observable } from 'rxjs';
import { ProjectCardComponent } from './components/project-card/project-card.component';
import { ProjectSearchParamsComponent } from './components/project-search-params/project-search-params.component';

@Component({
    standalone: true,
    selector: 'app-project-search',
    templateUrl: './project-search.component.html',
    styleUrls: ['./project-search.component.scss'],
    imports: [CommonModule, ProjectCardComponent, ProjectSearchParamsComponent]
})
export class ProjectSearchComponent {
    public projects$: Observable<ProjectModel[]>;

    constructor(
        private readonly projectService: ProjectService
    ) {
        this.projects$ = this.projectService.getAll();
    }
}
