import { Routes } from '@angular/router';
import { AuthPageComponent } from '../features/auth-page/auth.component';
import { NotFoundPageComponent } from '../features/not-found-page/not-found.component';
import { MainPageComponent } from '../features/website-structure/main-page.component';
import { ProfilePageComponent } from '../features/profile-page/profile.component';
import { AppRoutes } from '../constants/app-routes';

export const routes: Routes = [
    {
        path: AppRoutes.Home,
        component: MainPageComponent,
        children: [
            {
                path: AppRoutes.Profile,
                pathMatch: 'full',
                component: ProfilePageComponent
            }
        ]
    },
    { path: AppRoutes.Authorization, component: AuthPageComponent },
    { path: '**', component: NotFoundPageComponent }
];
