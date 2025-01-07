import { Component } from '@angular/core';
import { ciNSURoutes } from '../../../constants/external-routes';
import { CollegeInfo } from '../../../constants/ci-nsu-info';

@Component({
    standalone: true,
    selector: 'app-footer',
    templateUrl: './footer.component.html',
    styleUrls: ['./footer.component.scss'],
    imports: []
})
export class FooterComponent {
    public readonly currentYear = new Date().getFullYear();
    public readonly email = CollegeInfo.email;
    public readonly phoneNumber = CollegeInfo.phone;

    public readonly ciNsuRoutes = ciNSURoutes;
}
