import { Injectable } from '@angular/core';
import { ApiEndpoints } from '../../../constants/api-routes';
import { BaseService } from '../base.service';
import { StudentModel } from './student.model';

@Injectable({
    providedIn: 'root'
})
export class StudentService extends BaseService<StudentModel> {
    constructor() {
        super(ApiEndpoints.Students);
    }
}
