import { Component } from '@angular/core';

@Component({
    templateUrl: './about.component.html',
    styleUrls: ['/about.component.css']
})
export class AboutUsComponent {
    public pageTitle: string = 'About Us';
    lat: number = 35.9375;
    lng: number = 14.3754;
}
