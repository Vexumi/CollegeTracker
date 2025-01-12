import { Injectable } from '@angular/core';
import { ApiEndpoints } from '../../../constants/api-routes';
import { SpecialityModel } from './speciality.model';
import { BaseService } from '../base.service';

@Injectable({
    providedIn: 'root'
})
export class SpecialityService extends BaseService<SpecialityModel> {
    constructor() {
        super(ApiEndpoints.Specialities);
    }
}
