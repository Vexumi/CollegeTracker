import { Injectable } from '@angular/core';
import { ApiEndpoints } from '../../../constants/api-routes';
import { BaseService } from '../base.service';
import { TeacherModel } from './teacher.model';

@Injectable({
    providedIn: 'root'
})
export class TeacherService extends BaseService<TeacherModel> {
    constructor() {
        super(ApiEndpoints.Teachers);
    }
}
