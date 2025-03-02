import { Injectable } from '@angular/core';
import { ApiEndpoints } from '../../../constants/api-routes';
import { BaseService } from '../base.service';
import { ProjectTaskModel } from './project-task.model';

@Injectable({
    providedIn: 'root'
})
export class ProjectTaskService extends BaseService<ProjectTaskModel> {
    constructor() {
        super(ApiEndpoints.ProjectTasks);
    }
}
