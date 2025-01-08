import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { SubjectModel } from './subject.model';
import { ApiEndpoints } from '../../../constants/api-routes';

@Injectable({
    providedIn: 'root'
})
export class SubjectService {
    constructor(private readonly http: HttpClient) {}

    public getAllSubjects(): Observable<SubjectModel[]> {
        return this.http.get<SubjectModel[]>(`${ApiEndpoints.Subjects}/GetAll`);
    }


    public createSubject(subject: SubjectModel): Observable<number> {
        return this.http.post<number>(`${ApiEndpoints.Subjects}/Create`, subject);
    }

    public editSubject(subject: SubjectModel): Observable<SubjectModel> {
        return this.http.post<SubjectModel>(`${ApiEndpoints.Subjects}/Update`, subject);
    }
}
