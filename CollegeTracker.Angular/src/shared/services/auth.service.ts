import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { UserModel } from '../../features/entities/user/user.model';
import { catchError, EMPTY, Observable, tap } from 'rxjs';
import { JwtTokenResponse } from '../../features/entities/token-response.model';
import { ApiEndpoints } from '../../constants/api-routes';
import { UserRole } from '../../features/entities/user/user-role.model';

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

    public getToken(): string | null {
        return localStorage.getItem('token');
    }

    public refreshToken(password: string) {
        const login = this.getCurrentUser().email;
        return this.authorizeUser(login, password);
    }

    public logout(): void {
        localStorage.clear();
    }

    public isAdmin(): boolean {
        return this.getCurrentUser().role == UserRole.Admin;
    }

    public isTeacher(): boolean {
        return this.getCurrentUser().role == UserRole.Teacher;
    }

    public isStudent(): boolean {
        return this.getCurrentUser().role == UserRole.Student;
    }

    public isSignedIn(): boolean {
        return this.getToken() != null;
    }

    public authorizeUser(login: string, password: string): Observable<JwtTokenResponse> {
        return this.http
            .get<JwtTokenResponse>(
                `${ApiEndpoints.Authorization}?login=${encodeURIComponent(login)}&password=${encodeURIComponent(password)}`
            )
            .pipe(
                tap((resp) => {
                    if (resp) {
                        this.saveUserInfo(resp.user);
                        this.saveToken(resp.token);
                    }
                }),
                catchError(() => {
                    return EMPTY;
                })
            );
    }
}
