import { Routes } from '@angular/router';
import { AuthPageComponent } from '../features/auth-page/auth.component';
import { NotFoundPageComponent } from '../features/not-found-page/not-found.component';
import { MainPageComponent } from '../features/website-structure/main-page.component';
import { UserPageComponent } from '../features/user-page/user-page.component';

export const routes: Routes = [
    { 
        path: "", 
        component: MainPageComponent,
        children: [
            {
                path: "profile",
                pathMatch: "full",
                component: UserPageComponent
            }
        ]
    },
    { path: "sign-in", component: AuthPageComponent },
    { path: "**", component: NotFoundPageComponent }
];
