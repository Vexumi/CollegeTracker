import { Component } from '@angular/core';
import { PhonePipe } from '../../shared/pipes/phone-number/phone.pipe';

@Component({
  standalone: true,
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.scss'],
  imports: [PhonePipe]
})
export class ProfilePageComponent {}