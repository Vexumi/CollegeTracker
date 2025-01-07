import { Component } from '@angular/core';
import { PhonePipe } from '../../shared/pipes/phone-number/phone.pipe';
import { AuthService } from '../entities/user/auth.service';
import { UserModel } from '../entities/user/user.model';

@Component({
    standalone: true,
    selector: 'app-profile',
    templateUrl: './profile.component.html',
    styleUrls: ['./profile.component.scss'],
    imports: [PhonePipe]
})
export class ProfilePageComponent {
    public readonly currentUser: UserModel;

    constructor(private readonly authService: AuthService) {
        console.log(this.authService.getCurrentUser());
        this.currentUser = this.authService.getCurrentUser();
    }
}
