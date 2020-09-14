import { Component, OnInit } from '@angular/core';
import { IProperty } from './property';
import { ApiService } from '../api.service';

@Component({
    selector: 'test',
    templateUrl: './property-list.component.html',
    styleUrls:['./property-style.component.css'],
    providers: [ApiService]
})
export class PropertyListComponent
                    implements OnInit {
    private errorMessage: any;
    private filteredProperties: IProperty[];
    
    private pageTitle: string = 'Property Page';
    private showImage: boolean = true;

    private _listFilter: string;

    get listFilter(): string {
        return this._listFilter;
    }
    set listFilter(value:string) {
        this._listFilter = value;
        this.filteredProperties = this._listFilter ? this.performFilter(this.listFilter) : this.properties;
    }

    constructor(private _apiService: ApiService) {
        this.filteredProperties = this.properties;
        this.pagination();
    }

    performFilter(filterBy: string): IProperty[] {
        filterBy = filterBy.toLocaleLowerCase();
        return this.properties.filter((property: IProperty) =>
            property.location.toLocaleLowerCase().indexOf(filterBy) !== -1);
    }

    deleteProperty(property: IProperty) {
        this._apiService.deleteProperty(property._id).subscribe(response => 
            this.properties = this.properties.filter(properties => properties !== property),)   
            window.location.reload();
    }

    properties: IProperty[] =  [];

    toggleImage(): void { 
        this.showImage =! this.showImage
    };

    pagination(): void{
        this.properties.push();
    }

    ngOnInit() : void {
        this._apiService.getProperties()
            .subscribe(properties => { 
                this.properties = properties;
                this.filteredProperties = this.properties;
            },
            error => this.errorMessage = <any>error);
    } 
}