import { Injectable } from '@angular/core';
import { ApiEndpoints } from '../../../constants/api-routes';
import { BaseService } from '../base.service';
import { GroupModel } from './group.model';

@Injectable({
    providedIn: 'root'
})
export class GroupService extends BaseService<GroupModel> {
    constructor() {
        super(ApiEndpoints.Group);
    }
}
