import { HttpClient } from '@angular/common/http';
import { inject } from '@angular/core';
import { Observable } from 'rxjs';

export class BaseService<T> {
    private readonly http = inject(HttpClient);

    constructor(private readonly baseControllerUrl: string) {}

    public getAll(): Observable<T[]> {
        return this.http.get<T[]>(`${this.baseControllerUrl}/GetAll`);
    }

    public create(model: T): Observable<number> {
        return this.http.post<number>(`${this.baseControllerUrl}/Create`, model);
    }

    public edit(model: T): Observable<T> {
        return this.http.post<T>(`${this.baseControllerUrl}/Update`, model);
    }

    public changeActive(id: number): Observable<void> {
        return this.http.post<void>(`${this.baseControllerUrl}/ChangeActivityState/${id}`, null);
    }
}
