import { HttpErrorResponse } from '@angular/common/http'
import { Http,Headers } from '@angular/http';
import { Injectable } from '@angular/core';

import { Observable } from 'rxjs/Observable';
import { PropertyListComponent } from './property/property-list.component';
import { IProperty } from './property/property';
import { SignUp } from './sign-up/sign-up';

import 'rxjs/add/operator/map'
import 'rxjs/add/observable/throw'
import 'rxjs/add/operator/catch'
import 'rxjs/add/operator/do'


@Injectable()
export class ApiService{
    properties = [];
    constructor (private http: Http){}
    
    getProperties(): Observable<IProperty[]> {
        return this.http.get('http://localhost:3000/propertylist/')
            .map(res => res.json())
            .map(res => <IProperty[]>res.props);
    }

    getProperty(id: string): Observable<IProperty> {
        return this.getProperties()
            .map((properties: IProperty[]) => properties.find(p => p._id === id));
    }

    addProperty(props: IProperty) {
        let headers = new Headers;
        let body = JSON.stringify({propertyType: props.propertyType,
                                   location: props.location,
                                   price: props.price,
                                   description: props.description,
                                   imageUrl: props.imageUrl});
        headers.append('Content-Type', 'application/json');
        return this.http.post('http://localhost:3000/propertylist/', body ,{headers: headers})
        .map(res => res.json());
    }

    deleteProperty(id : string) {
        let LINK = `http://localhost:3000/propertylist/${id}`;
          let headers = new Headers;
          headers.append('Content-Type', 'application/json');
          return this.http.delete(LINK, {headers: headers})
          .map(res => res.json());
    }


    addUser(users: SignUp) {
        let headers = new Headers;
        let body = JSON.stringify({firstName: users.firstName,
                                   lastName: users.lastName,
                                   email: users.email,
                                   mobile: users.mobile,
                                   catalog: users.catalog,
                                   notification: users.notification});
        headers.append('Content-Type', 'application/json');
        return this.http.post('http://localhost:3000/registrationlist/', body ,{headers: headers})
        .map(res => res.json());
    }

    private handleError(err: HttpErrorResponse) {
        console.error(err.message); 
        return Observable.throw(err.message); 
    }
}
