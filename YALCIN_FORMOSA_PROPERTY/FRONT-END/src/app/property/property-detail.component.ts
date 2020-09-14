import { IProperty } from './property';
import { Component, OnInit } from "@angular/core";
import { ActivatedRoute, Router } from '@angular/router';
import { ApiService } from '../api.service';

@Component({
    templateUrl: './property-detail.component.html',
    styleUrls:['./property-style.component.css'],
    providers: [ApiService]
})

export class PropertyDetailComponent
                             implements OnInit
{
    private errorMessage: any;
    private pageTitle: String = "Property: ";
    property: IProperty;

    constructor(private _route: ActivatedRoute,
                private _router: Router,
                private _apiService: ApiService) {}

    ngOnInit()
    {
        let id = this._route.snapshot.paramMap.get('id');

        const param = this._route.snapshot.paramMap.get('id');
        if(param)
        {
            const id = param;
            this.getProperty(id);
        }
    }
    getProperty(id: string) {
        this._apiService.getProperty(id).subscribe(
            property => this.property = property,
            error => this.errorMessage = <any>error);
    }

    onBack(): void
    {
        this._router.navigate(['/property']);
    }
}
