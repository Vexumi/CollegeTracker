import { Provider } from '@angular/core';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { AuthInterceptor } from './auth.interceptor';

export const httpInterceptorsProvider: Provider = [
    { provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi: true }
];