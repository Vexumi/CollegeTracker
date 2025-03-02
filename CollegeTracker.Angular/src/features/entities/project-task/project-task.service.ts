import { Injectable } from '@angular/core';
import { ApiEndpoints } from '../../../constants/api-routes';
import { BaseService } from '../base.service';
import { ProjectTaskModel } from './project-task.model';
import { ProjectTaskStateEnum } from './project-task-state.enum';
import { Observable } from 'rxjs';

@Injectable({
    providedIn: 'root'
})
export class ProjectTaskService extends BaseService<ProjectTaskModel> {
    constructor() {
        super(ApiEndpoints.ProjectTasks);
    }

    public changeState(id: number, state: ProjectTaskStateEnum): Observable<number> {
        return this.http.post<number>(`${this.baseControllerUrl}/ChangeState`, { id, state });
    }
}
