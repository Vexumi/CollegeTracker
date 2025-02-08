/* eslint-disable @typescript-eslint/no-unused-vars */
import { Injectable, Inject } from "@angular/core";
import { ActivatedRouteSnapshot, Router, RouterStateSnapshot } from "@angular/router";
import { AuthService } from "../../shared/services/auth.service";
import { AppRoutes } from "../../constants/app-routes";

@Injectable()
export class TeacherGuard {
    constructor(
        @Inject(AuthService) private readonly authService: AuthService,
        @Inject(Router) private readonly router: Router
    ) {}

    canActivate(
        _next: ActivatedRouteSnapshot,
        _state: RouterStateSnapshot
    ): boolean {
        const isTeacher = this.authService.isTeacher();

        if (!isTeacher) {
            this.router.navigate([AppRoutes.Profile]);
        }

        return isTeacher;
    }

    canActivateChild(
        next: ActivatedRouteSnapshot,
        state: RouterStateSnapshot
    ): boolean {
        return this.canActivate(next, state);
    }
}