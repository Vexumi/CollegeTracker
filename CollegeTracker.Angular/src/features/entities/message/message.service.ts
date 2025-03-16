import { Injectable } from '@angular/core';
import { ApiEndpoints } from '../../../constants/api-routes';
import { BaseService } from '../base.service';
import { MessageModel } from './message.model';
import { Observable, startWith } from 'rxjs';

@Injectable({
    providedIn: 'root'
})
export class MessageService extends BaseService<MessageModel> {
    constructor() {
        super(ApiEndpoints.Message);
    }

    public getAllByProject(projectId: number): Observable<MessageModel[]> {
        return this.http.get<MessageModel[]>(`${this.baseControllerUrl}/GetAll/${projectId}`).pipe(startWith([]));
    }
}
