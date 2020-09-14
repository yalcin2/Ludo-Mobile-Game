import { Component, OnInit } from '@angular/core';
import { IProperty } from '../property/property';
import { ApiService } from '../api.service';

@Component({
    templateUrl: './home.component.html',
    styleUrls:['./home.component.css'],
    providers: [ApiService]

})
export class HomeComponent
                implements OnInit {
    errorMessage: any;
    public pageTitle: string = 'Home Page';
    Properties: IProperty[];
    _listFilter: string;
    
    constructor(private _apiService: ApiService)
    {
        this.Properties = this.properties;
    }

    properties: IProperty[] =  [];

    ngOnInit() : void
    {
        this._apiService.getProperties()
            .subscribe(properties => { 
                this.properties = properties;
                this.Properties = this.properties;
            },
            error => this.errorMessage = <any>error);
    }
}