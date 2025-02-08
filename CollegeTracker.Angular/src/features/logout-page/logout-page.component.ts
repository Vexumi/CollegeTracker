import { Component, OnInit } from "@angular/core";
import { Router } from "@angular/router";
import { AuthService } from "../../shared/services/auth.service";
import { AppRoutes } from "../../constants/app-routes";

@Component({
    template: ''
})
  
export class LogoutPageComponent implements OnInit {
    constructor(
        private readonly authService: AuthService, 
        private readonly router: Router
    ) {}

    ngOnInit() {
        this.authService.logout();
        this.router.navigate([AppRoutes.Authorization]);
    }
}