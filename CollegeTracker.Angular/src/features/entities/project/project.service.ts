import { Injectable } from '@angular/core';
import { ApiEndpoints } from '../../../constants/api-routes';
import { BaseService } from '../base.service';
import { ProjectModel } from './project.model';
import { Observable } from 'rxjs';

@Injectable({
    providedIn: 'root'
})
export class ProjectService extends BaseService<ProjectModel> {
    constructor() {
        super(ApiEndpoints.Projects);
    }


    public getById(id: number): Observable<ProjectModel> {
        return this.http.get<ProjectModel>(`${this.baseControllerUrl}/GetById/${id}`);
    }
}
