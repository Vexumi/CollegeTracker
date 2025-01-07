import { Component } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { AuthService } from '../entities/user/auth.service';
import { Router } from '@angular/router';

@Component({
    standalone: true,
    selector: 'app-auth',
    templateUrl: './auth.component.html',
    styleUrls: ['./auth.component.scss'],
    imports: [ReactiveFormsModule]
})
export class AuthPageComponent {
    public form = new FormGroup({
        login: new FormControl<string>('', [Validators.minLength(3), Validators.required]),
        password: new FormControl<string>('', [Validators.minLength(3), Validators.required])
    });

    constructor(
        private readonly authService: AuthService,
        private readonly router: Router
    ) {}

    public signIn(): void {
        const authData = this.form.value;
        this.authService.authorizeUser(authData.login!, authData.password!).subscribe((result) => {
            if (result) {
                this.router.navigate(['/profile']);
            }
        });
    }
}
