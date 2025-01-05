import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { HeaderComponent } from '../features/website-structure/header/header.component';
import { SideMenuComponent } from '../features/website-structure/side-menu/side-menu.component';
import { FooterComponent } from '../features/website-structure/footer/footer.component';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, HeaderComponent, SideMenuComponent, FooterComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})
export class AppComponent {
  title = 'CollegeTracker';
}
