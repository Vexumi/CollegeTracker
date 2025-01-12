import { Component } from '@angular/core';
import { CompanyWebsiteBaseRoute } from '../../../constants/external-routes';
import { CompanyInfo } from '../../../constants/company-info';

@Component({
    standalone: true,
    selector: 'app-footer',
    templateUrl: './footer.component.html',
    styleUrls: ['./footer.component.scss'],
    imports: []
})
export class FooterComponent {
    public readonly currentYear = new Date().getFullYear();
    public readonly email = CompanyInfo.email;
    public readonly phoneNumber = CompanyInfo.phone;
    public readonly address = CompanyInfo.address;
    public readonly subtitle = CompanyInfo.subtitle;

    public readonly companyWebsiteRoute = CompanyWebsiteBaseRoute;
}
