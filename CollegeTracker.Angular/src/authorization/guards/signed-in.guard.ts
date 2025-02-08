/* eslint-disable @typescript-eslint/no-unused-vars */
import { Injectable, Inject } from "@angular/core";
import { ActivatedRouteSnapshot, Router, RouterStateSnapshot } from "@angular/router";
import { AuthService } from "../../shared/services/auth.service";
import { AppRoutes } from "../../constants/app-routes";

@Injectable()
export class SignedInGuard {
    constructor(
        @Inject(AuthService) private readonly authService: AuthService,
        @Inject(Router) private readonly router: Router
    ) {}

    canActivate(
        _next: ActivatedRouteSnapshot,
        _state: RouterStateSnapshot
    ): boolean {
        const signedIn = this.authService.isSignedIn();
        if (!signedIn) {
            this.router.navigate([AppRoutes.Authorization]);
        }
        return signedIn;
    }

    canActivateChild(
        next: ActivatedRouteSnapshot,
        state: RouterStateSnapshot
    ): boolean {
        return this.canActivate(next, state);
    }
}