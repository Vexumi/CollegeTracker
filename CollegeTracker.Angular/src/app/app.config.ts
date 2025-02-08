import { ApplicationConfig, importProvidersFrom } from '@angular/core';
import { provideRouter } from '@angular/router';

import { routes } from './app.routes';
import { HttpClientModule, provideHttpClient } from '@angular/common/http';
import { provideAnimationsAsync } from '@angular/platform-browser/animations/async';
import { httpInterceptorsProvider } from '../authorization/interceptors/interceptors.provider';
import { guardsProvider } from '../authorization/guards/guards.provider';

export const appConfig: ApplicationConfig = {
    providers: [
        provideRouter(routes), 
        provideHttpClient(), 
        provideAnimationsAsync(),
        importProvidersFrom(HttpClientModule),
        httpInterceptorsProvider,
        guardsProvider
    ]
};
