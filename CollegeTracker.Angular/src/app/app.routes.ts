import { Routes } from '@angular/router';
import { AuthPageComponent } from '../features/auth-page/auth.component';
import { NotFoundPageComponent } from '../features/not-found-page/not-found.component';
import { MainPageComponent } from '../features/website-structure/main-page.component';
import { ProfilePageComponent } from '../features/profile-page/profile.component';

export const routes: Routes = [
    {
        path: '',
        component: MainPageComponent,
        children: [
            {
                path: 'profile',
                pathMatch: 'full',
                component: ProfilePageComponent
            }
        ]
    },
    { path: 'sign-in', component: AuthPageComponent },
    { path: '**', component: NotFoundPageComponent }
];
