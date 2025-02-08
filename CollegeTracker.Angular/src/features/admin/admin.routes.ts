import { Routes } from "@angular/router";
import { AdminGuard } from "../../authorization/guards/admin.guard";
import { AppRoutes } from "../../constants/app-routes";
import { GroupsPageComponent } from "./groups-page/groups.component";
import { SpecialitiesPageComponent } from "./specialities-page/specialities.component";
import { StudentsPageComponent } from "./students-page/students.component";
import { SubjectsPageComponent } from "./subjects-page/subjects.component";
import { TeachersPageComponent } from "./teachers-page/teachers.component";

export const AdminRoutes: Routes = [
    {
        path: AppRoutes.Admin,
        canActivateChild: [AdminGuard],
        children: [
            {
                path: AppRoutes.AdminRoutes.Subjects,
                pathMatch: 'full',
                component: SubjectsPageComponent
            },
            {
                path: AppRoutes.AdminRoutes.Specialities,
                pathMatch: 'full',
                component: SpecialitiesPageComponent
            },
            {
                path: AppRoutes.AdminRoutes.Groups,
                pathMatch: 'full',
                component: GroupsPageComponent
            },
            {
                path: AppRoutes.AdminRoutes.Students,
                pathMatch: 'full',
                component: StudentsPageComponent
            },
            {
                path: AppRoutes.AdminRoutes.Teachers,
                pathMatch: 'full',
                component: TeachersPageComponent
            },
        ]
    }
]