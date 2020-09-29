import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot } from '@angular/router';
import { isNull } from 'util';

@Injectable()
export class PropertyGuardService  
                        implements CanActivate{

    constructor(){}

    canActivate(route: ActivatedRouteSnapshot): boolean
    {
        let id = route.url[1].path;

        if (isNull(id))
        {
            window.alert("Invalid Property Id");
            return false;
        }
        else
        {
            return true;
        }
    }
}