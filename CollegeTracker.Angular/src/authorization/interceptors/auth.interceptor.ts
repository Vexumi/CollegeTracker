/* eslint-disable @typescript-eslint/no-explicit-any */
import { HttpInterceptor, HttpRequest, HttpHandler, HttpEvent, HttpErrorResponse } from "@angular/common/http";
import { inject, Injectable } from "@angular/core";
import { catchError, Observable, of } from "rxjs";
import { AuthService } from "../../shared/services/auth.service";
import { Router } from "@angular/router";
import { AppRoutes } from "../../constants/app-routes";

const WHITE_LIST_ERROR_CODES = [400];

@Injectable()
export class AuthInterceptor implements HttpInterceptor {
    private readonly authService = inject(AuthService);
    private readonly router = inject(Router);

    intercept(
        req: HttpRequest<any>,
        next: HttpHandler
    ): Observable<HttpEvent<any>> {
        const authReq = req.clone({
            setHeaders: {
              'Authorization': `Bearer ${this.authService.getToken()}`,
            },
          });

        return next.handle(authReq).pipe(
            catchError((e: HttpErrorResponse) => {
                if (e.status == 401) {
                    this.router.navigate([AppRoutes.Logout]);
                    return of();
                }

                if (WHITE_LIST_ERROR_CODES.includes(e.status)) {
                    return of();
                }

                console.log('Http request error catched', e);
                return of();
              })
        );
    }
}