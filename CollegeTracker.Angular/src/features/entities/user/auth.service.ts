import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { UserModel } from './user.model';
import { catchError, EMPTY, Observable, tap } from 'rxjs';
import { JwtTokenResponse } from '../token-response.model';

@Injectable({
    providedIn: 'root'
})
export class AuthService {
    constructor(private readonly http: HttpClient) {}

    public saveToken(token: string): void {
        localStorage.setItem('token', token);
    }

    public saveUserInfo(user: UserModel): void {
        localStorage.setItem('user', JSON.stringify(user));
    }

    public getCurrentUser(): UserModel {
        return JSON.parse(localStorage.getItem('user')!);
    }

    public authorizeUser(login: string, password: string): Observable<JwtTokenResponse> {
        return this.http
            .get<JwtTokenResponse>(
                `api/authorize?login=${encodeURIComponent(login)}&password=${encodeURIComponent(password)}`
            )
            .pipe(
                tap((resp) => {
                    this.saveUserInfo(resp.user);
                    this.saveToken(resp.token);
                }),
                catchError(() => {
                    return EMPTY;
                })
            );
    }
}
