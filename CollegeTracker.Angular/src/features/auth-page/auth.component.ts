import { Component } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';

@Component({
  standalone: true,
  selector: 'app-auth',
  templateUrl: './auth.component.html',
  styleUrls: ['./auth.component.scss'],
  imports: [ReactiveFormsModule]
})
export class AuthPageComponent {
  public form = new FormGroup({
    login: new FormControl<string>("", [Validators.minLength(3), Validators.required]),
    password: new FormControl<string>("", [Validators.minLength(3), Validators.required]),
  })

  public signIn(): void {
    console.log(this.form.value);
  }
}