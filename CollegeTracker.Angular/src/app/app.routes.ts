import { Routes } from '@angular/router';
import { AuthPageComponent } from '../authorization/auth-page/auth.component';
import { NotFoundPageComponent } from '../features/not-found-page/not-found.component';
import { MainPageComponent } from '../features/website-structure/main-page.component';
import { ProfilePageComponent } from '../features/profile-page/profile.component';
import { AppRoutes } from '../constants/app-routes';
import { LogoutPageComponent } from '../authorization/logout-page/logout-page.component';
import { SignedInGuard } from '../authorization/guards/signed-in.guard';
import { AdminRoutes } from '../features/admin/admin.routes';

export const routes: Routes = [
    {
        path: AppRoutes.Home,
        component: MainPageComponent,
        children: [
            {
                path: AppRoutes.Profile,
                pathMatch: 'full',
                canActivate: [SignedInGuard],
                component: ProfilePageComponent
            },
            ...AdminRoutes
        ]
    },
    { path: AppRoutes.Logout, component: LogoutPageComponent },
    { path: AppRoutes.Authorization, component: AuthPageComponent },
    { path: '**', component: NotFoundPageComponent }
];
